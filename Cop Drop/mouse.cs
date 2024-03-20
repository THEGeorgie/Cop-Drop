using System.Data.Entity.Infrastructure;

namespace CopDrop
{

    public enum mouseCursors
    {
        CURSOR,
        POINTER
    }
    public class Mouse
    {

        private List<IntPtr> surface = new List<IntPtr>();
        private List<IntPtr> cursor = new List<IntPtr>();
        private string[] surfaceDIR = new string[]{
            "general/assets/cursor.png",
            "general/assets/pointingHand.png"
        };

        int currentMouseCursor = (int)mouseCursors.CURSOR;
        public Mouse()
        {
            for (int i = 0; i < surfaceDIR.Length; i++)
            {
                surface.Add(SDL_image.IMG_Load(surfaceDIR[i]));
                cursor.Add(SDL_CreateColorCursor(surface[i], 0, 0));
            }

            for (int i = 0; i < surface.Count; i++)
            {
                SDL_FreeSurface(surface[i]);
            }

        }

        public void changeCursor(mouseCursors cursors)
        {
            if (currentMouseCursor != (int)cursors)
            {
                try
                {
                    currentMouseCursor = (int)cursors;
                    Console.WriteLine("changin to " + cursors);
                    SDL_SetCursor(cursor[currentMouseCursor]);
                }
                catch (System.IndexOutOfRangeException)
                {
                    Console.WriteLine("The specified index is ot of range");
                    throw;
                }
            }

        }
    }
}