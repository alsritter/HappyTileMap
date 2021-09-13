using System;
using System.Collections.Generic;

namespace AlsRitter.GenerateMap.CustomTileFrame {
    /// <summary>
    /// 这个主要标识当前处于哪个图层中
    /// </summary>
    public enum DisplayLayer {
        /// <summary>
        /// 背景
        /// </summary>
        Background = 1,
        /// <summary>
        /// 碰撞
        /// </summary>
        Crash = 2,
        /// <summary>
        /// 前景
        /// </summary>
        Foreground = 3
    }

    /// <summary>
    /// 给砖块赋特殊的标签
    /// </summary>
    public enum TileTag {
        /// <summary>
        /// 墙 1
        /// </summary>
        Wall = 1,
        /// <summary>
        /// 楼梯  2
        /// </summary>
        Ladder = 1 << 1,
        /// <summary>
        /// 破碎块 4
        /// </summary>
        Broken = 1 << 2
    }

    public static class NumberConvertEnumTool {

        /**
         * 数字转枚举对象
         */
        public static DisplayLayer NumberToLayer(int layerNumber) {
            switch (layerNumber) {
                case 1:
                    return DisplayLayer.Background;
                case 2:
                    return DisplayLayer.Crash;
                case 3:
                    return DisplayLayer.Foreground;
                default:
                    return DisplayLayer.Crash;
            }
        }

        public static TileTag NumberToTag(int tagNumber) {
            switch (tagNumber) {
                case 1:
                    return TileTag.Wall;
                case 2:
                    return TileTag.Ladder;
                case 4:
                    return TileTag.Broken;
                default:
                    return TileTag.Wall;
            }
        }

        public static List<TileTag> NumbersToTags(List<int> tagNumbers) {
            var result = new List<TileTag>();
            tagNumbers.ForEach(x => {
                result.Add(NumberToTag(x));
            });

            return result;
        }
    }
}