namespace CopDrop
{
    class Button : Texture
    {
        int[] buttonAreaPositionX;
        int[] buttonAreaPositionY;

        public Button(IntPtr surface, int textureWidth, int textureHeight, int rotation, int x, int y) : base(surface, textureWidth, textureHeight, rotation)
        {
            transform.x = x;
            transform.y = y;

            // Sets values to array by taking buttons x cord and ads +1 for every pixle of its width same for y and height.
            // This will then calculate the surface of the button plus the cordinate of every pixle 
            buttonAreaPositionX = new int[transform.w];
            buttonAreaPositionY = new int[transform.h];
            for (int i = 0; i < transform.w; i++)
            {
                buttonAreaPositionX[i] = transform.x + i;
            }
            for (int i = 0; i < transform.h; i++)
            {
                buttonAreaPositionY[i] = transform.y + i;
            }
        }

        public bool isButtonPressed()
        {
            for (int i = 0; i < transform.w; i++)
            {
                for (int k = 0; k < transform.h; k++)
                {
                    //checks if the mouse cords are in the area of the button 
                    if (buttonAreaPositionX[i] == GlobalVariable.Instance.mouseX && buttonAreaPositionY[k] == GlobalVariable.Instance.mouseY)
                    {
                        return true;
                    }

                }
            }
            return false;
        }
    }
    public class commandLine
    {
        private string[] accitonList = { "load_map", "exit" };
        private string[] severd;

        private DAccitons[] dAccitons = new DAccitons[2];
        public commandLine()
        {
            dAccitons[0] = LoadMap;
            dAccitons[1] = Exit;
        }
        public void cli(string command)
        {
            severd = command.Split(' ');
            for (int i = 0; i < severd.Length; i++)
            {
                Console.WriteLine(severd[i]);
            }
            for (int i = 0; i < accitonList.Length; i++)
            {
                if (severd[0] == accitonList[i])
                {
                    dAccitons[i](severd[1]);
                }
            }
        }
        DAccitons LoadMap = (string mapLocation) =>
        {
            Map map = new Map(mapLocation);
            try
            {
                MapManager.Instance.LoadMap(map);
            }
            catch (System.Exception)
            {
                Console.WriteLine("Map specifed dose not exist");
                throw;
            }

        };
        DAccitons Exit = (string blank) =>
        {
            GlobalVariable.Instance.exit = true;
        };

        private delegate void DAccitons(string input);
        public static commandLine Instance
        {
            get
            {
                return LazyInstance.Value;
            }
        }

        private static readonly Lazy<commandLine> LazyInstance = new Lazy<commandLine>(() => new commandLine());
    }

}
