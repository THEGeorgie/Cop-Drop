namespace CopDrop
{
    public class Texture
    {
        // All of the struct nedded for a texture

        // Used for transforming the whole texture
        public SDL_Rect transform;

        // Used for transforming the surface of the texture
        public SDL_Rect transformSurface;

        // The texture variable stored as a integer pointer
        public IntPtr texture;

        // For the rottation of the texture
        public double rotation = 0.0;

        // The point of the texture most often the center cordinantes of the texture
        public SDL_Point point;

        private IntPtr surface;

        // Initalizing all of the structs/variable that are passed from the constructor
        public Texture(IntPtr surface, int textureWidth, int textureHeight, int rotation)
        {
            this.surface = surface;
            this.rotation = rotation;
            texture = SDL_CreateTextureFromSurface(GlobalVariable.Instance.renderer, surface);
            point.x = textureWidth / 2;
            point.y = textureHeight / 2;

            transform.w = textureWidth;
            transform.h = textureHeight;
            transform.x = 0;
            transform.y = 0;
            SDL_Surface surfaceInfo = (SDL.SDL_Surface)System.Runtime.InteropServices.Marshal.PtrToStructure(surface, typeof(SDL.SDL_Surface));

            transformSurface.w = surfaceInfo.w;
            transformSurface.h = surfaceInfo.h;
            transformSurface.x = 0;
            transformSurface.y = 0;
        }
        //!NOT TO BE USED FOR TEXTURE THAT REQUIRE A SPECIAL WIDTH AND HEIGHT ie. sprites
        //mainly used for text rendering
         public Texture(IntPtr surface, int rotation)
        {
            int textureWidth;
            int textureHeight;

            this.surface = surface;
            this.rotation = rotation;
            texture = SDL_CreateTextureFromSurface(GlobalVariable.Instance.renderer, surface);
            SDL_QueryTexture(texture, out _,  out _, out textureWidth, out textureHeight);
            point.x = textureWidth / 2;
            point.y = textureHeight / 2;

            transform.w = textureWidth;
            transform.h = textureHeight;
            transform.x = 0;
            transform.y = 0;

            transformSurface.w = textureWidth;
            transformSurface.h = textureHeight;
            transformSurface.x = 0;
            transformSurface.y = 0;
        }

        public void show()
        {
            SDL_RenderCopyEx(GlobalVariable.Instance.renderer, texture, ref transformSurface, ref transform, rotation, ref point, SDL_RendererFlip.SDL_FLIP_NONE);
        }

        public Texture deepCopy()
        {
            Texture deepCopyTexture = new Texture(surface, transform.w, transform.h, (int)rotation);

            return deepCopyTexture;

        }

        // Code celanup

        public void discrad()
        {
            SDL_DestroyTexture(texture);
        }
    }
}
