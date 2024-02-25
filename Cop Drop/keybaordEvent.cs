using System.Runtime.InteropServices;
namespace CopDrop
{
    public class keybaordEvent
    {
        public SDL_Event ev;

        public event EventHandler<SDL.SDL_Keycode> KeyPressed;
        public event EventHandler<SDL.SDL_Keycode> KeyReleased;

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
        public void inputListener()
        {
            SDL_GetMouseState(out int buffer1, out int buffer2);
            GlobalVariable.Instance.mouseX = buffer1;
            GlobalVariable.Instance.mouseY = buffer2;
            while (SDL_PollEvent(out ev) != 0)
            {
                switch (ev.type)
                {
                    case SDL_EventType.SDL_QUIT:
                        commandLine.Instance.cli("exit");
                        break;
                    case SDL_EventType.SDL_MOUSEBUTTONDOWN:
                        GlobalVariable.Instance.mouseButtonClick = 1;
                        break;
                    case SDL_EventType.SDL_MOUSEBUTTONUP:
                        GlobalVariable.Instance.mouseButtonClick = 0;
                        break;
                    case SDL_EventType.SDL_KEYDOWN:
                        if (GetKey(SDL_Keycode.SDLK_ESCAPE))
                        {
                            commandLine.Instance.cli("exit");
                        }
                        KeyPressed?.Invoke(this, ev.key.keysym.sym);
                        break;
                    case SDL.SDL_EventType.SDL_KEYUP:
                        KeyReleased?.Invoke(this, ev.key.keysym.sym);
                        break;
                    default:
                        break;

                }
            }

        }
    }
}
