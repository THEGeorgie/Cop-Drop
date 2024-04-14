namespace CopDrop
{

    class Program
    {
        public static int Main()
        {

            const int FPS = 60;
            const int frameDelay = 1000 / FPS;

            UInt32 frameStart;

            Game game = new Game(1024, 600);

            bool loop = true;
            while (loop)
            {
                frameStart = SDL_GetTicks();
                if (GlobalVariable.Instance.exit)
                {
                    loop = false;
                }
                game.update();
                game.render();

                GlobalVariable.Instance.frameTime = (int)(SDL_GetTicks() - frameStart);

                if (frameDelay > GlobalVariable.Instance.frameTime)
                {
                    SDL_Delay((uint)(frameDelay - GlobalVariable.Instance.frameTime));
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
        public int WINDOW_WIDTH { get; set; }
        public int WINDOW_HEIGHT { get; set; }
        public int mouseButtonClick { get; set; }
        public IntPtr font;
        public IntPtr renderer;
        public int frameTime;

        public keybaordEvent keybaordEvent;

        public bool exit { get; set; }

        public Mouse mouse;

        private GlobalVariable()
        {
            // Initialize your global variable here
            mouse = new Mouse();
            keybaordEvent = new keybaordEvent();


            mouseX = 42;
            mouseY = 42;
            mouseButtonClick = 0;
            font = TTF_OpenFont("m6x11.ttf", 13);
            if (font == null)
            {
                Console.WriteLine("Failed to load font: %s", SDL_GetError());
            }
        }



        public static GlobalVariable Instance
        {
            get
            {
                return LazyInstance.Value;
            }
        }
        private static readonly Lazy<GlobalVariable> LazyInstance = new Lazy<GlobalVariable>(() => new GlobalVariable());
    }
}