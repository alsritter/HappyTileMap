using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AlsRitter.Net.Entity
{
    [Serializable]
    public class GameDeathInfoDTO
    {
        [JsonProperty("x")] public float x { get; set; }

        [JsonProperty("y")] public float y { get; set; }

        public GameDeathInfoDTO()
        {
        }

        /// <summary>
        /// 这里创建两个构造方法
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public GameDeathInfoDTO(float x, float y)
        {
            this.y = y;
            this.x = x;
        }
    }
}