using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace AlsRitter.GenerateMap.CustomTileFrame.MapDataEntity.V1.Dto
{
    public class MapRootDto
    {
        /// <summary>
        /// 创建时间
        /// </summary>
        [JsonProperty("create_time")]
        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTime CreateTime { get;set; }

        /// <summary>
        /// 地图版本
        /// </summary>
        [JsonProperty("version")]
        public string Version { get;set; }

        /// <summary>
        /// 创建该地图的作者
        /// </summary>
        [JsonProperty("author")]
        public string Author { get;set; }

        /// <summary>
        /// 地图是否是无限模式（未来推出固定大小的模式）
        /// </summary>
        [JsonProperty("infinite")]
        public bool Infinite { get;set; }

        /// <summary>
        /// 每个 Chunk 块的宽度
        /// </summary>
        [JsonProperty("chunk_size_width")]
        public long ChunkSizeWidth { get;set; }

        /// <summary>
        /// 每个 Chunk 块的高度
        /// </summary>
        [JsonProperty("chunk_size_height")]
        public long ChunkSizeHeight { get;set; }

        /// <summary>
        /// 角色起始状态
        /// </summary>
        [JsonProperty("initial")]
        public Initial Initial { get;set; }

        /// <summary>
        /// 背景图片
        /// </summary>
        [JsonProperty("background")]
        public Background Background { get;set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("prefabs")]
        public List<PrefabsItem> Prefabs { get;set; }

        /// <summary>
        /// 索引的格子
        /// </summary>
        [JsonProperty("tiles")]
        public List<TilesItem> Tiles { get;set; }

        /// <summary>
        /// 图层
        /// </summary>
        [JsonProperty("layer")]
        public List<LayerItem> Layer { get;set; }
    }
}