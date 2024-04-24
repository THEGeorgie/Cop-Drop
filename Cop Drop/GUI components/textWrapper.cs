namespace CopDrop
{
    public class TextWrapper:Text
    {
        protected IlinkTextScripts customScript;
        public TextWrapper(string txt, int fontSize, SDL_Color fgg,IlinkTextScripts customScript):base(txt, fontSize, fgg){
            if (customScript != null)
            {
                this.customScript = customScript;
                this.customScript.loadText(this);
                this.customScript.start();
            }
        }

        public void update()
        {
            if (customScript != null)
            {
                customScript.update();
            }
            base.update();
        }
    }
}


