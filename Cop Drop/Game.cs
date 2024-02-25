// A runtime library used for pixel accses
global using SDL2;
//system claases
global using System;
global using static SDL2.SDL;
// for displaying dynamic texts
global using static SDL2.SDL_ttf;

namespace CopDrop
{

    class Game
    {
        int WINDOW_WIDTH;
        int WINDOW_HEIGHT;
        IntPtr window;

        //Creates the window and the renderer window titl, icon and loads in a map
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

        //every specifed amount of time it clears the screeen and re renders the objects
        public void render()
        {
            SDL_RenderClear(GlobalVariable.Instance.renderer);
            MapManager.Instance.Render();
            SDL_RenderPresent(GlobalVariable.Instance.renderer);
        }

        //Updates any changes
        public void update()
        {
            MapManager.Instance.Update();
            GlobalVariable.Instance.keybaordEvent.inputListener();
        }


        //After closing the program it releases the memory and closes any open libraries in use
        public void deallocate()
        {
            MapManager.Instance.Discrad();
            SDL_DestroyRenderer(GlobalVariable.Instance.renderer);
            SDL_DestroyWindow(window);
            TTF_CloseFont(GlobalVariable.Instance.font);

            TTF_Quit();
            SDL_Quit();
        }

        // Listens to any input being made
        //! this method has to be overhalled and depricated


    }
}
