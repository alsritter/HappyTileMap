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
        public string username { get; set; }

        [JsonProperty("token")]
        public string token  { get; set; }

        [JsonProperty("win_count")]
        public int winCount { get; set; } // 用户胜利次数

        [JsonProperty("sum_count")]
        public int sumCount { get; set; } // 用户总场次

        [JsonProperty("death_count")]
        public int deathCount { get; set; } // 死亡次数

        public override string ToString()
        {
            return $"username: {username}, winCount: {winCount}, sumCount: {sumCount}, deathCount:{deathCount}";
        }
    }
}