using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MapDataEntity.Dto
{
    public class LayerItem
    {
        /// <summary>
        /// 这个 id 只是前端绘图时的图层排序
        /// </summary>
        [JsonProperty("id")]
        public long Id { get;set; }

        /// <summary>
        /// 图层名称
        /// </summary>
        [JsonProperty("name")]
        public string Name { get;set; }

        /// <summary>
        /// 是否显示该图层
        /// </summary>
        [JsonProperty("show")]
        public bool Show { get;set; }

        /// <summary>
        /// 地图块
        /// </summary>
        [JsonProperty("chunks")]
        public List<ChunksItem> Chunks { get;set; }
    }
}
