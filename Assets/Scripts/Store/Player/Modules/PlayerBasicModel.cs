using System;
using UnityEngine;

namespace AlsRitter.Global.Store.Player.Model {
    /**
     * 角色的基本数据：生命值，速度等参数
     * 角色的基本组件等
     * 角色的通用参数
     */
    public class PlayerBasicModel : MonoBehaviour {
        /**
         * 用于射线检查
         */
        [Header("射线检查距离")]
        public float upCheckDistance = 0.05f;
        public float downCheckDistance  = 0.05f;
        public float horizCheckDistance = 0.1f;

        [Header("当前移动的速度")]
        public Vector3 moveSpeed;

        [HideInInspector]
        public float currentSpeed; // 当前移动速度，主要用于不同状态时切换速度
        [HideInInspector]
        public float moveHSpeed; //横向位移减速时的速度
        [HideInInspector]
        public int introDir; //横向位移减速时的方向

        [Header("移动参数")]
        public float speed = 3.5f; //速度
        public float runSpeed    = 7.5f;
        public float crouchSpeed = 1.5f; // 下蹲走路


        [Header("跳跃参数")]
        public float jumpMax = 16f; //跳跃的最大高度
        public float jumpMin   = 8f; //跳跃的最小高度
        public float jumpSpeed = 5f;

        // [Tooltip("跳跃的基础力")]
        // public float jumpForce = 7f;
        // [Tooltip("二段跳的除数")]
        // public float jump2ForceDivisor = 3f;
        // [Tooltip("蹬墙跳给的推力")]
        // public float climbLateralForce = 10f;

        [HideInInspector]
        public float startJumpPos; //开始跳跃时的位置
        [HideInInspector]
        public int coyotetimeFram; // 土狼时间
        [HideInInspector]
        public Rigidbody2D rb;
        [HideInInspector]
        public bool fixHorizon; // 是否横向位置修正

        private void Awake() {
            rb = GetComponent<Rigidbody2D>();
        }
    }
}