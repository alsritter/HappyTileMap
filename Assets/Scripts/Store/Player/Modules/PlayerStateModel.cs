using System;
using UnityEngine;

namespace AlsRitter.Global.Store.Player.Model {
    /// <summary>
    /// 玩家现在的方向
    /// </summary>
    public enum PlayDir {
        Right,
        Left,
    }

    /// <summary>
    /// 状态枚举
    /// </summary>
    public enum PlayState {
        Normal,
        Jump,
        Fall,
        Run
    }

    /**
     * 角色的 “状态” 资源数据：是否在移动，当前方向等待
     */
    public class PlayerStateModel : MonoBehaviour {
        [Header("当前方向")]
        public PlayDir playDir; //角色面朝方向
        [HideInInspector]
        public PlayDir lastDir; // 上一帧的方向

        [Header("角色当前状态")]
        public PlayState playState;

        public bool isMove = true; //是否允许左右移动
        public bool isIntroJump; //是否是刚进入跳跃的状态

        public bool isGround; //是否在地面上
        public bool isStand; //是否站着
        public bool isHalfFoot; //半只脚着地
        public bool isExistTop; // 判断头顶是否有墙
        public bool isDie;  //死亡
    }
}