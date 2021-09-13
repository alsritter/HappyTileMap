using AlsRitter.GenerateMap.CustomTileFrame;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEngine;

namespace AlsRitter.GenerateMap.Interface.Do
{
    /**
     * 这里面存的是各个 Tile 的通用信息
     */
    public class TileData
    {
        public TileData(string key, DisplayLayer layer, string tileSpriteId, string color, string[] effectKeys,
            TileTag[] tags)
        {
            this.key = key;
            this.layer = layer;
            TileSpriteId = tileSpriteId;
            Color = color;
            EffectKeys = effectKeys;
            Tags = tags;
        }

        /**
         * 存储一个 key，方便给 TilePoint 索引
         */
        public string key { get; }


        /**
         * 显示模式：
         * 1 背景
         * 2 碰撞
         * 3 前景
         */
        public DisplayLayer layer { get; }

        /**
         * 图片的编号
         */
        public string TileSpriteId { get; }

        /**
         * 16 进制颜色码
         */
        public string Color { get; }

        /**
         * 效果的编号
         */
        public string[] EffectKeys { get; }

        /**
         * tags 的编号
         */
        public TileTag[] Tags { get; }
    }
}