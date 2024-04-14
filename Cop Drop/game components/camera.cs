namespace CopDrop
{
    public class Camera
    {
        private SDL_Rect viewport;
        public Camera(int Cx, int Cy, int width, int height){
            viewport = new SDL_Rect{x = Cx, y = Cy, w = width, h = height};
        }

        public int x{
            get {return viewport.x;}
            set {viewport.x = value;}
        }

        public int y{
            get {return viewport.y;}
            set {viewport.y = value;}
        }

        public int w{
            get {return viewport.w;}
            set {viewport.w = value;}
        }

        public int h{
            get {return viewport.h;}
            set {viewport.h = value;}
        }
        public void update(){
            //SDL_RenderSetClipRect(GlobalVariable.Instance.renderer,ref viewport);
        }
    }
}


