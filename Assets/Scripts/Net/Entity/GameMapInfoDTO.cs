using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AlsRitter.Net.Entity
{
    [Serializable]
    public class GameMapInfoDTO
    {
        [JsonProperty("id")]
        private string id; // 地图编号

        [JsonProperty("author")]
        private string author; // 作者名称

        [JsonProperty("introduction")]
        private string introduction; // 地图介绍

        [JsonProperty("cover_path")]
        private string coverPath; // 地图封面

        [JsonProperty("down_count")]
        private int downCount; // 下载次数

        [JsonProperty("win_count")]
        private int winCount; // 通过次数

        [JsonProperty("sum_count")]
        private int sumCount; // 游玩总次数

        [JsonProperty("down_path")]
        private string downPath; // 地图下载地址

        [JsonProperty("version")]
        private string version; // 地图版本

        [JsonProperty("grade")]
        private int grade; // 地图评分
    }

}