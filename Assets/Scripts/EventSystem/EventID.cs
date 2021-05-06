using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventFrame
{
    /// <summary>
    /// 定义事件 id
    ///
    /// 这里可以定义一个事件
    /// </summary>
    public enum EventID
    {
        /// <summary>
        /// 捡到收集品时的事件
        /// </summary>
        Scores = 10001,

        #region 角色状态

        // 下面这部分用于角色状态，即 PlayerStateEventData 类
        /// <summary>
        /// 跑步事件
        /// </summary>
        Run = 10002,
        /// <summary>
        /// 在地面的事件
        /// </summary>
        OnGround = 10003,
        /// <summary>
        /// 抓住墙的事件
        /// </summary>
        GraspWall = 10004,
        /// <summary>
        /// 头顶撞墙事件
        /// </summary>
        OnWallTap = 10005,
        /// <summary>
        /// 在空中的事件
        /// </summary>
        InTheAir = 10006,
        /// <summary>
        /// 在下蹲的事件
        /// </summary>
        IsCrouching = 10007,

        #endregion

        /// <summary>
        /// 受伤
        /// </summary>
        Harm = 10008
    }
}