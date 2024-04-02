namespace CopDrop
{
    public class Collision
    {
        private List<SDL_Rect> collisionBox = new List<SDL_Rect>();
        private List<bool> collisionBoxDebug = new List<bool>();
        private List<Texture> collsionBoxShow = new List<Texture>();
        public Collision()
        {
            Console.WriteLine("Collision system initelized");
        }

        private SDL_Rect rect1;
        private SDL_Rect rect2;
        public void update()
        {
        }

        public void render()
        {
            
        }
        
        public bool isCollision(int indexColliosn, out float overlapX,out float overlapY)
        {
            for (int i = 0; i < collisionBox.Count; i++)
            {
                
                    rect2 = collisionBox[i];
                    rect1 = collisionBox[indexColliosn];
                    if (SDL_HasIntersection( ref rect1, ref rect2) == SDL_bool.SDL_TRUE && i != indexColliosn)
                    {
                        int rect1CenterX = rect1.x + rect1.w / 2;
                        int rect1CenterY = rect1.y + rect1.h / 2;
                        int rect2CenterX = rect2.x + rect2.w / 2;
                        int rect2CenterY = rect2.y + rect2.h / 2;   
                        
                        int dx = rect1CenterX - rect2CenterX;
                        int dy = rect1CenterY - rect2CenterY;
                        int penetration = Math.Max(Math.Abs(dx), Math.Abs(dy));
                        if (penetration != 0)
                        {
                            overlapX = dx * (rect1.w / 2 + rect2.w / 2 - penetration) / penetration;
                            overlapY = dy * (rect1.h / 2 + rect2.h / 2 - penetration) / penetration;
                            return true;
                        }
                         
                    }
                    
            }

            overlapX = 0;
            overlapY = 0;

            return false;
        }

        public float[] getColliosonBoxValue(int collisionBoxID)
        {
            float[] ret = {collisionBox[collisionBoxID].x,collisionBox[collisionBoxID].y,collisionBox[collisionBoxID].w,collisionBox[collisionBoxID].h};
            return ret;
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
