using SDL2;
using static SDL2.SDL;
using static SDL2.SDL_ttf;

namespace CopDrop
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
            pt.y = 96 / 2;
            pt.x = 143 / 2;


            Texture[] wallFront = new Texture[3];
            for (int i = 0; i < wallFront.Length; i++)
            {
                wallFront[i] = new Texture(renderer, SDL_image.IMG_Load("Modern tiles_Free/Interiors_free/48x48/Room_builder_free_48x48.png"), 143, 96, 0, pt);
                wallFront[i].transformSurface.x = 0;
                wallFront[i].transformSurface.y = 239;
                wallFront[i].transform.y = 150;

                if (i != 0)
                {

                    wallFront[i].transform.x = (WINDOW_WIDTH / 2 - 143 * 2 + 143 / 2) + (143 * i);
                }
                else
                {
                    wallFront[i].transform.x = WINDOW_WIDTH / 2 - 143 * 2 + 143 / 2;
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


                SDL_SetRenderDrawColor(renderer, 204, 255, 255, 255);
                SDL_RenderClear(renderer);

                for (int i = 0; i < wallFront.Length; i++)
                {
                    // wallFront[i].show();
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
        public IntPtr texture;
        private double rotation = 0.0;
        private SDL_Point point;

        public Texture(IntPtr renderer, IntPtr surface, int textureWidth, int textureHeight, int rotation, SDL_Point point)
        {
            texture = SDL_CreateTextureFromSurface(renderer, surface);
            this.renderer = renderer;
            SDL_FreeSurface(surface);
            this.rotation = (double)rotation;
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

