// A runtime library used for pixel accses
using SDL2;
using static SDL2.SDL;

// for displaying dynamic texts
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
            pt.y = 0;
            pt.x = 0;


            // Level building code
            /*
            // Creates an array texture named wallFront
            Texture[] wallFront = new Texture[3];
            for (int i = 0; i < wallFront.Length; i++)
            {
                // Every texture class constructor takes the renderer, surface, width and height, rotation and the point of the texture which in this case is the center point (x,y)
                // and most other cases is.
                wallFront[i] = new Texture(renderer, SDL_image.IMG_Load("Modern tiles_Free/Interiors_free/48x48/Room_Builder_free_48x48.png"), 143, 96, 0, pt);
                // The cordinants for the specific object from the surface.
                wallFront[i].transformSurface.x = 0;
                wallFront[i].transformSurface.y = 239;

                // The y cord on the window.
                wallFront[i].transform.y = 150;

                if (i != 0)
                {
                    // The next texture class is on the right of the first texture class (index 0) 
                    wallFront[i].transform.x = (WINDOW_WIDTH / 2 - 143 * (wallFront.Length - 1) + 143 / 2) + (143 * i);
                }
                else
                {
                    // First texture class of the array is at that specific area on the window. The math is that evey texture class is displayed the middle of the screen 
                    wallFront[i].transform.x = WINDOW_WIDTH / 2 - 143 * (wallFront.Length - 1) + 143 / (wallFront.Length - 1);
                }


            }

            // Creates an array texture named cornerBorder
            Texture[] cornerBorder = new Texture[2];
            for (int i = 0; i < cornerBorder.Length; i++)
            {
                cornerBorder[i] = new Texture(renderer, SDL_image.IMG_Load("Modern tiles_Free/Interiors_free/48x48/Room_Builder_free_48x48.png"), 25, 18, 0, pt);
                cornerBorder[i].transformSurface.x = 555;
                cornerBorder[i].transformSurface.y = 48;


                if (i != 0)
                {
                    // Second texture is to the right of the last wallfront texture class array
                    cornerBorder[i].transform.x = wallFront[wallFront.Length - 1].transform.x + wallFront[wallFront.Length - 1].transform.w;
                    cornerBorder[i].transform.y = wallFront[0].transform.y + 1;

                }
                else
                {
                    // First texture is right next to the first wallFront array texture
                    cornerBorder[i].transform.x = wallFront[0].transform.x - 20;
                    cornerBorder[i].transform.y = wallFront[0].transform.y + 1;
                }


            }



            // Creates an array texture named wallBorder
            Texture[] wallBorder = new Texture[6];
            for (int i = 0; i < wallBorder.Length; i++)
            {
                wallBorder[i] = new Texture(renderer, SDL_image.IMG_Load("Modern tiles_Free/Interiors_free/48x48/Room_Builder_free_48x48.png"), 25, 78, 0, pt);
                wallBorder[i].transformSurface.x = 555;
                wallBorder[i].transformSurface.y = 66;

                // From 0 to 2 index the texture class is to the left
                if (i == 0 || i == 1 || i == 2)
                {

                    wallBorder[i].transform.x = wallFront[0].transform.x - 20;

                    if (i != 0)
                    {
                        // From 1 to 2 index the textures are at the bottom of the first array texture class
                        wallBorder[i].transform.y = cornerBorder[0].transform.y + 18 + 78 * i;
                    }
                    else
                    {
                        // First index of the array class is ont the bottom of the first cornerBorder texture array class
                        wallBorder[i].transform.y = cornerBorder[0].transform.y + 18;

                    }
                }

                // From 3 to 5 index the texture class is to the right
                else
                {
                    // The position is on the right of the last wall Front textre array class 
                    wallBorder[i].transform.x = wallFront[wallFront.Length - 1].transform.x + wallFront[wallFront.Length - 1].transform.w;

                    if (i != 3)
                    {
                        if (i == 4)
                        {
                            // Index 4 the textures is at the bottom of the third array texture class
                            wallBorder[i].transform.y = cornerBorder[cornerBorder.Length - 1].transform.y + 18 + 78;
                        }
                        if (i == 5)
                        {
                            // From 1 to 2 index the textures are at the bottom of the first array texture class
                            wallBorder[i].transform.y = cornerBorder[cornerBorder.Length - 1].transform.y + 18 + 78 * 2;

                        }
                    }
                    else
                    {
                        // Third index of the array class is ont the bottom of the second cornerBorder texture array class
                        wallBorder[i].transform.y = cornerBorder[cornerBorder.Length - 1].transform.y + 18;

                    }
                }
            }
            Texture[] cornerBorderBottom = new Texture[2];

            for (int i = 0; i < cornerBorderBottom.Length; i++)
            {
                cornerBorderBottom[i] = new Texture(renderer, SDL_image.IMG_Load("Modern tiles_Free/Interiors_free/48x48/Room_Builder_free_48x48.png"), 25, 18, 0, pt);
                cornerBorderBottom[i].transformSurface.x = 555;
                cornerBorderBottom[i].transformSurface.y = 48;


                if (i != 0)
                {
                    // Second texture is to the right of the last wallfront texture class array
                    cornerBorderBottom[i].transform.x = wallFront[wallFront.Length - 1].transform.x + wallFront[wallFront.Length - 1].transform.w;
                    cornerBorderBottom[i].transform.y = (wallFront[0].transform.y + 1 + wallBorder[wallBorder.Length - 1].transform.y) - 73;

                }
                else
                {
                    // First texture is right next to the first wallFront array texture
                    cornerBorderBottom[i].transform.x = wallFront[0].transform.x - 20;
                    cornerBorderBottom[i].transform.y = (wallFront[0].transform.y + 1 + wallBorder[wallBorder.Length - 1].transform.y) - 73;
                }
            }

            // Creates an array texture named floor
            Texture[] floor = new Texture[6];

            for (int i = 0; i < floor.Length; i++)
            {


                if (i == 3 || i == 4 || i == 5)
                {
                    // From index 3 to 5 the textures are under index 0 to 2 textures
                    floor[i] = new Texture(renderer, SDL_image.IMG_Load("Modern tiles_Free/Interiors_free/48x48/Room_Builder_free_48x48.png"), 143, 66, 0, pt);
                    floor[i].transform.y = wallFront[0].transform.y + 96 * 2;

                }

                else
                {
                    // From index 0 to 2 the textures are under wallFront textures
                    floor[i] = new Texture(renderer, SDL_image.IMG_Load("Modern tiles_Free/Interiors_free/48x48/Room_Builder_free_48x48.png"), 143, 96, 0, pt);
                    floor[i].transform.y = wallFront[0].transform.y + 96;

                }
                floor[i].transformSurface.x = 528;
                floor[i].transformSurface.y = 240;
                floor[i].transform.x = wallFront[0].transform.x;

                if (i == 3 || i == 4 || i == 5)
                {
                    if (i != 3)
                    {
                        if (i == 4)
                        {
                            // Index 4 is on the right of index 3 
                            floor[i].transform.x = (WINDOW_WIDTH / 2 - 143 * 2 + 143 / 2) + (143);

                        }
                        else if (i == 5)
                        {
                            // Index 5 is on the right of index 4 
                            floor[i].transform.x = (WINDOW_WIDTH / 2 - 143 * 2 + 143 / 2) + (143 * 2);


                        }
                    }
                    else
                    {
                        //  The 3rd index od the texture array is under the index 0 
                        floor[i].transform.x = WINDOW_WIDTH / 2 - 143 * (wallFront.Length - 1) + 143 / (wallFront.Length - 1);
                    }
                }
                // From index 0 to 2 the textures are under wallFront textures
                else
                {
                    if (i != 0)
                    {
                        floor[i].transform.x = (WINDOW_WIDTH / 2 - 143 * 2 + 143 / 2) + (143 * i);
                    }
                    else
                    {
                        floor[i].transform.x = WINDOW_WIDTH / 2 - 143 * (wallFront.Length - 1) + 143 / (wallFront.Length - 1);
                    }
                }
            }
            SDL_Point ptCorner;
            ptCorner.y = 78 / 2;
            ptCorner.x = 25 / 2;
            Texture[] wallBorderBottom = new Texture[6];
            for (int i = 0; i < wallBorderBottom.Length; i++)
            {
                wallBorderBottom[i] = new Texture(renderer, SDL_image.IMG_Load("Modern tiles_Free/Interiors_free/48x48/Room_Builder_free_48x48.png"), 25, 78, 90, ptCorner);

                wallBorderBottom[i].transformSurface.x = 555;
                wallBorderBottom[i].transformSurface.y = 66;

                wallBorderBottom[i].transform.y = cornerBorderBottom[0].transform.y - 30;
                if (i != 0)
                {
                    // The next texture class is on the right of the first texture class (index 0) 
                    wallBorderBottom[i].transform.x = (cornerBorderBottom[0].transform.x + cornerBorderBottom[0].transform.w * 2) - 3 + (78 * i);
                }
                else
                {
                    // First texture class of the array is at that specific area on the window. The math is that evey texture class is displayed the middle of the screen 
                    wallBorderBottom[i].transform.x = (cornerBorderBottom[0].transform.x + cornerBorderBottom[0].transform.w * 2) - 3;
                }
                if (i == wallBorderBottom.Length - 1)
                {
                    wallBorderBottom[i].transform.h = 50;
                    wallBorderBottom[i].transform.x = (cornerBorderBottom[0].transform.x + cornerBorderBottom[0].transform.w * 2) - 40 + (78 * i);

                }

            }
            */

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

            Texture wallFront = new Texture(renderer, destinationSurface, 143 * 3, 96, 0, pt);

            wallFront.transform.x = WINDOW_WIDTH / 2 - wallFront.transform.w / 2;
            wallFront.transform.y = WINDOW_HEIGHT / 2 - 100;

            SDL_FreeSurface(destinationSurface);
            SDL_FreeSurface(srf);

            srf = SDL_image.IMG_Load("Modern tiles_Free/Interiors_free/48x48/Room_Builder_free_48x48.png");

            destinationSurface = SDL.SDL_CreateRGBSurface(0, 25, 78, 32, 0, 0, 0, 0);

            // Wall Front
            sourceRect.x = 555;
            sourceRect.y = 66;
            sourceRect.w = 25;
            sourceRect.h = 78;

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

            Texture wallBorder = new Texture(renderer, destinationSurface, 143 * 3, 96, 0, pt);

            // Creates an array texture named cornerBorder
            Texture[] cornerBorder = new Texture[2];
            for (int i = 0; i < cornerBorder.Length; i++)
            {
                cornerBorder[i] = new Texture(renderer, SDL_image.IMG_Load("Modern tiles_Free/Interiors_free/48x48/Room_Builder_free_48x48.png"), 25, 18, 0, pt);
                cornerBorder[i].transformSurface.x = 555;
                cornerBorder[i].transformSurface.y = 48;


                if (i != 0)
                {
                    // Second texture is to the right of the last wallfront texture class array
                    cornerBorder[i].transform.x = wallFront.transform.x + wallFront.transform.w;
                    cornerBorder[i].transform.y = wallFront.transform.y;

                }
                else
                {
                    // First texture is right next to the first wallFront array texture
                    cornerBorder[i].transform.x = wallFront.transform.x - cornerBorder[i].transform.x - 20;
                    cornerBorder[i].transform.y = wallFront.transform.y;
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
                //For debugging 
                Console.WriteLine("x is" + mouseX + " y is" + mouseY);

                // makes the window color light blue
                SDL_SetRenderDrawColor(renderer, 204, 255, 255, 255);
                SDL_RenderClear(renderer);

                // Present/shows the the texture 

                wallFront.show();
                for (int i = 0; i < cornerBorder.Length; i++)
                {
                    cornerBorder[i].show();
                }

                SDL_RenderPresent(renderer);

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
        public SDL_Rect transform;
        private IntPtr texture;
        private IntPtr renderer;
        public SDL_Rect transforSurface;

        public Button(int width, int height, int x, int y, IntPtr surface, IntPtr renderer)
        {
            // baisc structs for the button 

            // this need to be tweaked for to work with the texture class (this will give muche more flexebilty) 
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

            // Sets values to array by taking buttons x cord and ads +1 for every pixle of its width same for y and height.
            // This will then calculate the surface of the button plus the cordinate of every pixle 
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
        // Specifiy which added methods should be called the texture is note in scope aka not nedded 
        ~Texture()
        {
            SDL_DestroyTexture(texture);
            SDL_DestroyRenderer(renderer);
        }
    }



}

