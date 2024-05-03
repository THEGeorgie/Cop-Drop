// json library
using Newtonsoft.Json.Linq;
using System.Configuration;

//assembley
using System.Reflection;

namespace CopDrop
{
    public class Map
    {
        int MAP_WIDTH;
        int MAP_HEIGHT;
        List<Sprite> spriteObjects = new List<Sprite>();
        List<Sprite> tile = new List<Sprite>();
        List<Button> btnObjects = new List<Button>();
        List<IntPtr> surfaces = new List<IntPtr>();
        List<TextWrapper> textObjects = new List<TextWrapper>();
        List<Player> playerObjects = new List<Player>();
        JArray json;
        JObject miscellaneous;
        Collision collision;
        MapTileManager mapTileManager;

        JArray backgroundColor;

        //Reads the provided json then builds the map
        public Map(string jsonPath, int MAP_WIDTH, int MAP_HEIGHT)
        {
            if (File.Exists(jsonPath))
            {
                this.MAP_WIDTH = MAP_WIDTH;
                this.MAP_HEIGHT = MAP_HEIGHT;
                // Read the JSON file
                string jsonContent = File.ReadAllText(jsonPath);
                Console.WriteLine("The file exist");
                // Parse the JSON content as a dynamic object
                json = JArray.Parse(jsonContent);
                // objects used  in general (background color assets to use etc.)
                miscellaneous = (JObject)json[0];
                collision = new Collision();
                mapBuilder();
            }
            else
            {
                Console.WriteLine(jsonPath);
                Console.WriteLine("The JSON file does not exist.");
            }
        }

        public Sprite getComponent(double type, int index)
        {
            return spriteObjects[index];
        }

        public void updateSpritePosition(int index, int x, int y)
        {
            if (x != null)
            {
                spriteObjects[index].transform.x = x;
                spriteObjects[index].collision.setCollisionBoxValue(spriteObjects[index].CollisionID, x, 'x');
            }
            else
            {
                Console.WriteLine("X cannot be null");
            }
            if (y != null)
            {
                spriteObjects[index].transform.y = y;
                spriteObjects[index].collision.setCollisionBoxValue(spriteObjects[index].CollisionID, y, 'y');
            }
            else
            {
                Console.WriteLine("Y cannot be null");
            }


        }

        public int insertComponent(Sprite sprite)
        {
            spriteObjects.Add(sprite);
            return (spriteObjects.Count - 1);
        }
        public int addComponent(double type, string datas)
        {
            JToken data = JToken.Parse(datas);

            SDL_Rect sourceRect = new SDL_Rect();
            SDL_Rect destinationRect = new SDL_Rect();
            SDL_Color textColor = new SDL_Color();
            switch (type)
            {
                case 1.0:
                    var destinationSurface = IntPtr.Zero;
                    // menas if it has multiple surfaces in one row it combines it into one texture
                    if ((char)data["alignOn"] == 'x')
                    {
                        // creates the surface where we will copy the images to 
                        destinationSurface = SDL.SDL_CreateRGBSurface(0, (int)data["w"] * (int)data["quantity"], (int)data["h"], 32, 0, 0, 0, 0);
                    }
                    else if ((char)data["alignOn"] == 'y')
                    {
                        // creates the surface where we will copy the images to 
                        destinationSurface = SDL.SDL_CreateRGBSurface(0, (int)data["w"], (int)data["h"] * (int)data["quantity"], 32, 0, 0, 0, 0);
                    }
                    uint color = SDL.SDL_MapRGB(SDL.SDL_AllocFormat(SDL.SDL_PIXELFORMAT_ARGB8888), (byte)backgroundColor[0], (byte)backgroundColor[0], (byte)backgroundColor[0]);

                    SDL.SDL_FillRect(destinationSurface, IntPtr.Zero, color);
                    SDL_Surface surfaceInfo = (SDL.SDL_Surface)System.Runtime.InteropServices.Marshal.PtrToStructure(surfaces[(int)data["surfaceIndex"]], typeof(SDL.SDL_Surface));

                    sourceRect.x = (int)data["sourceX"];
                    sourceRect.y = (int)data["sourceY"];
                    sourceRect.w = surfaceInfo.w;
                    sourceRect.h = surfaceInfo.h;
                    destinationRect.h = (int)data["h"];
                    destinationRect.w = (int)data["w"];


                    for (int i = 0; i < (int)data["quantity"]; i++)
                    {
                        if ((char)data["alignOn"] == 'x')
                        {
                            destinationRect.x = ((int)data["w"] + (int)data["margin"]) * i;
                            destinationRect.y = 0;
                        }
                        else if ((char)data["alignOn"] == 'y')
                        {
                            destinationRect.y = ((int)data["h"] + (int)data["margin"]) * i;
                            destinationRect.x = 0;
                        }
                        //combinig the surfaces
                        SDL_BlitSurface(surfaces[(int)data["surfaceIndex"]], ref sourceRect, destinationSurface, ref destinationRect);

                    }
                    createSprite(data, destinationSurface, 1);
                    return (spriteObjects.Count - 1);
                    break;
                // 2 means its a button object 
                case 2.0:
                    createButton(data);
                    return (btnObjects.Count - 1);
                    break;
                //means its a button with text
                case 2.3:
                    createButtonWithText(data, textColor);
                    return (btnObjects.Count - 1);
                    break;
                // means its a text object
                case 3.0:
                    createText(data, 1, textColor);
                    return (textObjects.Count - 1);
                    break;
                // means its a player object
                case 4.0:
                    createPlayer(data);
                    return (playerObjects.Count - 1);
                    break;
                default:
                    break;
            }

            return (int)IntPtr.Zero;
        }
        void mapBuilder()
        {
            SDL_Rect sourceRect = new SDL_Rect();
            SDL_Rect destinationRect = new SDL_Rect();
            SDL_Color textColor = new SDL_Color();

            JArray locationSurfaces = (JArray)miscellaneous["assetLocation"];
            backgroundColor = (JArray)miscellaneous["backGroundColor"];

            for (int i = 0; i < locationSurfaces.Count; i++)
            {
                surfaces.Add(SDL_image.IMG_Load(locationSurfaces[i].ToString()));
            }
            var destinationSurface = IntPtr.Zero;
            int count = 0;
            try
            {
                foreach (var type in json)
                {
                    count = 0;
                    // every type has its own way of building
                    if ((int)type["type"] != 0)
                    {
                        foreach (var objects in type["objects"])
                        {
                            //Creating reqquired configs for object instanciation
                            if ((int)type["type"] == 1)
                            {

                                // menas if it has multiple surfaces in one row it combines it into one texture
                                if ((char)objects["alignOn"] == 'x')
                                {
                                    // creates the surface where we will copy the images to 
                                    destinationSurface = SDL.SDL_CreateRGBSurface(0, (int)objects["w"] * (int)objects["quantity"], (int)objects["h"], 32, 0, 0, 0, 0);
                                }
                                else if ((char)objects["alignOn"] == 'y')
                                {
                                    // creates the surface where we will copy the images to 
                                    destinationSurface = SDL.SDL_CreateRGBSurface(0, (int)objects["w"], (int)objects["h"] * (int)objects["quantity"], 32, 0, 0, 0, 0);
                                }
                                uint color = SDL.SDL_MapRGB(SDL.SDL_AllocFormat(SDL.SDL_PIXELFORMAT_ARGB8888), (byte)backgroundColor[0], (byte)backgroundColor[0], (byte)backgroundColor[0]);

                                SDL.SDL_FillRect(destinationSurface, IntPtr.Zero, color);
                                SDL_Surface surfaceInfo = (SDL.SDL_Surface)System.Runtime.InteropServices.Marshal.PtrToStructure(surfaces[(int)objects["surfaceIndex"]], typeof(SDL.SDL_Surface));

                                sourceRect.x = (int)objects["sourceX"];
                                sourceRect.y = (int)objects["sourceY"];
                                sourceRect.w = surfaceInfo.w;
                                sourceRect.h = surfaceInfo.h;
                                destinationRect.h = (int)objects["h"];
                                destinationRect.w = (int)objects["w"];


                                for (int i = 0; i < (int)objects["quantity"]; i++)
                                {
                                    if ((char)objects["alignOn"] == 'x')
                                    {
                                        destinationRect.x = ((int)objects["w"] + (int)objects["margin"]) * i;
                                        destinationRect.y = 0;
                                    }
                                    else if ((char)objects["alignOn"] == 'y')
                                    {
                                        destinationRect.y = ((int)objects["h"] + (int)objects["margin"]) * i;
                                        destinationRect.x = 0;
                                    }
                                    //combinig the surfaces
                                    SDL_BlitSurface(surfaces[(int)objects["surfaceIndex"]], ref sourceRect, destinationSurface, ref destinationRect);

                                }
                            }
                            //Creating reqquired configs for object instanciation
                            else if ((int)type["type"] == 3 || (double)type["type"] == 2.3)
                            {
                                JArray colorArray = (JArray)objects["color"];
                                textColor = new SDL_Color
                                {
                                    r = (byte)colorArray[0],
                                    g = (byte)colorArray[1],
                                    b = (byte)colorArray[2],
                                    a = (byte)colorArray[3]
                                };
                            }

                            switch ((double)type["type"])
                            {
                                // 1 means its a texture object 
                                case 1.0:
                                    createSprite(objects, destinationSurface, count);
                                    break;
                                // 2 means its a button object 
                                case 2.0:
                                    createButton(objects);
                                    break;
                                //means its a button with text
                                case 2.3:
                                    createButtonWithText(objects, textColor);
                                    break;
                                // means its a text object
                                case 3.0:
                                    createText(objects, count, textColor);
                                    break;
                                // means its a player object
                                case 4.0:
                                    createPlayer(objects);
                                    break;
                                case 5.0:
                                    createTileMap(objects, count);
                                    break;
                                default:
                                    break;
                            }

                            count++;
                        }
                    }
                }
            }
            catch (Exception)
            {
                Console.WriteLine("An error occurred while building the map. Most likely, it's a JSON problem.");
                throw;
            }
        }
        void createSprite(JToken objects, nint destinationSurface, int count)
        {
            if ((string)objects["script"] != null)
            {
                ScriptCompiler scriptCompiler = new ScriptCompiler((string)objects["script"]);
                if ((char)objects["alignOn"] == 'x')
                {
                    spriteObjects.Add(new Sprite(destinationSurface, (int)objects["w"] * (int)objects["quantity"], (int)objects["h"], (int)objects["rotation"], (int)objects["x"], (int)objects["y"], loadScriptSPR(scriptCompiler.DllPath, scriptCompiler.ScriptClassName), collision));
                }
                else if ((char)objects["alignOn"] == 'y')
                {
                    spriteObjects.Add(new Sprite(destinationSurface, (int)objects["w"], (int)objects["h"] * (int)objects["quantity"], (int)objects["rotation"], (int)objects["x"], (int)objects["y"], loadScriptSPR(scriptCompiler.DllPath, scriptCompiler.ScriptClassName), collision));
                }
            }
            else
            {
                if ((char)objects["alignOn"] == 'x')
                {
                    spriteObjects.Add(new Sprite(destinationSurface, (int)objects["w"] * (int)objects["quantity"], (int)objects["h"], (int)objects["rotation"], (int)objects["x"], (int)objects["y"], null, collision));

                }
                else if ((char)objects["alignOn"] == 'y')
                {
                    spriteObjects.Add(new Sprite(destinationSurface, (int)objects["w"], (int)objects["h"] * (int)objects["quantity"], (int)objects["rotation"], (int)objects["x"], (int)objects["y"], null, collision));
                }
            }
            if ((bool)objects["collision"])
            {
                spriteObjects[spriteObjects.Count - 1].CollisionID = collision.addCollisionBox(new SDL_Rect { x = (int)objects["x"], y = (int)objects["y"], w = (int)objects["collisionBoxWidth"], h = (int)objects["collisionBoxHeight"] }, (bool)objects["debug"], (char)objects["type"]);
                spriteObjects[spriteObjects.Count - 1].collision = collision;

            }

            spriteObjects[spriteObjects.Count - 1].transform.x = (int)objects["x"];
            spriteObjects[spriteObjects.Count - 1].transform.y = (int)objects["y"];
        }
        void createSprite(JToken objects, int width, int height, int x, int y, int surfaceWidth, int surfaceHeight, int surfaceX, int surfaceY, char type, int count)
        {
            if ((string)objects["script"] != null)
            {
                ScriptCompiler scriptCompiler = new ScriptCompiler((string)objects["script"]);
                tile.Add(new Sprite(surfaces[(int)objects["surfaceIndex"]], width, height, (int)objects["rotation"], x, y, loadScriptSPR(scriptCompiler.DllPath, scriptCompiler.ScriptClassName), collision));
            }
            else
            {
                tile.Add(new Sprite(surfaces[(int)objects["surfaceIndex"]], width, height, (int)objects["rotation"], x, y, null, collision));
            }
            if ((bool)objects["collision"])
            {
                tile[tile.Count - 1].CollisionID = collision.addCollisionBox(new SDL_Rect { x = x, y = y, w = width, h = height }, (bool)objects["debug"], (char)objects["type"]);
                tile[tile.Count - 1].collision = collision;
                collision.addCollisionType(tile[tile.Count - 1].CollisionID, type);
            }
            tile[tile.Count - 1].transformSurface.w = surfaceWidth;
            tile[tile.Count - 1].transformSurface.h = surfaceHeight;
            tile[tile.Count - 1].transformSurface.x = surfaceX;
            tile[tile.Count - 1].transformSurface.y = surfaceY;
        }
        void createButton(JToken objects)
        {
            //There is a convertion from 
            JArray commandList = (JArray)objects["onPress"];

            string[] bufferCommands = new string[commandList.Count];

            for (int i = 0; i < commandList.Count; i++)
            {
                bufferCommands[i] = (string)commandList[i];
            }

            if ((string)objects["script"] != null)
            {
                ScriptCompiler scriptCompiler = new ScriptCompiler((string)objects["script"]);
                btnObjects.Add(new Button(surfaces[(int)objects["surfaceIndex"]], (int)objects["w"] * (int)objects["quantity"], (int)objects["h"], (int)objects["rotation"], (int)objects["x"], (int)objects["y"], bufferCommands, loadScriptBTN(scriptCompiler.DllPath, scriptCompiler.ScriptClassName)));
            }
            else
            {
                btnObjects.Add(new Button(surfaces[(int)objects["surfaceIndex"]], (int)objects["w"] * (int)objects["quantity"], (int)objects["h"], (int)objects["rotation"], (int)objects["x"], (int)objects["y"], bufferCommands, null));
            }

        }
        void createButtonWithText(JToken objects, SDL_Color textColor)
        {
            JArray commandList1 = (JArray)objects["onPress"];
            string[] bufferCommands1 = new string[commandList1.Count];

            for (int i = 0; i < commandList1.Count; i++)
            {
                bufferCommands1[i] = (string)commandList1[i];
            }


            Text txt = new Text((string)objects["text"], (int)objects["fontsize"], textColor);

            if ((string)objects["script"] != null)
            {
                ScriptCompiler scriptCompiler = new ScriptCompiler((string)objects["script"]);
                btnObjects.Add(new Button(surfaces[(int)objects["surfaceIndex"]], (int)objects["w"], (int)objects["h"], (int)objects["rotation"], txt, (int)objects["textX"], (int)objects["textY"], (int)objects["x"], (int)objects["y"], bufferCommands1, loadScriptBTN(scriptCompiler.DllPath, scriptCompiler.ScriptClassName)));
            }
            else
            {
                btnObjects.Add(new Button(surfaces[(int)objects["surfaceIndex"]], (int)objects["w"], (int)objects["h"], (int)objects["rotation"], txt, (int)objects["textX"], (int)objects["textY"], (int)objects["x"], (int)objects["y"], bufferCommands1, null));
            }
        }
        void createText(JToken objects, int count, SDL_Color textColor)
        {
            if ((string)objects["script"] != null)
            {
                ScriptCompiler scriptCompiler = new ScriptCompiler((string)objects["script"]);
                textObjects.Add(new TextWrapper((string)objects["text"], (int)objects["fontsize"], textColor, loadScriptTXT(scriptCompiler.DllPath, scriptCompiler.ScriptClassName)));
            }
            else
            {
                textObjects.Add(new TextWrapper((string)objects["text"], (int)objects["fontsize"], textColor, null));
            }
            textObjects[textObjects.Count - 1].x = (int)objects["x"];
            textObjects[textObjects.Count - 1].y = (int)objects["y"];
            textObjects[textObjects.Count - 1].update();
        }
        void createPlayer(JToken objects)
        {
            if ((string)objects["script"] != null)
            {
                ScriptCompiler scriptCompiler = new ScriptCompiler((string)objects["script"]);
                playerObjects.Add(new Player(surfaces[(int)objects["surfaceIndex"]], (int)objects["w"], (int)objects["h"], (int)objects["rotation"], (int)objects["x"], (int)objects["y"], loadScriptPLA(scriptCompiler.DllPath, scriptCompiler.ScriptClassName), collision));
            }
            else
            {
                playerObjects.Add(new Player(surfaces[(int)objects["surfaceIndex"]], (int)objects["w"], (int)objects["h"], (int)objects["rotation"], (int)objects["x"], (int)objects["y"], null, collision));
            }
            if ((bool)objects["collision"])
            {
                playerObjects[playerObjects.Count - 1].CollisionID = collision.addCollisionBox(new SDL_Rect { x = 0, y = 0, w = (int)objects["collisionBoxWidth"], h = (int)objects["collisionBoxHeight"] }, (bool)objects["debug"], (char)objects["type"]);
                playerObjects[playerObjects.Count - 1].collision = collision;
            }
        }
        void createTileMap(JToken objects, int count)
        {
            JArray tileMap = (JArray)objects["TileMap"];
            JArray tiles = (JArray)objects["Tiles"];
            JToken metaData = (JToken)objects["metaData"];

            int[,] map = new int[tileMap.Count(), (int)tileMap[0].Count()];
            for (int i = 0; i < tileMap.Count(); i++)
            {
                JArray innerArray = (JArray)tileMap[i];
                for (int j = 0; j < tileMap[0].Count(); j++)
                {
                    map[i, j] = (int)innerArray[j];
                }
            }

            int width = (int)metaData["TileWidth"];
            int height = (int)metaData["TileHeight"];
            int x, y;

            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {

                    x = j * width;
                    y = i * height;

                    createTile(tiles, map[i, j], width, height, x, y, count);
                    Console.Write($"TILE IS {map[i, j]}");
                }
            }
        }
        void updateWorldX(float x)
        {

        }
        void updateWorldY(float y)
        {

        }

        void createTile(JArray tiles, int type, int width, int height, int x, int y, int count)
        {
            for (int i = 0; i < tiles.Count; i++)
            {
                JToken tile = tiles[i];
                if ((int)tile["Description"] == type)
                {
                    Console.WriteLine($"Create tile x/y {x}/{y}");
                    createSprite(tile, width, height, x, y, (int)tile["sourceW"], (int)tile["sourceH"], (int)tile["sourceW"] * (int)tile["sourceX"], (int)tile["sourceH"] * (int)tile["sourceY"], 'a', count);
                }
            }
        }
        void DeleteDllFiles(string folderPath)
        {
            try
            {
                string[] files = Directory.GetFiles(folderPath, "*.dll");
                foreach (string file in files)
                {
                    File.Delete(file);
                    Console.WriteLine($"Deleted file: {file}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
        //Script loading methods
        private static IlinkButtonScripts loadScriptBTN(string assemblyPath, string className)
        {
            Assembly assembly = Assembly.LoadFrom(assemblyPath);
            //diagnostics
            var types = assembly.GetTypes();

            foreach (var item in types)
            {
                Console.WriteLine(item);
            }

            Type type = assembly.GetType("CopDrop." + className);

            if (type == null)
            {
                throw new InvalidOperationException("Class not found in the specified assembly.");
            }
            if (!typeof(CopDrop.IlinkButtonScripts)!.IsAssignableFrom(type))
            {
                throw new InvalidOperationException("Class does not implement IlinkButtonScripts interface.");
            }

            return (IlinkButtonScripts)Activator.CreateInstance(type);

        }

        private static IlinkPlayerScripts loadScriptPLA(string assemblyPath, string className)
        {
            Assembly assembly = Assembly.LoadFrom(assemblyPath);
            //diagnostics
            var types = assembly.GetTypes();

            foreach (var item in types)
            {
                Console.WriteLine(item);
            }

            Type type = assembly.GetType("CopDrop." + className);

            if (type == null)
            {
                throw new InvalidOperationException("Class not found in the specified assembly.");
            }
            if (!typeof(CopDrop.IlinkPlayerScripts)!.IsAssignableFrom(type))
            {
                throw new InvalidOperationException("Class does not implement IlinkPlayerScripts interface.");
            }

            return (IlinkPlayerScripts)Activator.CreateInstance(type);

        }

        private static IlinkSpriteScripts loadScriptSPR(string assemblyPath, string className)
        {
            Assembly assembly = Assembly.LoadFrom(assemblyPath);
            //diagnostics
            var types = assembly.GetTypes();

            foreach (var item in types)
            {
                Console.WriteLine(item);
            }

            Type type = assembly.GetType("CopDrop." + className);

            if (type == null)
            {
                throw new InvalidOperationException("Class not found in the specified assembly.");
            }
            if (!typeof(CopDrop.IlinkSpriteScripts)!.IsAssignableFrom(type))
            {
                throw new InvalidOperationException("Class does not implement IlinkSpriteScripts interface.");
            }

            return (IlinkSpriteScripts)Activator.CreateInstance(type);

        }

        private static IlinkTextScripts loadScriptTXT(string assemblyPath, string className)
        {
            Assembly assembly = Assembly.LoadFrom(assemblyPath);
            //diagnostics
            var types = assembly.GetTypes();

            foreach (var item in types)
            {
                Console.WriteLine(item);
            }

            Type type = assembly.GetType("CopDrop." + className);

            if (type == null)
            {
                throw new InvalidOperationException("Class not found in the specified assembly.");
            }
            if (!typeof(CopDrop.IlinkTextScripts)!.IsAssignableFrom(type))
            {
                throw new InvalidOperationException("Class does not implement IlinkTextScripts interface.");
            }

            return (IlinkTextScripts)Activator.CreateInstance(type);

        }

        public void update()
        {
            for (int i = 0; i < btnObjects.Count; i++)
            {
                btnObjects[i].update();
                if (btnObjects[i].isButtonPressed())
                {
                    for (int k = 0; k < btnObjects[i].onPress.Length; k++)
                    {
                        commandLine.Instance.cli(btnObjects[i].onPress[k]);
                    }
                }
            }
            for (int i = 0; i < playerObjects.Count; i++)
            {
                playerObjects[i].update();
            }
            for (int i = 0; i < spriteObjects.Count; i++)
            {
                spriteObjects[i].update();
            }

            collision.update();
        }
        public void discrad()
        {
            if (spriteObjects != null)
            {
                for (int i = 0; i < spriteObjects.Count; i++)
                {
                    spriteObjects[i].discrad();
                }
            }
            if (tile != null)
            {
                for (int i = 0; i < tile.Count; i++)
                {
                    tile[i].discrad();
                }
            }
            if (playerObjects != null)
            {
                for (int i = 0; i < playerObjects.Count; i++)
                {
                    playerObjects[i].discrad();
                }
            }
            if (btnObjects != null)
            {
                for (int i = 0; i < btnObjects.Count; i++)
                {
                    btnObjects[i].discrad();
                    btnObjects[i].discardText();
                }
            }
            if (textObjects != null)
            {
                for (int i = 0; i < textObjects.Count; i++)
                {
                    textObjects[i].dealocate();
                }
            }

            string folderPath = @"scripts/dlls/"; // Specify the path of the folder

            DeleteDllFiles(folderPath);
        }

        public void render()
        {
            JArray backgroundColor = (JArray)miscellaneous["backGroundColor"];

            SDL_SetRenderDrawColor(GlobalVariable.Instance.renderer, (byte)backgroundColor[0], (byte)backgroundColor[1], (byte)backgroundColor[2], (byte)backgroundColor[3]);

            collision.render();

            if (spriteObjects != null)
            {
                for (int i = 0; i < spriteObjects.Count; i++)
                {
                    spriteObjects[i].show();
                }
            }
            if (tile != null)
            {
                for (int i = 0; i < tile.Count; i++)
                {
                    tile[i].show();
                }
            }
            if (btnObjects != null)
            {
                for (int i = 0; i < btnObjects.Count; i++)
                {
                    btnObjects[i].show();
                    btnObjects[i].showText();
                }
            }
            if (playerObjects != null)
            {
                for (int i = 0; i < playerObjects.Count; i++)
                {
                    playerObjects[i].show();
                }
            }
            if (textObjects != null)
            {
                for (int i = 0; i < textObjects.Count; i++)
                {
                    textObjects[i].render();
                }
            }
        }

    }

    public class MapManager
    {
        private List<Map> savedMaps = new List<Map>();
        public Map loadedMap;
        bool isMapSaved;
        public void LoadMap(Map map)
        {
            // Load the new map

            if (savedMaps != null || savedMaps.Count != 0)
            {

                for (int i = 0; i < savedMaps.Count; i++)
                {
                    if (savedMaps[i] == map)
                    {
                        isMapSaved = true;
                        loadedMap = savedMaps[i];
                    }
                }
                if (isMapSaved == false)
                {
                    savedMaps.Add(map);
                    for (int i = 0; i < savedMaps.Count; i++)
                    {
                        if (savedMaps[i] == map)
                        {
                            loadedMap = savedMaps[i];
                            Console.WriteLine("Map loaded from savedMaps");
                        }
                    }
                }


            }
            else
            {
                savedMaps.Add(map);
                loadedMap = map;
            }
            //Console.WriteLine("Thre are: " + savedMaps.Count + " saved");


        }

        public void Update()
        {
            // Update logic for the current map
            loadedMap.update();
        }

        public void Discrad()
        {
            for (int i = 0; i < savedMaps.Count; i++)
            {
                savedMaps[i].discrad();
            }
        }

        public void Render()
        {
            // Render the current map
            loadedMap.render();
        }
        public static MapManager Instance
        {
            get
            {
                return LazyInstance.Value;
            }
        }

        private static readonly Lazy<MapManager> LazyInstance = new Lazy<MapManager>(() => new MapManager());

    }

}
