    // A runtime library used for pixel accses
    global using SDL2;
    global using static SDL2.SDL;
    // for displaying dynamic texts
    global using static SDL2.SDL_ttf;
    //system claases
    global using System;
namespace CopDrop
{

    class Game
    {
        int WINDOW_WIDTH;
        int WINDOW_HEIGHT;
        IntPtr window;

        public Game(int WINDOW_WIDTH, int WINDOW_HEIGHT)
        {
            SDL_Init(SDL_INIT_VIDEO);
            TTF_Init();

            this.WINDOW_HEIGHT = WINDOW_HEIGHT;
            this.WINDOW_WIDTH = WINDOW_WIDTH;
            var surface = SDL_image.IMG_Load("maps/main_level/assets/stockXIcon.png");

            SDL_CreateWindowAndRenderer(WINDOW_WIDTH, WINDOW_HEIGHT, SDL_WindowFlags.SDL_WINDOW_SHOWN, out window, out GlobalVariable.Instance.renderer);
            SDL.SDL_RenderSetLogicalSize(GlobalVariable.Instance.renderer, WINDOW_WIDTH, WINDOW_HEIGHT);
            SDL_SetWindowTitle(window, "Cop Drop");
            SDL_SetWindowIcon(window, surface);

            commandLine.Instance.cli("load_map maps/main_menu_level/main_menu_map.json");

        }

        public void render()
        {
            SDL_RenderClear(GlobalVariable.Instance.renderer);
            MapManager.Instance.Render();
            SDL_RenderPresent(GlobalVariable.Instance.renderer);
        }

        public void update()
        {
            MapManager.Instance.Update();
        }

        public void deallocate()
        {
            MapManager.Instance.Discrad();
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
            while (SDL_PollEvent(out GlobalVariable.Instance.ev) != 0)
            {
                switch (GlobalVariable.Instance.ev.type)
                {
                    case SDL_EventType.SDL_QUIT:
                        return true;
                        break;
                    case SDL_EventType.SDL_MOUSEBUTTONDOWN:
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
