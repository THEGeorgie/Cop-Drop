// json library
using System.Globalization;
using Newtonsoft.Json.Linq;

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
        JObject miselaneus;

        public Map(string jsonPath)
        {
            if (File.Exists(jsonPath))
            {
                // Read the JSON file
                string jsonContent = File.ReadAllText(jsonPath);

                // Parse the JSON content as a dynamic object
                json = JArray.Parse(jsonContent);
                // objects used for in general (background color assets to use etc.)
                miselaneus = (JObject)json[0];

                mapBuilder();
            }
            else
            {
                Console.WriteLine("The JSON file does not exist.");
            }
        }

        void mapBuilder()
        {
            SDL_Rect sourceRect = new SDL_Rect();
            SDL_Rect destinationRect = new SDL_Rect();
            SDL_Color textColor = new SDL_Color();

            var srf = SDL_image.IMG_Load(miselaneus["assetLocation"].ToString());

            JArray locationSurfaces = (JArray)miselaneus["assetLocation"];
            JArray backgroundColor = (JArray)miselaneus["backGroundColor"];

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
                                Console.WriteLine("The text color from map are: "+ colorArray[0] + " " + textColor.g + " "+ textColor.b);

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
                                    JArray scriptList = (JArray)objects["scripts"];

                                    string[] bufferCommands = new string[commandList.Count];
                                    string[] bufferScripts = new string[scriptList.Count];

                                    for (int i = 0; i < commandList.Count; i++)
                                    {
                                        bufferCommands[i] = (string)commandList[i];
                                    }
                                    for (int i = 0; i < scriptList.Count; i++)
                                    {
                                        bufferScripts[i] = (string)scriptList[i];
                                    }


                                    if ((char)objects["alignOn"] == 'x')
                                    {
                                        btnObjects.Add(new Button(destinationSurface, (int)objects["w"] * (int)objects["quantity"], (int)objects["h"], (int)objects["rotation"], (int)objects["x"], (int)objects["y"], bufferCommands,bufferScripts));
                                    }
                                    else if ((char)objects["alignOn"] == 'y')
                                    {
                                        btnObjects.Add(new Button(destinationSurface, (int)objects["w"], (int)objects["h"] * (int)objects["quantity"], (int)objects["rotation"], (int)objects["x"], (int)objects["y"], bufferCommands,bufferScripts));
                                    }
                                    break;
                                case 2.3:
                                    JArray commandList1 = (JArray)objects["onPress"];
                                    JArray scriptList1 = (JArray)objects["scripts"];
                                    string[] bufferCommands1 = new string[commandList1.Count];
                                    string[] bufferScripts1 = new string[scriptList1.Count];

                                    for (int i = 0; i < commandList1.Count; i++)
                                    {
                                        bufferCommands1[i] = (string)commandList1[i];
                                    }
                                    for (int i = 0; i < scriptList1.Count; i++)
                                    {
                                        bufferScripts1[i] = (string)scriptList1[i];
                                    }

                                    Text txt = new Text((string)objects["text"], (int)objects["fontsize"], textColor);
                                    if ((char)objects["alignOn"] == 'x')
                                    {
                                        btnObjects.Add(new Button(destinationSurface, (int)objects["w"] * (int)objects["quantity"], (int)objects["h"], (int)objects["rotation"], txt, (int)objects["textX"], (int)objects["textY"], (int)objects["x"], (int)objects["y"], bufferCommands1, bufferScripts1));
                                    }
                                    else if ((char)objects["alignOn"] == 'y')
                                    {
                                        btnObjects.Add(new Button(destinationSurface, (int)objects["w"], (int)objects["h"] * (int)objects["quantity"], (int)objects["rotation"], txt, (int)objects["textX"], (int)objects["textY"], (int)objects["x"], (int)objects["y"], bufferCommands1, bufferScripts1));
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
            JArray backgroundColor = (JArray)miselaneus["backGroundColor"];

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
        public Map currentMap;

        public void LoadMap(Map map)
        {
            // Load the new map
            if (currentMap != null)
            {
                currentMap.discrad();
            }
            currentMap = map;
        }

        public void Update()
        {
            // Update logic for the current map
            currentMap.update();
        }

        public void Discrad()
        {
            currentMap.discrad();
        }

        public void Render()
        {
            // Render the current map
            currentMap.render();
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
