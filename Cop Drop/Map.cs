// json library
using Newtonsoft.Json.Linq;

namespace CopDrop
{
    public class Map
    {
        int MAP_WIDTH;
        int MAP_HEIGHT;
        List<Texture> texObjects = new List<Texture>();
        List<Button> btnObjects = new List<Button>();
        List<IntPtr> surfaces = new List<IntPtr>();
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
                    if ((int)type["type"] != 0)
                    {
                        foreach (var objects in type["objects"])
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
                            switch ((int)type["type"])
                            {
                                // 1 means its a texture object 
                                case 1:
                                    if ((char)objects["alignOn"] == 'x')
                                    {
                                        texObjects.Add(new Texture(destinationSurface, (int)objects["w"] * (int)objects["quantity"], (int)objects["h"], (int)objects["rotation"]));

                                    }
                                    else if ((char)objects["alignOn"] == 'y')
                                    {
                                        texObjects.Add(new Texture(destinationSurface, (int)objects["w"], (int)objects["h"] * (int)objects["quantity"], (int)objects["rotation"]));
                                    }
                                    texObjects[count].transform.x = (int)objects["x"];
                                    texObjects[count].transform.y = (int)objects["y"];
                                    break;
                                // 2 means its a button object 
                                case 2:
                                    JArray commandList = (JArray)objects["onPress"];
                                    string[] buffer = new string[commandList.Count];
                                    for (int i = 0; i < commandList.Count; i++)
                                    {
                                        buffer[i] = (string)commandList[i];
                                    }
                                    if ((char)objects["alignOn"] == 'x')
                                    {
                                        btnObjects.Add(new Button(destinationSurface, (int)objects["w"] * (int)objects["quantity"], (int)objects["h"], (int)objects["rotation"], (int)objects["x"], (int)objects["y"], buffer));
                                    }
                                    else if ((char)objects["alignOn"] == 'y')
                                    {
                                        btnObjects.Add(new Button(destinationSurface, (int)objects["w"], (int)objects["h"] * (int)objects["quantity"], (int)objects["rotation"], (int)objects["x"], (int)objects["y"], buffer));
                                    }
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
            for (int i = 0; i < texObjects.Count; i++)
            {
                texObjects[i].discrad();
            }
        }

        public void render()
        {
            JArray backgroundColor = (JArray)miselaneus["backGroundColor"];

            SDL_SetRenderDrawColor(GlobalVariable.Instance.renderer, (byte)backgroundColor[0], (byte)backgroundColor[1], (byte)backgroundColor[2], (byte)backgroundColor[3]);

            for (int i = 0; i < texObjects.Count; i++)
            {
                texObjects[i].show();
            }
            for (int i = 0; i < btnObjects.Count; i++)
            {
                btnObjects[i].show();
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
