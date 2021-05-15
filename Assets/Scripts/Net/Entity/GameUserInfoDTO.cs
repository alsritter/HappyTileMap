using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AlsRitter.Net.Entity
{
    [Serializable]
    public class GameUserInfoDTO
    {
        [JsonProperty("username")]
        private string username;

        [JsonProperty("win_count")]
        private int winCount; // 用户胜利次数

        [JsonProperty("sum_count")]
        private int sumCount; // 用户总场次

        [JsonProperty("death_count")]
        private int deathCount; // 死亡次数
    }
}