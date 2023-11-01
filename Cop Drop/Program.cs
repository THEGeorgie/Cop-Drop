/*
    using Microsoft.VisualBasic;
    using System.Text.Json.Nodes;
    using System.Security.AccessControl;
    using System.Xml.Serialization;
*/
using System.Runtime.InteropServices;

namespace CopDrop
{
    class Program
    {
        public static int Main()
        {

            const int FPS = 30;
            const int frameDelay = 1000 / FPS;

            UInt32 frameStart;
            int frameTime;

            Game game = new Game(1024, 600);

            bool loop = true;
            while (loop)
            {
                frameStart = SDL_GetTicks();

                if (game.inputListener() || GlobalVariable.Instance.exit)
                {
                    loop = false;
                }
                game.update();
                game.render();

                frameTime = (int)(SDL_GetTicks() - frameStart);

                if (frameDelay > frameTime)
                {
                    SDL_Delay((uint)(frameDelay - frameTime));
                }
            }
            game.deallocate();


            return 0;
        }
    }

    public class GlobalVariable
    {
        private static GlobalVariable instance;
        public int mouseX { get; set; }
        public int mouseY { get; set; }
        public int mouseButtonClick { get; set; }
        public int openSnekaerDetails { get; set; }
        public IntPtr font = TTF_OpenFont("font.otf", 13);
        public IntPtr renderer;

        public bool exit {get; set;}

        private GlobalVariable()
        {
            // Initialize your global variable here
            mouseX = 42;
            mouseY = 42;
            mouseButtonClick = 0;
            if (font == null)
            {
                Console.WriteLine("Failed to load font: %s", SDL_GetError());
            }
        }
        
        public bool GetKey(SDL.SDL_Keycode _keycode)
        {
            int arraySize;
            bool isKeyPressed = false;
            IntPtr origArray = SDL.SDL_GetKeyboardState(out arraySize);
            byte[] keys = new byte[arraySize];
            byte keycode = (byte)SDL.SDL_GetScancodeFromKey(_keycode);
            Marshal.Copy(origArray, keys, 0, arraySize);
            isKeyPressed = keys[keycode] == 1;
            return isKeyPressed;
        }

        public static GlobalVariable Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GlobalVariable();
                }
                return instance;
            }
        }

    }
}

















