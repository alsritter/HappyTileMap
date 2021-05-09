﻿using System;

namespace AlsRitter.GenerateMap.CustomTileFrame
{
    /// <summary>
    /// 这个主要标识当前处于哪个图层中
    /// </summary>
    public enum DisplayModel
    {
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
    [Flags]
    public enum TileTag
    {
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
}