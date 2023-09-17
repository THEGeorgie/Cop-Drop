using SDL2;
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

            int mouseX = 0;
            int mouseY = 0;

            var renderer = IntPtr.Zero;
            var window = IntPtr.Zero;

            SDL_CreateWindowAndRenderer(WINDOW_WIDTH, WINDOW_HEIGHT, SDL_WindowFlags.SDL_WINDOW_SHOWN, out window, out renderer);
            SDL_Point pt;
            pt.y = WINDOW_HEIGHT / 2;
            pt.x = WINDOW_WIDTH / 2;


            Texture[] floorTexture = new Texture[9];
            for (int i = 0; i < floorTexture.Length; i++)
            {
                floorTexture[i] = new Texture(renderer, SDL_image.IMG_Load("wood_128x64.png"), 128, 64, 0, pt);
                if (i == 0)
                {
                    floorTexture[i].transform.x = WINDOW_WIDTH / 2 - floorTexture[i].transform.w;
                    floorTexture[i].transform.y = WINDOW_HEIGHT / 2 - floorTexture[i].transform.h;
                }
                else
                {
                    floorTexture[i].transform.x = (WINDOW_WIDTH / 2 - floorTexture[i].transform.w) + 64 * i;
                    floorTexture[i].transform.y = (WINDOW_HEIGHT / 2 - floorTexture[i].transform.h) + 32 * i;
                }

                if (i == 3)
                {
                    floorTexture[i].transform.x = 448;
                    floorTexture[i].transform.y = 331;

                }
                if (i == 4)
                {
                    floorTexture[i].transform.x = 384;
                    floorTexture[i].transform.y = 299;

                }
                if (i == 5)
                {
                    floorTexture[i].transform.x = 320;
                    floorTexture[i].transform.y = 267;

                }


                if (i == 6)
                {
                    floorTexture[i].transform.x = 574;
                    floorTexture[i].transform.y = 269;

                }
                if (i == 7)
                {
                    floorTexture[i].transform.x = 510;
                    floorTexture[i].transform.y = 237;

                }
                if (i == 8)
                {
                    floorTexture[i].transform.x = 446;
                    floorTexture[i].transform.y = 205;

                }



            }




            SDL_Event ev;
            bool loop = true;
            while (loop)
            {
                SDL_GetMouseState(out mouseX, out mouseY);

                while (SDL_PollEvent(out ev) == 1)
                {
                    switch (ev.type)
                    {
                        case SDL_EventType.SDL_QUIT:
                            loop = false;
                            break;
                        case SDL_EventType.SDL_MOUSEBUTTONDOWN:


                            break;
                    }

                }
                Console.WriteLine("x is" + mouseX + " y is" + mouseY);


                SDL_SetRenderDrawColor(renderer, 83, 104, 120, 255);
                SDL_RenderClear(renderer);

                for (int i = 0; i < floorTexture.Length; i++)
                {
                    floorTexture[i].show();
                }


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
        public SDL_Rect transform;
        private IntPtr texture;
        private IntPtr renderer;
        public SDL_Rect transforSurface;

        public Button(int width, int height, int x, int y, IntPtr surface, IntPtr renderer)
        {

            transforSurface.x = 0;
            transforSurface.y = 0;
            transforSurface.w = width;
            transforSurface.h = height;

            transform.w = width;
            transform.h = height;
            transform.x = x;
            transform.y = y;

            this.renderer = renderer;
            texture = SDL_CreateTextureFromSurface(renderer, surface);
            SDL_FreeSurface(surface);

        }

        public void init()
        {
            SDL_SetRenderDrawColor(renderer, 0, 0, 0, 255);
            SDL_RenderDrawRect(renderer, ref transform);
            SDL_RenderCopy(renderer, texture, ref transforSurface, ref transform);
        }
        public bool isButtonPressed(int mouseX, int mouseY)
        {
            int[] buttonAreaPositionX = new int[transform.w];
            int[] buttonAreaPositionY = new int[transform.h];

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
    class Texture
    {
        public SDL_Rect transform;
        public SDL_Rect transformSurface;
        private IntPtr renderer;
        private IntPtr texture;
        private int rotation = 0;
        private SDL_Point point;

        public Texture(IntPtr renderer, IntPtr surface, int textureWidth, int textureHeight, int rotation, SDL_Point point)
        {
            texture = SDL_CreateTextureFromSurface(renderer, surface);
            this.renderer = renderer;
            SDL_FreeSurface(surface);
            this.rotation = rotation;
            this.point = point;

            transform.w = textureWidth;
            transform.h = textureHeight;
            transform.x = 0;
            transform.y = 0;

            transformSurface.w = transform.w;
            transformSurface.h = transform.h;
            transformSurface.x = 0;
            transformSurface.y = 0;
        }

        public void show()
        {
            SDL_RenderCopyEx(renderer, texture, ref transformSurface, ref transform, rotation, ref point, SDL_RendererFlip.SDL_FLIP_NONE);
        }
        ~Texture()
        {
            SDL_DestroyTexture(texture);
            SDL_DestroyRenderer(renderer);
        }
    }



}

