using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MapDataEntity.Dto
{
    public class ChunksItem
    {
        /// <summary>
        /// 当前块的位置
        /// </summary>
        [JsonProperty("x")]
        public long X { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("y")]
        public long Y { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("width")]
        public long Width { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("height")]
        public long Height { get; set; }

        /// <summary>
        /// 二维数组
        /// </summary>
        [JsonProperty("data")]
        public long[][] Data { get; set; }
    }
}