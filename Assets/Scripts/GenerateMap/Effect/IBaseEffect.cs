namespace AlsRitter.V3.CustomTileFrame.TileEffect
{
    /**
     * 效果基类
     */
    public interface IBaseEffect
    {
        void ApplyTo(IPlayer player);
        int    versionUID { get; } // 用于做版本控制，如果子类更新了构造函数，需要更新这个版本号
        string name       { get; }
    }
}

