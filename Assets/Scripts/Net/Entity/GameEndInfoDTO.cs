using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AlsRitter.Net.Entity
{
    [Serializable]
    public class GameEndInfoDTO
    {
        /// <summary>
        /// 分数
        /// </summary>
        [JsonProperty("score")] 
        public int score { get; set; }

        /// <summary>
        /// 游戏花费的时间
        /// </summary>
        [JsonProperty("time")] 
        public float time { get; set; }

        /// <summary>
        /// 剩余 HP
        /// </summary>
        [JsonProperty("hp")]
        public int hp { get; set; }

        /// <summary>
        /// 游戏是否是胜利结束的？
        /// </summary>
        [JsonProperty("win")]
        public bool win { get; set; }

        public GameEndInfoDTO()
        {
        }

        /// <summary>
        /// 这里创建两个构造方法
        /// </summary>
        /// <param name="score"></param>
        /// <param name="time"></param>
        /// <param name="hp"></param>
        /// <param name="win"></param>
        public GameEndInfoDTO(int score, float time, int hp, bool win)
        {
            this.score = score;
            this.time = time;
            this.hp = hp;
            this.win = win;
        }
    }
}