using CopDrop;
using System;

public class buttonChangeColor : IlinkButtonScripts
{
    private Button button;

    public buttonChangeColor(Button button)
    {
        this.button = button;
    }
    public void start()
    {

    }
    int rndX;
    int rndY;
    public void update()
    {
        if (button.isButtonPressed())
        {
            rndX = Random.Shared.Next(1,1024 - button.transform.w);
            rndY = Random.Shared.Next(1,600 - button.transform.h);
            button.transform.x = rndX;
            button.text.x = rndX + 15;
            button.transform.y = rndY;
            button.text.y = rndY + 15;
            button.text.update();
        }
    }
}
