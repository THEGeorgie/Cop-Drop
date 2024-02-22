namespace CopDrop
{
    public class Player : Texture
    {
        IlinkPlayerScripts customScript;
        Collision collision;
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
            if (customScript != null)
            {
                customScript.update();
            }
        }


    }
}

