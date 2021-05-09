using System;
using System.Collections;
using System.Collections.Generic;
using AlsRitter.EventFrame;
using AlsRitter.EventFrame.CustomEvent;
using UnityEngine;


namespace AlsRitter.PlayerController.FSM
{
    /// <summary>
    /// 注意，状态机不能直接读取下面的这些 Public 修饰的状态，
    /// 它们是用于在编辑面板配置的，状态机应该读取 PlayerFSMGlobalVariable 里面的参数
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
    public class PlayerFSMSystem : MonoBehaviour, IEventObserver
    {
        [HideInInspector] 
        private PlayerBaseState curState { get;  set; } // 指向当前的状态

        [HideInInspector]
        public Rigidbody2D rb;
        [HideInInspector]
        public BoxCollider2D coll;

        [Header("移动参数")]
        public float speed = 8f;
        public float runDivisor = 3f;
        public float jumpSleepDivisor = 3f;
        public float climbSpeed = 2f;
        public float crouchSpeedDivisor = 3f; // 下蹲走路时的除数


        [Header("跳跃参数")]
        [Tooltip("跳跃的基础力")]
        public float jumpForce = 7f;
        [Tooltip("二段跳的除数")]
        public float jump2ForceDivisor = 3f;
        [Tooltip("蹬墙跳给的推力")]
        public float climbLateralForce = 10f;

        // 用于解决卡墙的问题
        // 参考资料：Unity 横版2D移动跳跃问题——关于一段跳与二段跳 https://www.cnblogs.com/AMzz/p/11802502.html
        [Header("物理")]
        public PhysicsMaterial2D hasFriction; //有摩擦力的
        public PhysicsMaterial2D noFriction; //无摩擦力的

        // 这里只存根状态
        public PlayerBaseState onGroundState; // 地面的状态
        public PlayerBaseState inTheAirState; // 空中的状态
        public PlayerBaseState inClimbState; // 挂墙状态

        // 临时变量
        [HideInInspector]
        public float xVelocity; // 临时存储移动速度，可以用于判断方向
        [HideInInspector]
        public float yVelocity;

        // 手的方向
        [HideInInspector]
        public Vector2 handDirection;

        [HideInInspector]
        public GameObject rightFoot;
        [HideInInspector]
        public GameObject leftFoot;
        [HideInInspector]
        public GameObject hand;
        [HideInInspector]
        public GameObject head;

        private bool isOnGround;
        private bool graspWall; // 是否抓住了墙

        [Header("Debug 相关 查看状态")]
        public bool showState = false;

        private void Awake()
        {
            onGroundState = new OnGroundState();
            inTheAirState = new InTheAirState();
            inClimbState = new InClimbState();
            // 把自己注册进事件中心
            EventManager.Register(this, EventID.GraspWall, EventID.OnGround);
        }


        private void Start()
        {
            rightFoot = GameObject.FindGameObjectWithTag("rightFoot");
            leftFoot = GameObject.FindGameObjectWithTag("leftFoot");
            hand = GameObject.FindGameObjectWithTag("hand");
            head = GameObject.FindGameObjectWithTag("head");

            rb = GetComponent<Rigidbody2D>();
            coll = GetComponent<BoxCollider2D>();

            // 最开始的状态
            curState = onGroundState;
        }

        private void Update()
        {
            xVelocity = Input.GetAxis("Horizontal");
            yVelocity = Input.GetAxis("Vertical");

            CheckCurrentState();
            curState.Update(this);
        }

        private void FixedUpdate()
        {
            FlipDirection();
            curState.FixedUpdate(this);
            if (showState) Debug.Log(curState.GetCurrentStateName(curState)); // 打印当前的状态
        }


        /// <summary>
        /// 判断当前角色的方向翻转
        /// </summary>
        private void FlipDirection()
        {
            // 这里需要使用 Vector3 否则 z 轴可能被置为 0
            if (xVelocity < -0.01)
            {
                gameObject.transform.localScale = new Vector2(-1, 1);
                handDirection = Vector2.left;
            }

            if (!(xVelocity > 0.01)) return;
            gameObject.transform.localScale = new Vector2(1, 1);
            handDirection = Vector2.right;
        }


        /// <summary>
        /// 用来检查当前状态，并切换到对应的状态
        /// </summary>
        private void CheckCurrentState()
        {
            if (graspWall)
            {
                SetRootCurrentState(inClimbState, true, true);
                return;
            }

            if (!isOnGround)
            {
                SetRootCurrentState(inTheAirState, true, true);
            }
            else
            {
                // 在地面才能跳跃
                if (Input.GetButtonDown("Jump"))
                {
                    SetRootCurrentState(inTheAirState, true, true);
                    return;
                }

                // 当前没有向上的力才能切换到在地面的状态
                if (rb.velocity.y < 0.01)
                {
                    SetRootCurrentState(onGroundState, true, true);
                }
            }
        }


        private void SetRootCurrentState(PlayerBaseState state)
        {
            if (this.curState == state) return; // 避免重复转换状态
            // Debug.Log(this.curState.name + "==>" + state.name);
            this.curState = state;
        }

        /// <summary>
        /// 上面方法的重置
        /// </summary>
        /// <param name="state"></param>
        /// <param name="executeEnter"></param>
        /// <param name="executeExit"></param>
        private void SetRootCurrentState(PlayerBaseState state, bool executeEnter, bool executeExit)
        {
            if (this.curState == state) return;

            if (executeExit) curState?.Exit(this);
            curState = state;
            if (executeEnter) curState?.Enter(this); // 如果为空则不执行
        }

        public void HandleEvent(EventData resp)
        {
            switch (resp.eid)
            {
                case EventID.GraspWall:
                    graspWall = ((PlayerStateEventData) resp).trigger;
                    break;
                case EventID.OnGround:
                    isOnGround = ((PlayerStateEventData) resp).trigger;
                    break;
            }
        }
    }
}