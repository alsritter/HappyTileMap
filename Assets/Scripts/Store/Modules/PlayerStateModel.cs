using System;
using UnityEngine;

namespace AlsRitter.Store.Model {
    /// <summary>
    /// 状态枚举
    /// </summary>
    public enum PlayState {
        Normal,
        Jump,
        Climb,
        Dash,
        Fall,
    }

    /// <summary>
    /// 玩家现在的方向
    /// </summary>
    public enum PlayDir {
        Right,
        Left,
    }

    /**
     * 角色的 “状态” 资源数据：是否在移动，当前方向等待
     */
    public class PlayerStateModel : MonoBehaviour {
        #region 角色动画控制

        [Header("动画控制器参数")]
        public bool isGround; //是否在地面  true在地面 false不在地面
        public bool isAlive;  // 玩家是否存活
        public bool isClimb; // 爬墙状态
        public bool jumpState; // 跳跃状态
        [Header("暗影冲刺持续时间")]
        public float dashTime;
        [Header("是否能够使用暗影冲刺")]
        public bool isCanDash;
        [HideInInspector]
        public int introDir; //横向位移减速时的方向

        #endregion


        #region 角色控制器

        [Header("当前方向")]
        public PlayDir playDir; //角色面朝方向
        [HideInInspector]
        public PlayDir   lastDir; // 上一帧的方向
        public PlayState playState;
        public Vector2   dashDir;
        public bool      isMove = true; //是否允许左右移动
        public bool      isIntroJump; //是否是刚进入跳跃的状态
        public bool      isCanControl = true; //是否允许控制

        #endregion
    }
}