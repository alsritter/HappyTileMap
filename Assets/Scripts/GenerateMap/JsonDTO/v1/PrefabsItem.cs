using Newtonsoft.Json;

namespace AlsRitter.GenerateMap.CustomTileFrame.MapDataEntity.V1.Dto
{
    public class PrefabsItem
    {
        /// <summary>
        /// 预制件的编号
        /// </summary>
        [JsonProperty("prefab_id")]
        public string PrefabId { get;set; }

        /// <summary>
        /// 位置（这个只是在 Grid 中的位置，需要转成世界坐标）
        /// </summary>
        [JsonProperty("x")]
        public int X { get;set; }

        /// <summary>
        /// 位置
        /// </summary>
        [JsonProperty("y")]
        public int Y { get;set; }
    }
}
