using CopDrop;
using System;

public class buttonChangeColor : IlinkButtonScripts
{
    public void Dispose()
    {
        // Dispose of any resources, if needed
        cleanup();

        GC.SuppressFinalize(this);
        Console.WriteLine("Script is disposed");
    }
    public void cleanup()
    {
        // Clean up any additional resources
        // This method can be called explicitly or as part of dispose()
    }
    public void start()
    {
        Console.WriteLine("Button change color has been linked");
    }
    int rndX;
    int rndY;
    public void update()
    {
        
    }
}
