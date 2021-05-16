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
        [JsonProperty("id")] public string id { get; set; } // 地图编号

        [JsonProperty("author")] public string author { get; set; } // 作者名称

        [JsonProperty("introduction")] public string introduction { get; set; } // 地图介绍

        [JsonProperty("cover_path")] public string coverPath { get; set; } // 地图封面

        [JsonProperty("down_count")] public int downCount { get; set; } // 下载次数

        [JsonProperty("pass_count")] public int passCount { get; set; } // 通过次数

        [JsonProperty("sum_count")] public int sumCount { get; set; } // 游玩总次数

        [JsonProperty("version")] public string version { get; set; } // 地图版本

        [JsonProperty("grade")] public int grade { get; set; } // 地图评分
    }
}