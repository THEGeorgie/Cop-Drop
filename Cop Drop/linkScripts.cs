namespace CopDrop
{
    public interface IlinkButtonScripts : IDisposable
    {
        void start();
        void update();
        void cleanup();
    }

    public interface IlinkTextureScripts : IDisposable
    {
        void start();
        void update();
        void cleanup();
    }
}


