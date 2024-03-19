namespace CopDrop
{
    public interface IlinkButtonScripts
    {
        void start();
        void update();

        void loadButton(Button btn);
    }

    public interface IlinkSpriteScripts
    {
        void start();
        void update();
        void loadSprite(Sprite sprite);
    }
    public interface IlinkPlayerScripts
    {
        void start();
        void update();
        void loadPlayer(Player player);
    }
}


