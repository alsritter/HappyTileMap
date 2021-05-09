using System;

namespace AlsRitter.GenerateMap.CustomTileFrame.TileEffect
{
    // 只能用在类上面，用于读取该类的用途和版本号之类的信息
    [AttributeUsage(AttributeTargets.Class)]
    public class EffectInfoAttribute : Attribute
    {
        public readonly int version;
        public readonly string effect;
        public readonly string author;

        /// <summary>
        /// 只能用在类上面，用于读取该类的用途和版本号之类的信息
        /// </summary>
        /// <param name="version">版本号</param>
        /// <param name="effect">效果</param>
        /// <param name="author">创建者</param>
        public EffectInfoAttribute(string effect, int version, string author)
        {
            this.version = version;
            this.author = author;
            this.effect = effect;
        }
    }
}

namespace EffectDocumentTools
{
    // 创建专门用于生成文档的 Attribute
}