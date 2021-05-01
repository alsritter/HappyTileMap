using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEngine;

namespace MapDataEntity.Dto
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
        /// 出生点 X
        /// </summary>
        [JsonProperty("initial_position_x")]
        public long InitialPositionX { get;set; }

        /// <summary>
        /// 出生点 Y
        /// </summary>
        [JsonProperty("initial_position_y")]
        public long InitialPositionY { get;set; }

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