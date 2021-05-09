using Newtonsoft.Json;

namespace AlsRitter.GenerateMap.CustomTileFrame.MapDataEntity.V1.Dto
{
    public class Background
    {
        /// <summary>
        /// 背景图片的 id
        /// </summary>
        [JsonProperty("bg_id")]
        public string BgId { get;set; }

        /// <summary>
        /// 颜色
        /// </summary>
        [JsonProperty("color")]
        public string Color { get;set; }
    }
}
