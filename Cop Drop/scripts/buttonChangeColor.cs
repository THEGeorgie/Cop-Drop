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

    public void update()
    {
        if (button.isButtonPressed())
        {
            Console.WriteLine("it works!!!!");
            button.transform.x += 10;
        }
    }
}
