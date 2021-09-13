using System;
using System.Collections.Generic;

namespace AlsRitter.GenerateMap.Interface.Do
{
    /**
     * 游戏地图数据就存这个对象，至于如何读取 JSON 生成这个对象，自己去实现
     */
    public class GameMap
    {
        public GameMap(DateTime createTime, string version, string author, Initial initial, Background background,
            List<PrefabsItem> prefabs, List<TileData> tilesData, List<TilePoint> tiles)
        {
            this.createTime = createTime;
            this.version = version;
            this.author = author;
            this.initial = initial;
            this.background = background;
            this.prefabs = prefabs;
            this.tilesData = tilesData;
            this.tiles = tiles;
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime createTime { get; }

        /// <summary>
        /// 地图版本
        /// </summary>
        public string version { get; }

        /// <summary>
        /// 创建该地图的作者
        /// </summary>
        public string author { get; }

        /// <summary>
        /// 角色起始状态
        /// </summary>
        public Initial initial { get; }

        /// <summary>
        /// 背景图片
        /// </summary>
        public Background background { get; }

        public List<PrefabsItem> prefabs { get; }

        /**
         * Tile 的通用数据
         */
        public List<TileData> tilesData { get; }

        public List<TilePoint> tiles { get; }
    }
}