    // A runtime library used for pixel accses
    global using SDL2;
    global using static SDL2.SDL;
    // for displaying dynamic texts
    global using static SDL2.SDL_ttf;
namespace CopDrop
{

    class Game
    {
        int WINDOW_WIDTH;
        int WINDOW_HEIGHT;
        /*
        Map mapStore;
        Map mapRoom;
        */
        //Button storeBtn;
        //Button exitStoreBtn;
        IntPtr window;
        Map mapy;

        SDL_Event ev;

        public Game(int WINDOW_WIDTH, int WINDOW_HEIGHT)
        {
            SDL_Init(SDL_INIT_VIDEO);
            TTF_Init();

            this.WINDOW_HEIGHT = WINDOW_HEIGHT;
            this.WINDOW_WIDTH = WINDOW_WIDTH;
            var surface = SDL_image.IMG_Load("storeAssets/stockXIcon.png");

            SDL_CreateWindowAndRenderer(WINDOW_WIDTH, WINDOW_HEIGHT, SDL_WindowFlags.SDL_WINDOW_SHOWN, out window, out GlobalVariable.Instance.renderer);
            SDL_SetWindowTitle(window, "Cop Drop");
            SDL_SetWindowIcon(window, surface);

            //mapy = new Map("maps/main_level/main_map.json");

            //Texture temp = new Texture(surface, 48, 48, 0);
            //storeBtn = new Button(temp, 50, 50);
            /*
            surface = SDL_image.IMG_Load("storeAssets/exitSign.png");
            temp = new Texture(surface, 50, 50, 0);
            exitStoreBtn = new Button(temp, 50, 50);


            SDL_FreeSurface(surface);
            temp.discrad();
            */
           // MapManager.Instance.LoadMap(mapy);

            commandLine.Instance.cli("load_map maps/main_level/main_map.json");

        }

        public void render()
        {
            SDL_RenderClear(GlobalVariable.Instance.renderer);
            MapManager.Instance.Render();
            /*
            if (map == 'r')
            {
                storeBtn.texture.show();
            }
            else if (map == 's')
            {
                if (GlobalVariable.Instance.openSnekaerDetails == 0)
                {
                    exitStoreBtn.texture.show();
                }
            }
            */
            SDL_RenderPresent(GlobalVariable.Instance.renderer);
        }

        public void update()
        {
            MapManager.Instance.Update();
        }

        public void deallocate()
        {
            //code cleanup
            /*
            mapRoom.release();
            mapStore.release();
            */
            MapManager.Instance.Discrad();
            /*
            storeBtn.texture.discrad();
            exitStoreBtn.texture.discrad();
            */
            SDL_DestroyRenderer(GlobalVariable.Instance.renderer);
            SDL_DestroyWindow(window);
            TTF_CloseFont(GlobalVariable.Instance.font);


            //Quits the sdl runtime library && font runtime library
            TTF_Quit();
            SDL_Quit();
        }

        public bool inputListener()
        {
            SDL_GetMouseState(out int buffer1, out int buffer2);
            GlobalVariable.Instance.mouseX = buffer1;
            GlobalVariable.Instance.mouseY = buffer2;
            while (SDL_PollEvent(out ev) != 0)
            {
                switch (ev.type)
                {
                    case SDL_EventType.SDL_QUIT:
                        return true;
                        break;
                    case SDL_EventType.SDL_MOUSEBUTTONDOWN:
                        /*
                         if (storeBtn.isButtonPressed() && map == 'r')
                         {
                             mapManager.LoadMap(mapStore);
                             map = 's';
                         }
                         else if (exitStoreBtn.isButtonPressed() && map == 's' && GlobalVariable.Instance.openSnekaerDetails == 0)
                         {
                             mapManager.LoadMap(mapRoom);
                             map = 'r';
                         }
                         */
                        GlobalVariable.Instance.mouseButtonClick = 1;
                        break;
                    case SDL_EventType.SDL_MOUSEBUTTONUP:
                        GlobalVariable.Instance.mouseButtonClick = 0;
                        break;
                    case SDL_EventType.SDL_KEYDOWN:
                        GlobalVariable.Instance.GetKey(SDL_Keycode.SDLK_ESCAPE);
                        break;
                        break;
                }
            }

            return false;
        }
    }

}
