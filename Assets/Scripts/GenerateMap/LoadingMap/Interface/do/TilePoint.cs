using AlsRitter.GenerateMap.CustomTileFrame;
using UnityEngine;

namespace AlsRitter.GenerateMap.Interface.Do {
    /**
     * 存储 Tile 的独有信息
     */
    public class TilePoint {
        public TilePoint(string key, Vector2 point, DisplayLayer layer) {
            this.key = key;
            this.point = point;
            this.layer = layer;
        }

        /**
         * 显示模式：
         * 1 背景
         * 2 碰撞
         * 3 前景
         */
        public DisplayLayer layer { get; }

        /**
         * 用来索引真正存储
         */
        public string key { get; }
        
        /**
         * 当前 Tile 的位置
         */
        public Vector2 point { get; }
    }
}