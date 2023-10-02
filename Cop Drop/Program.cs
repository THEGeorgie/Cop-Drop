// A runtime library used for pixel accses
using SDL2;
using static SDL2.SDL;

//Gpu rendering libraries

// for displaying dynamic texts
using static SDL2.SDL_ttf;


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
            // makes the window color light blue
            SDL_SetRenderDrawColor(renderer, 204, 255, 255, 255);
            SDL_RenderClear(renderer);

            int mouseX = 0;
            int mouseY = 0;


            SDL_Point pt;
            pt.y = 0;
            pt.x = 0;

            Texture storeBtnTexture = new Texture(renderer, SDL_image.IMG_Load("storeAssets/stockXIcon.png"), 48, 48, 0, pt);
            Button storeBtn = new Button(storeBtnTexture, 50, 50);
            Map mapRoom = new Map(renderer, 'r', WINDOW_WIDTH, WINDOW_HEIGHT);
            Map mapStore = new Map(renderer, 's', WINDOW_WIDTH, WINDOW_HEIGHT);

            SDL_Event ev;
            bool loop = true;
            bool switchMap = false;
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
                                switchMap = !switchMap;
                            }
                            break;
                    }
                }
                //For debugging 
                //Console.WriteLine("x is" + mouseX + " y is" + mouseY);
                if (switchMap)
                {
                    SDL_RenderClear(renderer);
                    mapStore.present();
                }
                else
                {
                    SDL_RenderClear(renderer);
                    mapRoom.present();
                }
                if (switchMap)
                {
                    SDL_RenderClear(renderer);
                    mapStore.present();
                }
                storeBtn.texture.show();

                // Present/shows the the texture and deletes them

                SDL_Delay(10);

                SDL_RenderPresent(renderer);
                mapStore.release();
                mapRoom.release();
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
        public Button(Texture tx, int x, int y)
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
        private IntPtr renderer;
        char map = ' ';
        int MAP_WIDTH;
        int MAP_HEIGHT;
        public Map(IntPtr renderer, char map, int MAP_WIDTH, int MAP_HEIGHT)
        {
            this.renderer = renderer;
            this.map = map;
            this.MAP_WIDTH = MAP_WIDTH;
            this.MAP_HEIGHT = MAP_HEIGHT;
            mapBuilder();
        }
        Texture wallFront;
        Texture[] cornerBorder = new Texture[2];
        Texture wallBorderLeft;
        Texture wallBorderRight;
        Texture[] cornerBorderBottom = new Texture[2];
        Texture floor;
        Texture wallBorderBottom;

        Texture[] sneakers;
        Texture storeName;
        enum texturesNames
        {
            wallFront = 1,
            cornerBorder = 2,
            wallborder = 3,
            floor = 4
        }
        /*
        int[,] map =
        {
            {0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0},
            {0,0,1,1,1,1,0,0},
            {0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0},

        };
        */
        private void mapBuilder()
        {
            // Level building code
            SDL_Point pt;
            pt.x = 0;
            pt.y = 0;

            var srf = SDL_image.IMG_Load("Modern tiles_Free/Interiors_free/48x48/Room_Builder_free_48x48.png");

            var destinationSurface = SDL.SDL_CreateRGBSurface(0, 143 * 3, 96, 32, 0, 0, 0, 0);



            switch (map)
            {
                case 'r':
                    wallFront = null;
                    cornerBorder = null;
                    wallBorderLeft = null;
                    wallBorderRight = null;
                    cornerBorderBottom = null;
                    floor = null;
                    wallBorderBottom = null;
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

                    wallFront = new Texture(renderer, destinationSurface, 143 * 3, 96, 0, pt);

                    wallFront.transform.x = MAP_WIDTH / 2 - wallFront.transform.w / 2;
                    wallFront.transform.y = MAP_HEIGHT / 2 - 100;


                    // Creates an array texture named cornerBorder
                    cornerBorder = new Texture[2];
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

                    wallBorderLeft = new Texture(renderer, destinationSurface, 21, 78 * 3, 0, pt);

                    destinationSurface = SDL.SDL_CreateRGBSurface(0, 25, 78, 32, 0, 0, 0, 0);

                    wallBorderLeft.transform.x = cornerBorder[0].transform.x;
                    wallBorderLeft.transform.y = cornerBorder[0].transform.y + cornerBorder[0].transform.h;

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
                    wallBorderRight = new Texture(renderer, destinationSurface, 21, 78 * 3, 0, pt);
                    wallBorderRight.transform.x = cornerBorder[1].transform.x;
                    wallBorderRight.transform.y = cornerBorder[1].transform.y + cornerBorder[1].transform.h;

                    // cornerBorderBottom code
                    cornerBorderBottom = new Texture[2];
                    for (int i = 0; i < cornerBorderBottom.Length; i++)
                    {
                        cornerBorderBottom[i] = new Texture(renderer, SDL_image.IMG_Load("Modern tiles_Free/Interiors_free/48x48/Room_Builder_free_48x48.png"), 25, 18, 0, pt);
                        cornerBorderBottom[i].transformSurface.x = 555;
                        cornerBorderBottom[i].transformSurface.y = 48;


                        if (i != 0)
                        {
                            // Second texture is to the right of the last wallfront texture class array
                            cornerBorderBottom[i].transform.x = wallFront.transform.x + wallFront.transform.w;
                            cornerBorderBottom[i].transform.y = wallBorderRight.transform.y + wallBorderLeft.transform.h;

                        }
                        else
                        {
                            // First texture is right next to the first wallFront array texture
                            cornerBorderBottom[i].transform.x = wallFront.transform.x - cornerBorderBottom[i].transform.x - 20;
                            cornerBorderBottom[i].transform.y = wallBorderRight.transform.y + wallBorderLeft.transform.h;
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


                    floor = new Texture(renderer, destinationSurface2, 143 * 3, 85 * 2, 0, pt);
                    floor.transform.x = wallFront.transform.x;
                    floor.transform.y = wallFront.transform.y + wallFront.transform.h;


                    SDL_FreeSurface(destinationSurface);

                    // Wall borderBottom
                    destinationSurface = SDL.SDL_CreateRGBSurface(0, 41 * 10, 18, 32, 0, 0, 0, 0);
                    sourceRect.x = 577;
                    sourceRect.y = 144;
                    sourceRect.w = 41;
                    sourceRect.h = 18;
                    for (int i = 0; i < 10; i++)
                    {
                        destinationRect.x = 41 * i;
                        destinationRect.y = 0;
                        destinationRect.w = 41;
                        destinationRect.h = 18;

                        // Copies the srf surface on the destination surface 
                        SDL_BlitSurface(srf, ref sourceRect, destinationSurface, ref destinationRect);
                    }

                    SDL_Point ptCenter;


                    ptCenter.x = 41 * 10 / 2;
                    ptCenter.y = 18 / 2;
                    wallBorderBottom = new Texture(renderer, destinationSurface, 41 * 10, 18, 0, ptCenter);
                    wallBorderBottom.transform.x = cornerBorderBottom[0].transform.x + cornerBorderBottom[0].transform.w - 4;
                    wallBorderBottom.transform.y = cornerBorderBottom[0].transform.y;

                    SDL_FreeSurface(srf);

                    break;
                case 's':
                    sneakers = null;
                    storeName = null;
                    SDL_SetRenderDrawColor(renderer, 255, 255, 255, 255);
                    SDL_RenderClear(renderer);

                    storeName = new Texture(renderer, SDL_image.IMG_Load("storeAssets/storeName.png"), 118, 48, 0, pt);
                    storeName.transform.x = MAP_WIDTH / 2;
                    break;
            }
            /*
            int[] arrayTexturelist =
            {
                (int)texturesNames.wallFront, (int)texturesNames.cornerBorder, (int)texturesNames.floor,
                (int)texturesNames.wallborder
            };
            int count = 0;
            int previusRow = 0;
            int newRow = 0;
            for (int i = 0; i < 7; i++)
            {
                for (int k = 0; k < 8; k++)
                {
                    newRow = i;
                    if (map[i, k] == (int)texturesNames.wallFront && previusRow == newRow)
                    {
                        sourceRect.x = 0;
                        sourceRect.y = 239;
                        sourceRect.w = 143;
                        sourceRect.h = 96;

                        destinationRect.x = 143 * count;
                        destinationRect.y = 0;
                        destinationRect.w = 143;
                        destinationRect.h = 96;
                        Console.WriteLine(count);
                        // Copies the srf surface on the destination surface
                        SDL_BlitSurface(srf, ref sourceRect, destinationSurface, ref destinationRect);
                        count++;
                    }
                    previusRow = newRow;
                }
            }

            Texture textures = new Texture(renderer, destinationSurface, 143 * 4, 96, 0, pt);
            */
        }

        public void present()
        {
            switch (map)
            {
                case 'r':
                    floor.show();
                    wallFront.show();
                    for (int i = 0; i < cornerBorder.Length; i++)
                    {
                        cornerBorder[i].show();
                        cornerBorderBottom[i].show();
                    }
                    wallBorderLeft.show();
                    wallBorderRight.show();
                    wallBorderBottom.show();
                    break;
                case 's':
                    storeName.show();
                    break;
            }
            Console.WriteLine(map);
        }
        public void release()
        {
            switch (map)
            {
                case 'r':
                    floor.discrad();
                    wallFront.discrad();
                    for (int i = 0; i < cornerBorder.Length; i++)
                    {
                        cornerBorder[i].discrad();
                        cornerBorderBottom[i].discrad();
                    }
                    wallBorderLeft.discrad();
                    wallBorderRight.discrad();
                    wallBorderBottom.discrad();
                    break;
                case 's':
                    storeName.discrad();
                    break;
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

