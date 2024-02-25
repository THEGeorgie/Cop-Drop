using System.Numerics;

namespace CopDrop
{
    public class Player : Texture
    {
        IlinkPlayerScripts customScript;
        Collision collision;
        public Vector2 velocity = new Vector2(0, 0);
        public Player(IntPtr surface, int textureWidth, int textureHeight, int rotation, int x, int y, IlinkPlayerScripts customScript) : base(surface, textureWidth, textureHeight, rotation)
        {
            transform.x = x;
            transform.y = y;
            this.customScript = customScript;

            if (this.customScript != null)
            {
                this.customScript.loadPlayer(this);
                customScript.start();
            }
        }

        public void update()
        {
            transform.x += (int)velocity.X;
            transform.y += (int)velocity.Y;
            velocity.Y.
            if (customScript != null)
            {
                customScript.update();
            }
        }


    }
}

