using System.Drawing;

namespace CopDrop
{
    public class Collision
    {


        private List<CollisionElement> CollisionElements = new List<CollisionElement>();

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

        public bool isCollision(int indexColliosn, out float overlapX, out float overlapY, out char collisionTypeO)
        {
            for (int i = 0; i < CollisionElements.Count; i++)
            {

                rect2 = CollisionElements[i].collisionBox;
                rect1 = CollisionElements[indexColliosn].collisionBox;
                if (SDL_HasIntersection(ref rect1, ref rect2) == SDL_bool.SDL_TRUE && i != indexColliosn)
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
                        collisionTypeO = CollisionElements[i].collisionType;
                        return true;
                    }

                }

            }

            overlapX = 0;
            overlapY = 0;
            collisionTypeO = ' ';
            return false;
        }

        public float[] getColliosonBoxValue(int collisionBoxID)
        {
            float[] ret = { CollisionElements[collisionBoxID].collisionBox.x, CollisionElements[collisionBoxID].collisionBox.y, CollisionElements[collisionBoxID].collisionBox.w, CollisionElements[collisionBoxID].collisionBox.h };
            return ret;
        }
        public void setCollisionBoxValue(int collisonBoxID, int x, int y, int w, int h)
        {
            CollisionElements[collisonBoxID].collisionBox = CollisionElements[collisonBoxID].collisionBox with { x = x, y = y, w = w, h = h };
        }
        public void setCollisionBoxValue(int collisonBoxID, int value, char valueToChange)
        {
            switch (valueToChange)
            {
                case 'x':
                    CollisionElements[collisonBoxID].collisionBox.x = value;
                    break;
                case 'y':
                    CollisionElements[collisonBoxID].collisionBox.y = value;
                    break;
                case 'w':
                    CollisionElements[collisonBoxID].collisionBox.w = value;
                    break;
                case 'h':
                    CollisionElements[collisonBoxID].collisionBox.h = value;
                    break;
                default:
                    Console.WriteLine("Value type unknown");
                    break;
            }
        }
        public void addCollisionType(int collisonBoxID, char value)
        {
            CollisionElements[collisonBoxID].collisionType = value;
        }
        public int addCollisionBox(SDL_Rect collision, bool debug, char type)
        {
            CollisionElements.Add(new CollisionElement(collision, debug, type));
            Console.WriteLine($"Collision system has {CollisionElements.Count}");
            return CollisionElements.Count -1;
        }
    }
    public class CollisionElement
    {
        public SDL_Rect collisionBox;
        public bool collisionBoxDebug;
        public Texture collsionBoxShow;
        public char collisionType;

        public CollisionElement(SDL_Rect collision, bool debug, char type){
            collisionBox = collision;
            collisionBoxDebug = debug;
            collisionType = type;
        }
    }
}
