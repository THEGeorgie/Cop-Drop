using System.Numerics;

namespace CopDrop
{
    public class Player : Texture
    {
        IlinkPlayerScripts customScript;
        public Vector2 velocity = new Vector2(0, 0);
        private int collisionID;
        public Collision collision;
        public Player(IntPtr surface, int textureWidth, int textureHeight, int rotation, int x, int y, IlinkPlayerScripts customScript,Collision collision) : base(surface, textureWidth, textureHeight, rotation)
        {
            transform.x = x;
            transform.y = y;
            this.customScript = customScript;
            this.collision = collision;
            if (this.customScript != null)
            {
                this.customScript.loadPlayer(this);
                customScript.start();
            }
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
            transform.x += (int)velocity.X;
            transform.y += (int)velocity.Y;
            if (customScript != null)
            {
                customScript.update();
            }
        }


    }
}

