namespace AlsRitter.GenerateMap.CustomTileFrame.TileScripts
{
    /// <summary>
    /// 用于读取 JSON 实例化的 Tile
    /// </summary>
    public class CustomTile : CustomBaseTile
    {
        /// <summary>
        /// 用于传入初始参数的工具
        /// </summary>
        /// <param name="effectKeys0"></param>
        /// <param name="tileImageId0"></param>
        /// <param name="model0"></param>
        public void InitializeCustomTile(string[] effectKeys0, string tileSpriteId0, DisplayLayer layer0,
            TileTag[] tags0)
        {
            this.effectKeys = effectKeys0;
            this.tileSpriteId = tileSpriteId0;
            this.layer = layer0;
            this.tags = tags0;
            RefreshTileInfo();
        }
    }
}