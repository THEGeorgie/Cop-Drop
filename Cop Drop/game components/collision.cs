namespace CopDrop
{
    public class Collision
    {
        private List<SDL_Rect> collisionBox = new List<SDL_Rect>();
        private List<bool> collisionBoxDebug = new List<bool>();
        private List<Texture> collsionBOxShow = new List<Texture>();
        public Collision()
        {
            Console.WriteLine("Collision system initelized");
        }

        private SDL_Rect rect1;
        private SDL_Rect rect2;
        private SDL_Rect rect3;
        public void update()
        {
            
        }

        public void render()
        {
            
           

            
        }
        
        public bool isCollision(int indexColliosn)
        {
            for (int i = 0; i < collisionBox.Count; i++)
            {
                
                    rect1 = collisionBox[i];
                    rect2 = collisionBox[indexColliosn];
                    if (SDL_IntersectRect( ref rect1, ref rect2, out _) == SDL_bool.SDL_TRUE && i != indexColliosn)
                    {
                        return true;
                    }
                
                
            }

            return false;
        }

        public void setCollisionBoxValue(int collisonBoxID,int x, int y, int w, int h)
        {
            collisionBox[collisonBoxID] = collisionBox[collisonBoxID] with {x = x, y = y, w = w, h = h};
        }
        public void setCollisionBoxValue(int collisonBoxID,int value, char valueToChange)
        {
            switch (valueToChange)
            {
                case 'x':
                    collisionBox[collisonBoxID] = collisionBox[collisonBoxID] with {x = value};
                    break;
                case 'y':
                    collisionBox[collisonBoxID] = collisionBox[collisonBoxID] with {y = value};
                    break;
                case 'w':
                    collisionBox[collisonBoxID] = collisionBox[collisonBoxID] with {w = value};
                    break;
                case 'h':
                    collisionBox[collisonBoxID] = collisionBox[collisonBoxID] with {h = value};
                    break;
                default:
                    Console.WriteLine("Value type unknown");
                    break;
            }
            
        }
            
        public int addCollisionBox(SDL_Rect collision, bool debug)
        {
            collisionBox.Add(collision);
            Console.WriteLine(collisionBox.Count);
            Console.WriteLine(collisionBox[0].w);
            
            collisionBoxDebug.Add(debug);
            if (debug)
            {
            }
            return collisionBox.Count - 1;
        }
    }
}
