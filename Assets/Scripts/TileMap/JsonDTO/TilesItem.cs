using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonTileEnum;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MapDataEntity.Dto
{
    public class TilesItem
    {
        /// <summary>
        /// 显示模式：
        /// 1 背景
        /// 2 碰撞
        /// 3 前景
        /// </summary>
        [JsonProperty("display_model")]
        [JsonConverter(typeof(StringEnumConverter))] // 表示将枚举值转换成对应的字符串
        public DisplayModel DisplayModel { get; set; }

        /// <summary>
        /// 图片的编号
        /// </summary>
        [JsonProperty("tile_image_id")]
        public string TileImageId { get; set; }

        /// <summary>
        /// 16 进制颜色码
        /// </summary>
        [JsonProperty("color")]
        public string Color { get; set; }

        /// <summary>
        /// 效果的编号
        /// </summary>
        [JsonProperty("effect_keys")]
        public string[] EffectKeys { get; set; }

        /// <summary>
        /// tags 的编号
        /// </summary>
        [JsonProperty("tags")]
        public TileTag[] Tags { get; set; }
    }
}