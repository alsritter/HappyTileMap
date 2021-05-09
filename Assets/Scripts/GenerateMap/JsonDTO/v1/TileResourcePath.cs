namespace AlsRitter.GenerateMap.CustomTileFrame.MapDataEntity.V1.Dto
{

    /// <summary>
    /// 封装资源路径
    /// </summary>
    public class TileResourcePath
    {
        public enum SpriteMode
        {
            Single = 0,
            Multiple = 1
        }

        public TileResourcePath(string spriteId, string path, SpriteMode mode)
        {
            this.spriteId = spriteId;
            this.path = path;
            this.mode = mode;
        }

        public string spriteId { get; }
        public string path { get; }
        public SpriteMode mode { get; }
    }
}
