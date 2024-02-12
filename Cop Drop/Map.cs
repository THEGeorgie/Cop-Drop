// json library
using Newtonsoft.Json.Linq;
//assembley
using System.Reflection;

namespace CopDrop
{
    public class Map
    {
        int MAP_WIDTH;
        int MAP_HEIGHT;
        List<Texture> spriteObjects = new List<Texture>();
        List<Button> btnObjects = new List<Button>();
        List<IntPtr> surfaces = new List<IntPtr>();
        List<Text> textObjects = new List<Text>();
        JArray json;
        JObject miscellaneous;

        //Reads the provided json then builds the map
        public Map(string jsonPath)
        {
            if (File.Exists(jsonPath))
            {
                // Read the JSON file
                string jsonContent = File.ReadAllText(jsonPath);

                // Parse the JSON content as a dynamic object
                json = JArray.Parse(jsonContent);
                // objects used  in general (background color assets to use etc.)
                miscellaneous = (JObject)json[0];

                mapBuilder();
            }
            else
            {
                Console.WriteLine(jsonPath);
                Console.WriteLine("The JSON file does not exist.");
            }
        }

        void mapBuilder()
        {
            SDL_Rect sourceRect = new SDL_Rect();
            SDL_Rect destinationRect = new SDL_Rect();
            SDL_Color textColor = new SDL_Color();

            var srf = SDL_image.IMG_Load(miscellaneous["assetLocation"].ToString());

            JArray locationSurfaces = (JArray)miscellaneous["assetLocation"];
            JArray backgroundColor = (JArray)miscellaneous["backGroundColor"];

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
                            if ((int)type["type"] == 1 || (int)type["type"] == 2)
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
                                case 1:
                                    if ((char)objects["alignOn"] == 'x')
                                    {
                                        spriteObjects.Add(new Texture(destinationSurface, (int)objects["w"] * (int)objects["quantity"], (int)objects["h"], (int)objects["rotation"]));

                                    }
                                    else if ((char)objects["alignOn"] == 'y')
                                    {
                                        spriteObjects.Add(new Texture(destinationSurface, (int)objects["w"], (int)objects["h"] * (int)objects["quantity"], (int)objects["rotation"]));
                                    }
                                    spriteObjects[count].transform.x = (int)objects["x"];
                                    spriteObjects[count].transform.y = (int)objects["y"];
                                    break;
                                // 2 means its a button object 
                                case 2:
                                    //There is a convertion from 
                                    JArray commandList = (JArray)objects["onPress"];

                                    string[] bufferCommands = new string[commandList.Count];

                                    for (int i = 0; i < commandList.Count; i++)
                                    {
                                        bufferCommands[i] = (string)commandList[i];
                                    }



                                    if ((char)objects["alignOn"] == 'x')
                                    {
                                        if ((string)objects["script"] != null)
                                        {
                                            ScriptCompiler scriptCompiler = new ScriptCompiler((string)objects["script"]);
                                            btnObjects.Add(new Button(destinationSurface, (int)objects["w"] * (int)objects["quantity"], (int)objects["h"], (int)objects["rotation"], (int)objects["x"], (int)objects["y"], bufferCommands, loadScript(scriptCompiler.DllPath, scriptCompiler.ScriptClassName)));
                                        }
                                        else
                                        {
                                            btnObjects.Add(new Button(destinationSurface, (int)objects["w"] * (int)objects["quantity"], (int)objects["h"], (int)objects["rotation"], (int)objects["x"], (int)objects["y"], bufferCommands, null));
                                        }
                                    }
                                    else if ((char)objects["alignOn"] == 'y')
                                    {

                                        if ((string)objects["script"] != null)
                                        {
                                            ScriptCompiler scriptCompiler = new ScriptCompiler((string)objects["script"]);

                                            btnObjects.Add(new Button(destinationSurface, (int)objects["w"], (int)objects["h"] * (int)objects["quantity"], (int)objects["rotation"], (int)objects["x"], (int)objects["y"], bufferCommands, loadScript(scriptCompiler.DllPath, scriptCompiler.ScriptClassName)));

                                        }
                                        else
                                        {
                                            btnObjects.Add(new Button(destinationSurface, (int)objects["w"], (int)objects["h"] * (int)objects["quantity"], (int)objects["rotation"], (int)objects["x"], (int)objects["y"], bufferCommands, null));
                                        }
                                    }
                                    break;
                                case 2.3:
                                    JArray commandList1 = (JArray)objects["onPress"];
                                    string[] bufferCommands1 = new string[commandList1.Count];

                                    for (int i = 0; i < commandList1.Count; i++)
                                    {
                                        bufferCommands1[i] = (string)commandList1[i];
                                    }


                                    Text txt = new Text((string)objects["text"], (int)objects["fontsize"], textColor);
                                    if ((char)objects["alignOn"] == 'x')
                                    {
                                        if ((string)objects["script"] != null)
                                        {
                                            ScriptCompiler scriptCompiler = new ScriptCompiler((string)objects["script"]);
                                            btnObjects.Add(new Button(destinationSurface, (int)objects["w"] * (int)objects["quantity"], (int)objects["h"], (int)objects["rotation"], txt, (int)objects["textX"], (int)objects["textY"], (int)objects["x"], (int)objects["y"], bufferCommands1, loadScript(scriptCompiler.DllPath, scriptCompiler.ScriptClassName)));
                                        }
                                        else
                                        {
                                            btnObjects.Add(new Button(destinationSurface, (int)objects["w"] * (int)objects["quantity"], (int)objects["h"], (int)objects["rotation"], txt, (int)objects["textX"], (int)objects["textY"], (int)objects["x"], (int)objects["y"], bufferCommands1, null));
                                        }
                                    }
                                    else if ((char)objects["alignOn"] == 'y')
                                    {

                                        if ((string)objects["script"] != null)
                                        {
                                            ScriptCompiler scriptCompiler = new ScriptCompiler((string)objects["script"]);
                                            btnObjects.Add(new Button(destinationSurface, (int)objects["w"], (int)objects["h"] * (int)objects["quantity"], (int)objects["rotation"], txt, (int)objects["textX"], (int)objects["textY"], (int)objects["x"], (int)objects["y"], bufferCommands1, loadScript(scriptCompiler.DllPath, scriptCompiler.ScriptClassName)));
                                        }
                                        else
                                        {
                                            btnObjects.Add(new Button(destinationSurface, (int)objects["w"], (int)objects["h"] * (int)objects["quantity"], (int)objects["rotation"], txt, (int)objects["textX"], (int)objects["textY"], (int)objects["x"], (int)objects["y"], bufferCommands1, null));
                                        }
                                    }
                                    break;
                                case 3:
                                    textObjects.Add(new Text((string)objects["text"], (int)objects["fontsize"], textColor));
                                    Console.WriteLine(count);
                                    textObjects[count].x = (int)objects["x"];
                                    textObjects[count].y = (int)objects["y"];
                                    textObjects[count].update();
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
                throw new InvalidOperationException("Class does not implement IlinkButtonScripts interface.");
            }

            return (IlinkPlayerScripts)Activator.CreateInstance(type);

        }

        private static IlinkTextureScripts loadScriptTEX(string assemblyPath, string className)
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
            if (!typeof(CopDrop.IlinkTextureScripts)!.IsAssignableFrom(type))
            {
                throw new InvalidOperationException("Class does not implement IlinkButtonScripts interface.");
            }

            return (IlinkTextureScripts)Activator.CreateInstance(type);

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
        }

        public void render()
        {
            JArray backgroundColor = (JArray)miscellaneous["backGroundColor"];

            SDL_SetRenderDrawColor(GlobalVariable.Instance.renderer, (byte)backgroundColor[0], (byte)backgroundColor[1], (byte)backgroundColor[2], (byte)backgroundColor[3]);

            if (spriteObjects != null)
            {
                for (int i = 0; i < spriteObjects.Count; i++)
                {
                    spriteObjects[i].show();
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
        Map loadedMap;
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
