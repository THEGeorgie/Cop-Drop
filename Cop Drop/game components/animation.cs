namespace CopDrop
{
    public class Animation
    {
        public Animation(Texture texture)
        {
            this.texture = texture;
        }
        Texture texture;
        List<string> animationsNames = new List<string>();
        List<AnimationStrcuct> animations = new List<AnimationStrcuct>();
        public struct AnimationStrcuct
        {
            public AnimationStrcuct(SDL_Rect rect, int animationSpeed, float startY, int frames)
            {
                this.rect = rect;
                this.animationSpeed = animationSpeed;
                this.frames = frames;
                this.startY = startY;
            }
            SDL_Rect rect;
            public int RectX
            {
                get { return rect.x; }
            }
            public int RectY
            {
                get { return rect.y; }
            }
            public int RectW
            {
                get { return rect.w; }
            }
            public int RectH
            {
                get { return rect.h; }
            }
            public int Frames
            {
                get { return frames; }
            }
            int frames;
            public float startY;
            public int animationSpeed;
        }
        public void create(string name, float startY, int animationSpeed, int frames)
        {
            animationsNames.Add(name);
            animations.Add(new AnimationStrcuct(texture.transformSurface, animationSpeed, startY, frames));
        }


        //plays the animation
        public void play(string animation)
        {
            if (animations != null && animationsNames != null)
            {
                for (int i = 0; i < animationsNames.Count; i++)
                {
                    if (animation == animationsNames[i])
                    {
                        texture.transformSurface.x = texture.transformSurface.w * (int)(SDL_GetTicks() / animations[i].animationSpeed % animations[i].Frames);
                        
                    }
                }
            }

        }
    }
}


