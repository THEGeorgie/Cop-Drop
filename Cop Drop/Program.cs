// A runtime library used for pixel accses
using SDL2;
using static SDL2.SDL;

//Gpu rendering libraries
using OpenTK;
using OpenTK.Graphics.GL;
using OpenTK.Graphics.OpenGL;

// for displaying dynamic texts
using static SDL2.SDL_ttf;
using System.Drawing;
using System.Runtime.CompilerServices;


namespace CopDrop
{

    class Program
    {

        public static int Main()
        {

            int WINDOW_WIDTH = 1024;
            int WINDOW_HEIGHT = 600;

            SDL_Init(SDL_INIT_VIDEO);
            TTF_Init();

            var renderer = IntPtr.Zero;
            var window = IntPtr.Zero;


            SDL_CreateWindowAndRenderer(WINDOW_WIDTH, WINDOW_HEIGHT, SDL_WindowFlags.SDL_WINDOW_SHOWN, out window, out renderer);

            int mouseX = 0;
            int mouseY = 0;

            
            SDL_Point pt;
            pt.y = 0;
            pt.x = 0;

            Texture storeBtnTexture = new Texture(renderer, SDL_image.IMG_Load("stockXIcon.png"), 48, 48, 0, pt);
            Button storeBtn = new Button(storeBtnTexture, 50, 50);
            Map map = new Map(renderer);
            map.mapBuilder(1024, 600);
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
                            if (storeBtn.isButtonPressed(mouseX, mouseY))
                            {
                            }
                            break;
                    }
                }
                //For debugging 
                Console.WriteLine("x is" + mouseX + " y is" + mouseY);



                // Present/shows the the texture and deletes them

                
                SDL_Delay(10);
                map.init();
                storeBtn.texture.discrad();



            }

            //code cleanup

            SDL_DestroyRenderer(renderer);
            SDL_DestroyWindow(window);

            //Quits the sdl runtime library
            SDL_Quit();

            return 0;
        }
    }

    class Button
    {
        public Texture texture;
        public Button(Texture tx,int x, int y)
        {
            // baisc structs for the button 

            // this need to be tweaked for to work with the texture class (this will give muche more flexebilty) 

            texture = tx;

            texture.transform.x = x;
            texture.transform.y = y;
        }

        public bool isButtonPressed(int mouseX, int mouseY)
        {
            int[] buttonAreaPositionX = new int[texture.transform.w];
            int[] buttonAreaPositionY = new int[texture.transform.h];

            // Sets values to array by taking buttons x cord and ads +1 for every pixle of its width same for y and height.
            // This will then calculate the surface of the button plus the cordinate of every pixle 
            for (int i = 0; i < texture.transform.w; i++)
            {
                buttonAreaPositionX[i] = texture.transform.x + i;
            }
            for (int i = 0; i < texture.transform.h; i++)
            {
                buttonAreaPositionY[i] = texture.transform.y + i;
            }
            for (int i = 0; i < texture.transform.w; i++)
            {
                for (int k = 0; k < texture.transform.h; k++)
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
    }
    class Texture
    {
        // All of the struct nedded for a texture

        // Used for transforming the whole texture
        public SDL_Rect transform;

        // Used for transforming the surface of the texture
        public SDL_Rect transformSurface;

        // The renderer for to be shwon on the screen
        private IntPtr renderer;

        // The texture variable stored as a integer pointer
        public IntPtr texture;

        // For the rottation of the texture
        public double rotation = 0.0;

        // The point of the texture most often the center cordinantes of the texture
        private SDL_Point point;

        // Initalising all of the structs/variable that are passed from the constructor
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

        // Code celanup

        public void discrad()
        {
            SDL_DestroyTexture(texture);
            SDL_DestroyRenderer(renderer);
        }
    }

    class Map
    {
        private Texture[,] Textures = new Texture[1,1];
        private IntPtr renderer;
        public Map(IntPtr renderer)
        {
            this.renderer = renderer;

        }

        public void mapBuilder( int WINDOW_WIDTH, int WINDOW_HEIGHT)
        {
            // Level building code
            SDL_Point pt;
            pt.x = 0;
            pt.y = 0;
            Textures = new Texture[7,4];

            var srf = SDL_image.IMG_Load("Modern tiles_Free/Interiors_free/48x48/Room_Builder_free_48x48.png");

            var destinationSurface = SDL.SDL_CreateRGBSurface(0, 143 * 3, 96, 32, 0, 0, 0, 0);

            // Wall Front
            SDL_Rect sourceRect;
            sourceRect.x = 0;
            sourceRect.y = 239;
            sourceRect.w = 143;
            sourceRect.h = 96;

            SDL_Rect destinationRect;
            destinationRect.x = 0;
            destinationRect.y = 0;
            destinationRect.w = 143;
            destinationRect.h = 96;
            // Copies the srf surface on the destination surface 

            SDL_BlitSurface(srf, ref sourceRect, destinationSurface, ref destinationRect);
            sourceRect.x = 0;
            sourceRect.y = 239;
            sourceRect.w = 143;
            sourceRect.h = 96;

            destinationRect.x = 143;
            destinationRect.y = 0;
            destinationRect.w = 143;
            destinationRect.h = 96;

            // Copies the srf surface on the destination surface 
            SDL_BlitSurface(srf, ref sourceRect, destinationSurface, ref destinationRect);

            sourceRect.x = 0;
            sourceRect.y = 239;
            sourceRect.w = 143;
            sourceRect.h = 96;

            destinationRect.x = 143 * 2;
            destinationRect.y = 0;
            destinationRect.w = 143;
            destinationRect.h = 96;

            SDL_BlitSurface(srf, ref sourceRect, destinationSurface, ref destinationRect);

            Textures[0,0] = new Texture(renderer, destinationSurface, 143 * 3, 96, 0, pt);

            Textures[0,0].transform.x = WINDOW_WIDTH / 2 - Textures[0,0].transform.w / 2;
            Textures[0,0].transform.y = WINDOW_HEIGHT / 2 - 100;


            // Creates an array texture named cornerBorder
            
            for (int i = 0; i < 2; i++)
            {

                Textures[1,i] = new Texture(renderer, SDL_image.IMG_Load("Modern tiles_Free/Interiors_free/48x48/Room_Builder_free_48x48.png"), 25, 18, 0, pt);
                Textures[1, i].transformSurface.x = 555;
                Textures[1, i].transformSurface.y = 48;


                if (i != 0)
                {
                    // Second texture is to the right of the last wallfront texture class array
                    Textures[1, i].transform.x = Textures[0, 0].transform.x + Textures[0, 0].transform.w;
                    Textures[1, i].transform.y = Textures[0, 0].transform.y;

                }
                else
                {
                    // First texture is right next to the first wallFront array texture
                    Textures[1, i].transform.x = Textures[0, 0].transform.x - Textures[1, i].transform.x - 20;
                    Textures[1, i].transform.y = Textures[0, 0].transform.y;
                } 


            }
            destinationSurface = SDL.SDL_CreateRGBSurface(0, 21, 78 * 3, 32, 0, 0, 0, 0);

            // wallBorderLeft border
            sourceRect.x = 555;
            sourceRect.y = 66;
            sourceRect.w = 21;
            sourceRect.h = 78;

            destinationRect.x = 0;
            destinationRect.y = 0;
            destinationRect.w = 21;
            destinationRect.h = 78;
            // Copies the srf surface on the destination surface 

            SDL_BlitSurface(srf, ref sourceRect, destinationSurface, ref destinationRect);
            sourceRect.x = 555;
            sourceRect.y = 66;
            sourceRect.w = 21;
            sourceRect.h = 78;

            destinationRect.x = 0;
            destinationRect.y = 78;
            destinationRect.w = 21;
            destinationRect.h = 78;

            // Copies the srf surface on the destination surface 
            SDL_BlitSurface(srf, ref sourceRect, destinationSurface, ref destinationRect);

            sourceRect.x = 555;
            sourceRect.y = 66;
            sourceRect.w = 21;
            sourceRect.h = 78;

            destinationRect.x = 0;
            destinationRect.y = 78 * 2;
            destinationRect.w = 21;
            destinationRect.h = 78;

            SDL_BlitSurface(srf, ref sourceRect, destinationSurface, ref destinationRect);

            Textures[2, 0] = new Texture(renderer, destinationSurface, 21, 78 * 3, 0, pt);

            destinationSurface = SDL.SDL_CreateRGBSurface(0, 25, 78, 32, 0, 0, 0, 0);

            Textures[2, 0].transform.x = Textures[1, 0].transform.x;
            Textures[2, 0].transform.y = Textures[1, 0].transform.y + Textures[1, 0].transform.h;

            // wallBorderRight code

            sourceRect.x = 555;
            sourceRect.y = 66;
            sourceRect.w = 21;
            sourceRect.h = 78;

            destinationRect.x = 0;
            destinationRect.y = 0;
            destinationRect.w = 21;
            destinationRect.h = 78;
            // Copies the srf surface on the destination surface 

            SDL_BlitSurface(srf, ref sourceRect, destinationSurface, ref destinationRect);
            sourceRect.x = 555;
            sourceRect.y = 66;
            sourceRect.w = 21;
            sourceRect.h = 78;

            destinationRect.x = 0;
            destinationRect.y = 78;
            destinationRect.w = 21;
            destinationRect.h = 78;

            // Copies the srf surface on the destination surface 
            SDL_BlitSurface(srf, ref sourceRect, destinationSurface, ref destinationRect);

            sourceRect.x = 555;
            sourceRect.y = 66;
            sourceRect.w = 21;
            sourceRect.h = 78;

            destinationRect.x = 0;
            destinationRect.y = 78 * 2;
            destinationRect.w = 21;
            destinationRect.h = 78;

            SDL_BlitSurface(srf, ref sourceRect, destinationSurface, ref destinationRect);
            Textures[3, 0] = new Texture(renderer, destinationSurface, 21, 78 * 3, 0, pt);
            Textures[3, 0].transform.x = Textures[1, 1].transform.x;
            Textures[3, 0].transform.y = Textures[1, 1].transform.y + Textures[1, 1].transform.h;

            // cornerBorderBottom code
            for (int i = 2; i < 4; i++)
            {
                Textures[4, i] = new Texture(renderer, SDL_image.IMG_Load("Modern tiles_Free/Interiors_free/48x48/Room_Builder_free_48x48.png"), 25, 18, 0, pt);
                Textures[4, i].transformSurface.x = 555;
                Textures[4, i].transformSurface.y = 48;


                if (i != 2)
                {
                    // Second texture is to the right of the last wallfront texture class array
                    Textures[4, i].transform.x = Textures[0, 0].transform.x + Textures[0, 0].transform.w;
                    Textures[4, i].transform.y = Textures[3, 0].transform.y + Textures[2, 0].transform.h;

                }
                else
                {
                    // First texture is right next to the first wallFront array texture
                    Textures[4 ,i].transform.x = Textures[0, 0].transform.x - Textures[4, i].transform.x - 20;
                    Textures[4, i].transform.y = Textures[3, 0].transform.y + Textures[2, 0].transform.h;
                }


            }
            destinationSurface = SDL.SDL_CreateRGBSurface(0, 144 * 3, 96, 32, 0, 0, 0, 0);

            //Floor code
            for (int i = 0; i < 3; i++)
            {
                sourceRect.x = 528;
                sourceRect.y = 240;
                sourceRect.w = 143;
                sourceRect.h = 96;

                destinationRect.x = 143 * i;
                destinationRect.y = 0;
                destinationRect.w = 143;
                destinationRect.h = 96;
                // Copies the srf surface on the destination surface 

                SDL_BlitSurface(srf, ref sourceRect, destinationSurface, ref destinationRect);
            }


            sourceRect.x = 528;
            sourceRect.y = 240;
            sourceRect.w = 143;
            sourceRect.h = 96;

            destinationRect.x = 143;
            destinationRect.y = 0;
            destinationRect.w = 143;
            destinationRect.h = 96;

            // Copies the srf surface on the destination surface
            SDL_BlitSurface(srf, ref sourceRect, destinationSurface, ref destinationRect);
            sourceRect.x = 528;
            sourceRect.y = 240;
            sourceRect.w = 143;
            sourceRect.h = 96;

            destinationRect.x = 143 * 2;
            destinationRect.y = 0;
            destinationRect.w = 143;
            destinationRect.h = 96;

            // Copies the srf surface on the destination surface
            SDL_BlitSurface(srf, ref sourceRect, destinationSurface, ref destinationRect);

            // new destination surface for the bottom part of the floor
            var destinationSurface2 = SDL.SDL_CreateRGBSurface(0, 143 * 3, 96 * 2, 32, 0, 0, 0, 0);

            sourceRect.x = 0;
            sourceRect.y = 0;
            sourceRect.w = 143 * 3;
            sourceRect.h = 96;

            destinationRect.x = 0;
            destinationRect.y = 0;
            destinationRect.w = 143;
            destinationRect.h = 96;
            SDL_BlitSurface(destinationSurface, ref sourceRect, destinationSurface2, ref destinationRect);
            sourceRect.x = 0;
            sourceRect.y = 0;
            sourceRect.w = 143 * 3;
            sourceRect.h = 96;

            destinationRect.x = 0;
            destinationRect.y = 96;
            destinationRect.w = 143 * 2;
            destinationRect.h = 96;

            SDL_BlitSurface(destinationSurface, ref sourceRect, destinationSurface2, ref destinationRect);


            Textures[5, 0] = new Texture(renderer, destinationSurface2, 143 * 3, 85 * 2, 0, pt);
            Textures[5, 0].transform.x = Textures[0, 0].transform.x;
            Textures[5, 0].transform.y = Textures[0, 0].transform.y + Textures[0, 0].transform.h;


            SDL_FreeSurface(destinationSurface);

            // Wall borderBottom
            destinationSurface = SDL.SDL_CreateRGBSurface(0, 21, 78 * 5, 32, 0, 0, 0, 0);
            for (int i = 0; i < 5; i++)
            {
                sourceRect.x = 555;
                sourceRect.y = 66;
                sourceRect.w = 21;
                sourceRect.h = 78;

                destinationRect.x = 0;
                destinationRect.y = 78 * i;
                destinationRect.w = 21;
                destinationRect.h = 78;

                // Copies the srf surface on the destination surface 
                SDL_BlitSurface(srf, ref sourceRect, destinationSurface, ref destinationRect);
            }
            SDL_Point ptCenter;


            ptCenter.x = 21 / 2;
            ptCenter.y = (78 * 3) / 2;
            Textures[6, 0] = new Texture(renderer, destinationSurface, 19, 78 * 5 + 40, 90, ptCenter);
            Textures[6, 0].transform.x = Textures[5, 0].transform.y + Textures[5, 0].transform.h + 135;// + cornerBorderBottom[0].transform.h; 
            Textures[6, 0].transform.y = Textures[5, 0].transform.x + 47;

            SDL_FreeSurface(srf);
        }

        public void init()
        {
            for (int i = 0; i < Textures.Length; i++)
            {
                for (int k = 0; k < 4; k++)
                {
                    Textures[i, k].show();
                    Textures[i, k].show();
                    Textures[i, k].show();
                    Textures[i, k].show();
                    Textures[i, k].show();
                    Textures[i, k].show();
                    Textures[i, k].show();
                }
                
            }
            


            SDL_RenderPresent(renderer);

            for (int i = 0; i < Textures.Length; i++)
            {
                for (int k = 0; k < Textures.Length; k++)
                {
                    Textures[i, k].discrad();
                    Textures[i, k].discrad();
                    Textures[i, k].discrad();
                    Textures[i, k].discrad();
                    Textures[i, k].discrad();
                    Textures[i, k].discrad();
                    Textures[i, k].discrad();
                }

            }
        }
    }

    class Game
    {
        

        public Game()
        {
            
        }

        enum levels
        {
            ROOM,
            STORE
        }

        struct Textures
        {

        }

        void levelRoom()
        {
            SDL_Point pt;
            pt.y = 0;
            pt.x = 0;
            
        }

        void updateRender()
        {
            
        }

        void deallocate()
        {

        }

        void inputListener()
        {

        }
    }

}

