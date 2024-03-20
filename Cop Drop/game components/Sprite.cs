namespace CopDrop
{
    public class Sprite:Texture
    {
        protected IlinkSpriteScripts customScript;
        private int collsionID;
        public Collision collision;

        public Sprite(IntPtr surface, int textureWidth, int textureHeight, int rotation, IlinkSpriteScripts customScript):base( surface,  textureWidth,  textureHeight,  rotation)
        {
            if (customScript != null)
            {
                this.customScript = customScript;
                this.customScript.loadSprite(this);
                this.customScript.start();
            }
            
        }
        public int CollsionID
        {
            get => collsionID;
            set
            {
                
                collsionID = value;
                
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


