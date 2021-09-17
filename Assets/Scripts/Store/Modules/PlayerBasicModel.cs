using System;
using UnityEngine;

namespace AlsRitter.Store.Model {
    /**
     * 角色的基本数据：生命值，速度等参数
     * 角色的基本组件等
     * 角色的通用参数
     */
    public class PlayerBasicModel : MonoBehaviour {
        [HideInInspector]
        public Rigidbody2D rb;

        #region 玩家控制器

        [Header("当前速度")]
        public Vector3 velocity;
        [Header("冲刺速度")]
        [Range(0.01f, 1f)]
        public float dashSpeed = 0.5f;
        
        [HideInInspector]
        public float moveSpeed = 3.5f; //速度
        [HideInInspector]
        public int coyotetimeFram = 0; // 土狼时间：当游戏开发者在重力作用起作用前给予玩家离开悬崖边缘的时间
        [HideInInspector]
        public float moveH; //横向位移减速时的速度
        [HideInInspector]
        public int introDir; //横向位移减速时的方向


        [Header("距离地面的最小高度")]
        public float distance; //距离地面的高度

        //爬墙耐力相关
        [HideInInspector]
        public float curStamina;
        [HideInInspector]
        public float climbMaxStamina = 110;
        [HideInInspector]
        public float climbUpCost = 100 / 2.2f;
        [HideInInspector]
        public float climbStillCost = 100 / 10f;
        [HideInInspector]
        public float climbJumpCost = 110 / 4f;
        [HideInInspector]
        public bool fixHorizon; // 检测并修正水平方向的位移
        [HideInInspector]
        public int playerLayerMask; //玩家层级，射线检测时忽略玩家自身

        [HideInInspector]
        public float startJumpPos; //开始跳跃时的位置

        [Header("Jump")]
        public float jumpMax; //跳跃的最大高度
        public float jumpMin; //跳跃的最小高度
        public float jumpSpeed;
        public float climbSpeed;

        /**
         * 用于射线检查
         */
        [Header("射线检查距离")]
        public float upCheckDistance = 0.05f;
        public float downCheckDistance  = 0.05f;
        public float leftCheckDistance  = 0.1f;
        public float rightCheckDistance = 0.1f;


        //玩家中心点
        public Vector2 Position => transform.position - centerPos;


        /**
         * 冲撞次数
         */
        public int dashCount = 0;

        [Header("碰撞盒大小")]
        public Vector2 boxSize;
        [Header("中心点位置")]
        public Vector3 centerPos;

        #endregion

        private void Awake() {
            rb = GetComponent<Rigidbody2D>();
        }

        private void Start() {
            playerLayerMask = LayerMask.GetMask("Player");
            playerLayerMask = ~playerLayerMask; //获得当前玩家层级的 mask 值，让射线忽略玩家层检测
            //设置盒子射线的大小
            boxSize = new Vector2(0.5f, 1.55f);
            centerPos = new Vector3(0, 0.21f);
        }
    }
}