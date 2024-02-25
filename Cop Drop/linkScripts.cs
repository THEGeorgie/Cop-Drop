namespace CopDrop
{
    public interface IlinkButtonScripts
    {
        void start();
        void update();

        void loadButton(Button btn);
    }

    public interface IlinkTextureScripts
    {
        void start();
        void update();
        void loadTexture(Texture texture);
    }
    public interface IlinkPlayerScripts
    {
        void start();
        void update();
        void loadPlayer(Player player);
    }
}


