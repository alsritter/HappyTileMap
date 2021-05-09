using System;
using System.Collections;
using System.Collections.Generic;
using AlsRitter.CustomTileFrame.CommonTileEnum;
using AlsRitter.CustomTileFrame.Tile;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace AlsRitter.CustomTileFrame
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
        public void InitializeMyTileInfo(string[] effectKeys0, string tileSpriteId0, DisplayModel model0,
            TileTag[] tags0)
        {
            this.effectKeys = effectKeys0;
            this.tileSpriteId = tileSpriteId0;
            this.model = model0;
            this.tags = tags0;
            RefreshTileInfo();
        }
    }
}