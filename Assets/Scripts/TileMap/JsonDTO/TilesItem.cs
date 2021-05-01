using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MapDataEntity.Dto
{
    public enum DisplayModel
    {
        /// <summary>
        /// 背景
        /// </summary>
        Background = 1,
        /// <summary>
        /// 碰撞
        /// </summary>
        Crash = 2,
        /// <summary>
        /// 前景
        /// </summary>
        Foreground = 3
    }

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
        /// 
        /// </summary>
        [JsonProperty("tile_image_id")]
        public string TileImageId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("color")]
        public string Color { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("effect_keys")]
        public string[] EffectKeys { get; set; }
    }
}