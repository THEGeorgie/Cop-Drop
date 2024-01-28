namespace CopDrop
{
    public class Animation
    {
        Texture texture;
        public Animation(Texture texture,float playbackSpeed){
            this.texture = texture;
        }

        //plays the animation
        public void play(){
            texture.transformSurface.x += texture.transform.w;
        }
    }
}


