using Newtonsoft.Json;

namespace AlsRitter.GenerateMap.Interface.Do
{
    public class Background
    {
        public Background(string bgId, string color)
        {
            BgId = bgId;
            Color = color;
        }

        /// <summary>
        /// 背景图片的 id
        /// </summary>
        public string BgId { get;}

        /// <summary>
        /// 颜色
        /// </summary>
        public string Color { get;}
    }
}
