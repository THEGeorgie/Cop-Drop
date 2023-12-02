namespace CopDrop
{
    public class Text
    {
        Texture texture;
        IntPtr surface;
        SDL_Color fg;
        int fontSize;
        public int x { get; set; }
        public int y { get; set; }
        public int rotation { get; set; }

        string txt;
        public Text(string txt, int fontSize, SDL_Color fgg)
        {
            this.fg = fgg;
            this.txt = txt;
            this.fontSize = fontSize;
            TTF_SetFontSize(GlobalVariable.Instance.font, fontSize);
            surface = TTF_RenderText_Solid(GlobalVariable.Instance.font, txt, fg);
            Console.WriteLine("THe red color is" + fg.r);
            x = 0;
            y = 0;
            rotation = 0;
            texture = new Texture(surface, fontSize * txt.Length, fontSize, rotation);
            SDL_FreeSurface(surface);
        }
        public void render()
        {
            texture.show();
        }
        public Text deepCopy(){
            Text deepCopy = new Text(txt, fontSize, fg);
            return deepCopy;
        }
        public void update()
        {
            texture.transform.x = x;
            texture.transform.y = y;
            texture.rotation = rotation;
        }
        public void updateText(string txt)
        {
            surface = TTF_RenderText_Solid(GlobalVariable.Instance.font, txt, fg);
            if (texture != null)
            {
                texture.discrad();
            }
            texture = new Texture(surface, fontSize * txt.Length, fontSize, rotation);
            SDL_FreeSurface(surface);
        }
        public void dealocate()
        {
            texture.discrad();
        }
    }
}
