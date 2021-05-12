using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlsRitter.EventFrame
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
        Run,
        /// <summary>
        /// 在地面的事件
        /// </summary>
        OnGround,
        /// <summary>
        /// 抓住墙的事件
        /// </summary>
        GraspWall,
        /// <summary>
        /// 头撞墙事件
        /// </summary>
        OnHeadWall,
        /// <summary>
        /// 头顶撞到墙了
        /// </summary>
        OnTopWall,
        /// <summary>
        /// 在空中的事件
        /// </summary>
        InTheAir,
        /// <summary>
        /// 在下蹲的事件
        /// </summary>
        IsCrouching,

        #endregion

        /// <summary>
        /// 这个受伤时
        /// </summary>
        Harm,
        /// <summary>
        /// 游戏胜利
        /// </summary>
        Win
    }
}