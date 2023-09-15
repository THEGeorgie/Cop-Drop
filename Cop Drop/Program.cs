using static SDL2.SDL;
using static SDL2.SDL_ttf;

namespace SneakerSim
{
    class Program
    {
        public static int Main()
        {
            SDL_Init(SDL_INIT_VIDEO);
            TTF_Init();

            int WINDOW_WIDTH = 1024;
            int WINDOW_HEIGHT = 600;

            var renderer = IntPtr.Zero;
            var window = IntPtr.Zero;

            SDL_CreateWindowAndRenderer(WINDOW_WIDTH, WINDOW_HEIGHT, SDL_WindowFlags.SDL_WINDOW_SHOWN, out window, out renderer);
            SDL_Color color = new SDL_Color() { r = 255, g = 255, b = 255, a = 255 };
            var font = TTF_OpenFont("font.ttf", 20);
            var surface = TTF_RenderText_Solid(font, "BTN", color);
            Button btn = new Button(100, 50, 300, 300, surface, renderer);

            SDL_Event ev;
            bool loop = true;
            while (loop)
            {
                while (SDL_PollEvent(out ev) == 1)
                {
                    switch (ev.type)
                    {
                        case SDL_EventType.SDL_QUIT:
                            loop = false;
                            break;
                    }

                }

                btn.init();
                SDL_SetRenderDrawColor(renderer, 83, 104, 120, 255);
                SDL_RenderClear(renderer);
                SDL_RenderPresent(renderer);

            }

            SDL_DestroyRenderer(renderer);
            SDL_DestroyWindow(window);
            SDL_Quit();

            return 0;
        }
    }

    class Button
    {
        private SDL_Rect transform;
        private IntPtr texture;
        private IntPtr renderer;
        public Button(int width, int height, int x, int y, IntPtr surface, IntPtr renderer)
        {
            transform.w = width;
            transform.h = height;
            transform.x = x;
            transform.h = y;

            this.renderer = renderer;
            texture = SDL_CreateTextureFromSurface(renderer, surface);

        }

        public void init()
        {
            SDL_RenderCopy(renderer, texture, IntPtr.Zero, ref transform);
        }
        public bool isButtonPressed(int mouseX, int mouseY)
        {
            int[] buttonAreaPositionX = new int[transform.w];
            int[] buttonAreaPositionY = new int[transform.y];

            //Sets values to array by taking buttons x cord ands +1 for every pixle of its width same for y and height
            for (int i = 0; i < transform.w; i++)
            {
                buttonAreaPositionX[i] = transform.x + i;
            }
            for (int i = 0; i < transform.h; i++)
            {
                buttonAreaPositionY[i] = transform.y + i;
            }
            for (int i = 0; i < transform.w; i++)
            {
                for (int k = 0; k < transform.h; k++)
                {
                    //checks if the mouse cords are in the area of the button 
                    if (buttonAreaPositionX[i] == mouseX && buttonAreaPositionY[k] == mouseY)
                    {
                        return true;
                    }

                }
            }
            return false;
        }

        ~Button()
        {
            SDL_DestroyTexture(texture);
        }
    }


}

