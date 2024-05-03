namespace CopDrop
{
    public class Sprite : Texture
    {
        protected IlinkSpriteScripts customScript;
        private int collisionID;
        public Collision collision;

        public Sprite(IntPtr surface, int textureWidth, int textureHeight, int rotation, IlinkSpriteScripts customScript, Collision collision) : base(surface, textureWidth, textureHeight, rotation)
        {
            this.collision = collision;
            if (customScript != null)
            {
                this.customScript = customScript;
                this.customScript.loadSprite(this);
                this.customScript.start();
            }
        }
        public Sprite(IntPtr surface, int textureWidth, int textureHeight, int rotation, int x, int y, IlinkSpriteScripts customScript, Collision collision) : base(surface, textureWidth, textureHeight, rotation)
        {


            this.collision = collision;

            if (customScript != null)
            {
                this.customScript = customScript;
                this.customScript.loadSprite(this);
                this.customScript.start();
            }

            transform.x = x;
            transform.y = y;
        }
        public int CollisionID
        {
            get => collisionID;
            set
            {

                collisionID = value;

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


