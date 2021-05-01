using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 注意，状态机不能直接读取下面的这些 Public 修饰的状态，
/// 它们是用于在编辑面板配置的，状态机应该读取 PlayerFSMGlobalVariable 里面的参数
/// </summary>
public class PlayerFSMSystem : MonoBehaviour
{
    [HideInInspector] public PlayerBaseState curState { get; private set; } // 指向当前的状态

    [HideInInspector]
    public Rigidbody2D rb;
    [HideInInspector]
    public BoxCollider2D coll;
    [HideInInspector]
    public GameObject rightFoot;
    [HideInInspector]
    public GameObject leftFoot;
    [HideInInspector]
    public GameObject hand;

    [Header("移动参数")]
    public float speed = 8f;
    public float runDivisor = 3f;
    public float jumpSleepDivisor = 3f;
    public float climbSpeed = 2f;
    public float crouchSpeedDivisor = 3f; // 下蹲走路时的除数

    // 用于解决卡墙的问题
    // 参考资料：Unity 横版2D移动跳跃问题——关于一段跳与二段跳 https://www.cnblogs.com/AMzz/p/11802502.html
    [Header("物理")] 
    public PhysicsMaterial2D hasFriction;  //有摩擦力的
    public PhysicsMaterial2D noFriction;  //无摩擦力的

    [Header("跳跃参数")]
    [Tooltip("跳跃的基础力")]
    public float jumpForce = 7f;
    [Tooltip("二段跳的除数")]
    public float jump2ForceDivisor = 3f;
    [Tooltip("蹬墙跳给的推力")]
    public float climbLateralForce = 10f;

    [Header("当前需要检查的Layer")]
    public LayerMask groundLayer; // 当前需要检查的“地面”的 Layer
    public LayerMask ladderLayer; // 当前需要检查的“楼梯”的 Layer


    // 这里只存根状态
    public PlayerBaseState onGroundState; // 地面的状态
    public PlayerBaseState inTheAirState; // 空中的状态
    public PlayerBaseState inClimbState; // 挂墙状态

    // 临时变量
    [HideInInspector]
    public float xVelocity; // 临时存储移动速度，可以用于判断方向
    public float yVelocity;

    private float footDistance; // 脚距离中心点的距离
    private float handDistance; // 脚距离中心点的距离
    private Vector2 handDirection;

    [Header("Debug 相关 检查状态用")]
    public bool isCrouching; // 在下蹲
    public bool isRun;
    public bool isOnGround; // 是否在地面
    public bool graspWall; // 是否抓住了墙
    public bool isOnWallTap; // 用于判断当前是否位于墙顶端
    public bool inTheAir;

    //[Header("按键相关")]


    [Header("Debug 相关 查看状态")]
    public bool showState = false;

    private void Awake()
    {
        onGroundState = new OnGroundState();
        inTheAirState = new InTheAirState();
        inClimbState = new InClimbState();
    }


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<BoxCollider2D>();
        rightFoot = GameObject.FindGameObjectWithTag("rightFoot");
        leftFoot = GameObject.FindGameObjectWithTag("leftFoot");
        hand = GameObject.FindGameObjectWithTag("hand");


        footDistance = Mathf.Abs(leftFoot.transform.position.y - coll.transform.TransformPoint(coll.offset).y);
        handDistance = Mathf.Abs(hand.transform.position.x - coll.transform.TransformPoint(coll.offset).x);

        handDirection = gameObject.transform.localScale.x > 0 ? Vector2.right : Vector2.left;

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
        PhysicsCheck(); // 射线检查
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
            gameObject.transform.localScale = new Vector3(-1, 1, 1);
            handDirection = Vector2.left;
        }

        if (xVelocity > 0.01)
        {
            gameObject.transform.localScale = new Vector3(1, 1, 1);
            handDirection = Vector2.right;
        }
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


    /// <summary>
    /// 射线检查
    /// </summary>
    private void PhysicsCheck()
    {
        Vector2 leftPos = leftFoot.transform.position;
        Vector2 rightPos = rightFoot.transform.position;
        Vector2 handPos = hand.transform.position;
        Vector2 blockPos = handPos - new Vector2(0, coll.offset.y);

        // 头顶的射线，用于判断是否刚好够到岩壁（头顶不应该被遮住）
        RaycastHit2D blockedCheck = Physics2D.Raycast(blockPos, handDirection, -handDistance, ladderLayer);


        RaycastHit2D leftCheck =
            Physics2D.Raycast(leftPos, Vector2.up, footDistance, groundLayer);

        RaycastHit2D rightCheck =
            Physics2D.Raycast(rightPos, Vector2.up, footDistance, groundLayer);

        RaycastHit2D footBottom = Physics2D.Raycast(leftPos, handDirection,  Mathf.Abs(leftPos.x - rightPos.x), groundLayer);
        
        RaycastHit2D handCheck =
            Physics2D.Raycast(handPos, handDirection, -handDistance, ladderLayer);

        Debug.DrawRay(leftPos, handDirection * Mathf.Abs(leftPos.x - rightPos.x), footBottom ? Color.red : Color.green);
        Debug.DrawRay(blockPos, -handDirection * handDistance, blockedCheck ? Color.red : Color.green);
        Debug.DrawRay(handPos, -handDirection * handDistance, handCheck ? Color.red : Color.green);
        Debug.DrawRay(leftPos, Vector2.up * footDistance, leftCheck ? Color.red : Color.green);
        Debug.DrawRay(rightPos, Vector2.up * footDistance,
            rightCheck ? Color.red : Color.green);

        graspWall = handCheck;
        isOnWallTap = !blockedCheck;

        // 判断当前是否在地面（这里使用了隐式转换，RaycastHit2D 转成 bool类型 标识它是否被击中）
        isOnGround = leftCheck || rightCheck || footBottom;
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
}