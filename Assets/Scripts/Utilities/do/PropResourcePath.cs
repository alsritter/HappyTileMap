using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlsRitter.Utilities.Do
{
    /// <summary>
    /// 用于存储预制件的信息
    /// </summary>
    public class PropResourcePath
    {
        public enum PropType
        {
            /// <summary>
            /// 装饰品
            /// </summary>
            Bauble = 0,
            /// <summary>
            /// 可交互的道具
            /// </summary>
            Props = 1,
            /// <summary>
            /// 陷阱
            /// </summary>
            Trap = 2
        }

        public PropResourcePath(string prefabId, int width, int high, PropType type, string path)
        {
            this.prefabId = prefabId;
            this.width = width;
            this.high = high;
            this.type = type;
            this.path = path;
        }
        
        public string prefabId { get; }
        public int width { get; }
        public int high { get; }
        public PropType type { get; }
        public string path { get; }
    }
}