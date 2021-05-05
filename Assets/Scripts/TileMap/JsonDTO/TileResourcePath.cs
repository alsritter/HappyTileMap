using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{

    /// <summary>
    /// 封装资源路径
    /// </summary>
    public class TileResourcePath
    {
        public enum SpriteMode
        {
            Single = 0,
            Multiple = 1
        }

        public TileResourcePath(string spriteId, string path, SpriteMode mode)
        {
            this.spriteId = spriteId;
            this.path = path;
            this.mode = mode;
        }

        public string spriteId { get; }
        public string path { get; }
        public SpriteMode mode { get; }
    }
}
