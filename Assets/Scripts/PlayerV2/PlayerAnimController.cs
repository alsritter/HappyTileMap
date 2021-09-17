using System;
using System.Collections;
using AlsRitter.GlobalControl.Store;
using AlsRitter.Store.Model;
using UnityEngine;

namespace AlsRitter.Player.System.FSM {
    /**
     * 主要用于控制动画
     */
    public class PlayerAnimController : MonoBehaviour {
        private PlayerInputModel input;
        private PlayerBasicModel basic;
        private PlayerStateModel state;
        private PlayerViewModel  view;

        private Animator playAnimator;

        [Header("距离手的长度")]
        public float xDistance = 0.25f;
        [Header("距离地面的高度")]
        public float yDistance = 0.25f; //距离地面的高度

        public Vector3 moveSpeed; //每一帧的移动速度
        public PlayDir nowDir; //现在的玩家的方向

        public bool inputEnable; //接受输入开关  true 游戏接受按键输入  false不接受按键输入

        private int playerLayerMask;

        #region 动画Key

        private int isGroundId;
        private int isJumpId;
        private int isJumpTwoId;
        private int isDownId; // 不在跳跃状态的自由落下
        private int stopTriggerId;
        private int isRotateId;
        private int isRunId;
        private int isSlowUpId; // 进入上跳减速状态，但还在上升
        private int isStopUpId; //进入下落状态
        private int isDashId;
        private int isStopClimbJumpId;
        private int isClimbId;
        private int exitClimbId;
        private int ySpeedId;
        private int xSpeedId;

        #endregion

        private void Awake() {
            input = UseStore.GetStore().inputModel;
            basic = UseStore.GetStore().basicModel;
            state = UseStore.GetStore().stateModel;
            view = UseStore.GetStore().viewModel;

            playAnimator = view.playAnimator;
        }

        private void Start() {
            state.isAlive = true;
            nowDir = PlayDir.Right;

            inputEnable = true;

            state.jumpState = false;
            state.isCanDash = true; //状态初始化

            playerLayerMask = LayerMask.GetMask("Player");
            playerLayerMask = ~playerLayerMask; //获得当前玩家层级的mask值，并使用~运算，让射线忽略玩家层检测

            // 绑定 key
            isGroundId = Animator.StringToHash("IsGround");
            isJumpId = Animator.StringToHash("IsJump");
            isJumpTwoId = Animator.StringToHash("IsJumpTwo");
            isDownId = Animator.StringToHash("IsDown");
            stopTriggerId = Animator.StringToHash("stopTrigger");
            isRotateId = Animator.StringToHash("IsRotate");
            isRunId = Animator.StringToHash("IsRun");
            isSlowUpId = Animator.StringToHash("IsSlowUp");
            isStopUpId = Animator.StringToHash("IsStopUp");
            isDashId = Animator.StringToHash("IsDash");
            isStopClimbJumpId = Animator.StringToHash("IsStopClimbJump");
            isClimbId = Animator.StringToHash("IsClimb");
            exitClimbId = Animator.StringToHash("exitClimb");

            ySpeedId = Animator.StringToHash("ySpeed");
            xSpeedId = Animator.StringToHash("xSpeed");
        }

        private void Update() {
            if (!state.isAlive) {
                return; //  死亡不进行任何操作
            }

            LRMove();
            UpdateAnimState();
            UDMpve();
            Jump();
            DashFunc();
            playAnimator.SetBool(isGroundId, state.isGround);
            CheckNextMove();
        }
        
        private void UDMpve() {
            moveSpeed.y = basic.velocity.y;
        }

        private void OnCollisionStay2D(Collision2D collision) {
            if (collision.contacts != null) {
            }
        }

        /// <summary>
        /// 左右移动
        /// </summary>
        private void LRMove() {
            if (!inputEnable) {
                return;
            }

            moveSpeed.x = basic.velocity.x;

            playAnimator.SetFloat(ySpeedId, basic.velocity.y);
            playAnimator.SetFloat(xSpeedId, basic.velocity.x);
            

            if (!state.isClimb) //爬墙状态不能通过按键转向
            {
                DirToRotate();
            }

            if (moveSpeed.x == 0) //停止按键输入
            {
                playAnimator.SetTrigger(stopTriggerId);
                playAnimator.ResetTrigger(isRotateId);
                playAnimator.SetBool(isRunId, false);
            }
            else {
                playAnimator.ResetTrigger(stopTriggerId);
            }
        }

        /// <summary>
        /// 根据方向进行旋转
        /// </summary>
        private void DirToRotate() {
            if (nowDir == PlayDir.Left && moveSpeed.x > 0) {
                playAnimator.transform.Rotate(0, 180, 0);
                nowDir = PlayDir.Right;
                if (state.isGround) {
                    playAnimator.SetTrigger(isRotateId);
                }
            }
            else if (nowDir == PlayDir.Right && moveSpeed.x < 0) {
                playAnimator.transform.Rotate(0, -180, 0);
                nowDir = PlayDir.Left;
                if (state.isGround) //在地面才播放转向动画
                {
                    playAnimator.SetTrigger(isRotateId);
                }
            }
            else if (nowDir == PlayDir.Right && moveSpeed.x > 0) {
                playAnimator.SetBool(isRunId, true);
            }
            else if (nowDir == PlayDir.Left && moveSpeed.x < 0) {
                playAnimator.SetBool(isRunId, true);
            }
        }

        /// <summary>
        ///根据落地状态更新动画以及玩家的状态信息
        /// </summary>
        private void UpdateAnimState() {
            if (state.isGround) {
                playAnimator.SetBool(isJumpId, false);
                playAnimator.ResetTrigger(isJumpTwoId);
                playAnimator.SetBool(isDownId, false);
                state.jumpState = false;

                if (state.isClimb) {
                    state.isClimb = false;
                }

                state.isCanDash = true;
            }
            else {
                if (!state.jumpState) {
                    playAnimator.SetBool(isDownId, true);
                }
                else {
                    playAnimator.SetBool(isDownId, false);
                }

                if (state.isClimb) {
                    playAnimator.SetTrigger(isClimbId);
                }
            }
        }


        /// <summary>
        /// 跳跃
        /// </summary>
        private void Jump() {
            if (!inputEnable) {
                return;
            }

            if (state.isClimb && input.JumpKeyDown) {
                StartCoroutine(ClimbJumpMove());
                return;
            }

            if (input.JumpKeyDown && state.isGround) {
                state.jumpState = true;
                playAnimator.SetBool(isJumpId, true); //播放一段跳动画
            }

            else if (input.JumpKey && state.jumpState) {
                // moveSpeed.y = basic.velocity.y;
            }

            else if (input.JumpKeyUp) {
                state.jumpState = false;
            }

            //进入上跳减速状态，但还在上升
            if (moveSpeed.y > 0 && moveSpeed.y < 1.5f) {
                playAnimator.SetBool(isSlowUpId, true);
            }
            else {
                playAnimator.SetBool(isSlowUpId, false);
            }

            //进入下落状态
            if (moveSpeed.y < 0) {
                playAnimator.SetBool(isStopUpId, true);
            }
            else {
                playAnimator.SetBool(isStopUpId, false);
            }
        }


        /// <summary>
        /// 爬墙跳后的转向
        /// </summary>
        private void ClimbRotate() {
            if (nowDir == PlayDir.Left) {
                nowDir = PlayDir.Right;
                playAnimator.transform.Rotate(0, 180, 0);
            }
            else {
                nowDir = PlayDir.Left;
                playAnimator.transform.Rotate(0, -180, 0);
            }
        }

        /// <summary>
        /// 墙上跳跃的移动
        /// </summary>
        /// <returns></returns>
        private IEnumerator ClimbJumpMove() {
            inputEnable = false; //此时不接受其余输入
            state.isClimb = false;
            playAnimator.SetTrigger(isStopClimbJumpId);
            playAnimator.ResetTrigger(isClimbId);
            yield return new WaitForSeconds(0.15f);
            inputEnable = true;
        }

        /// <summary>
        /// 冲刺函数
        /// </summary>
        private void DashFunc() {
            if (!inputEnable) {
                return;
            }

            if (state.playState == PlayState.Dash && state.isCanDash) {
                if (state.isClimb) {
                    ClimbRotate(); //如果是爬墙状态冲刺，先转向在进行冲刺
                }
                
                playAnimator.SetTrigger(isDashId); //播放冲刺动画
                state.isCanDash = false;
            }

            if (state.playState != PlayState.Dash) {
                playAnimator.ResetTrigger(isDashId);
            }
        }


        /// <summary>
        /// 检测下一帧的位置是否能够移动
        /// </summary>
        private void CheckNextMove() {
            var dir = 0; //确定下一帧移动的左右方向
            if (moveSpeed.x > 0) {
                dir = 1;
            }
            else if (moveSpeed.x < 0) {
                dir = -1;
            }
            else {
                dir = 0;
            }

            if (dir != 0) //当左右速度有值时
            {
                var lRHit2D =
                    Physics2D.BoxCast(transform.position,
                                      basic.boxSize, 0,
                                      Vector2.right * dir, 5.0f,
                                      playerLayerMask);

                if (lRHit2D.collider != null) //如果当前方向上有碰撞体
                {
                    var tempXValue = (float) Math.Round(lRHit2D.point.x, 1); //取X轴方向的数值，并保留1位小数精度。防止由于精度产生鬼畜行为
                    var colliderPoint = new Vector3(tempXValue, transform.position.y); //重新构建射线的碰撞点
                    var tempDistance = Vector3.Distance(colliderPoint, transform.position); //计算玩家与碰撞点的位置

                    if (tempDistance > (basic.boxSize.x * 0.5f + xDistance)) //如果距离大于 碰撞盒子的高度的一半 + 最小地面距离
                    {
                        if (state.isClimb) //如果左右方向没有碰撞体了，退出爬墙状态
                        {
                            state.isClimb = false;
                            playAnimator.ResetTrigger(isClimbId); //重置触发器  退出
                            playAnimator.SetTrigger(exitClimbId);
                        }
                    }
                    else //如果距离小于  根据方向进行位移修正
                    {
                        if (!lRHit2D.collider.CompareTag("Trap")) //如果左右不是陷阱
                        {
                            EnterClimbFunc(transform.position); //检测当前是否能够进入爬墙状态
                            playAnimator.ResetTrigger(exitClimbId);
                        }
                        else {
                            Die();
                        }
                    }
                }
                else {
                    if (state.isClimb) {
                        state.isClimb = false;
                        playAnimator.SetTrigger(exitClimbId);
                        playAnimator.ResetTrigger(isClimbId); //重置触发器  退出
                    }
                }
            }
            else {
                if (state.isClimb) //当左右速度无值时且处于爬墙状态时
                {
                    ExitClimbFunc();
                }
            }

            //更新方向信息，上下轴
            if (moveSpeed.y > 0) {
                dir = 1;
            }
            else if (moveSpeed.y < 0) {
                dir = -1;
            }
            else {
                dir = 0;
            }

            //上下方向进行判断
            if (dir != 0) {
                var uDHit2D = Physics2D.BoxCast(transform.position,
                                                basic.boxSize, 0,
                                                Vector3.up * dir,
                                                5.0f, playerLayerMask);

                if (uDHit2D.collider != null) {
                    var tempYValue = (float) Math.Round(uDHit2D.point.y, 1);
                    var colliderPoint = new Vector3(transform.position.x, tempYValue);
                    var tempDistance = Vector3.Distance(transform.position, colliderPoint);

                    if (tempDistance > (basic.boxSize.y * 0.5f + yDistance)) {
                        state.isGround = false; //更新在地面的bool值
                    }

                    else {
                        if (dir > 0) //如果是朝上方向移动，且距离小于规定距离，就说明玩家头上碰到了物体，反之同理。
                        {
                            state.isGround = false;
                            Debug.Log("头上碰到了物体");
                        }
                        else {
                            Debug.Log("着地");
                            state.isGround = true;
                            playAnimator.ResetTrigger(isStopClimbJumpId);
                        }

                        moveSpeed.y = 0;
                        if (uDHit2D.collider.CompareTag("Trap")) //如果头上是陷阱  死亡
                        {
                            Die();
                        }
                    }
                }
                else {
                    state.isGround = false;
                }
            }
            else {
                state.isGround = CheckIsGround(); //更新在地面的bool值
            }
        }

        /// <summary>
        /// 进入爬墙的函数
        /// </summary>
        private void EnterClimbFunc(Vector3 rayPoint) {
            //设定碰到墙 且  从碰撞点往下 玩家碰撞盒子高度内  没有碰撞体  就可进入碰撞状态。
            var hit2D = Physics2D.BoxCast(
                                          rayPoint, 
                                          basic.boxSize, 0, 
                                          Vector2.down, 
                                          basic.boxSize.y, 
                                          playerLayerMask);
            if (hit2D.collider != null) {
                Debug.Log("无法进入爬墙状态  " + hit2D.collider.name);
            }

            else {
                //如果上方是异形碰撞体，那么就无法进入爬墙状态
                hit2D = Physics2D.BoxCast(rayPoint, basic.boxSize, 0, Vector2.up, basic.boxSize.y * 0.8f,
                                          playerLayerMask);
                if (hit2D.collider == null || hit2D.collider.gameObject.tag != "Arc") {
                    playAnimator.SetTrigger(isClimbId); //动画切换
                    state.isClimb = true;
                    state.isCanDash = true; //爬墙状态，冲刺重置
                }
            }
        }

        /// <summary>
        /// 退出爬墙状态检测
        /// </summary>
        private void ExitClimbFunc() {
            var hit2D = new RaycastHit2D();
            switch (nowDir) {
                case PlayDir.Left:
                    hit2D = Physics2D.Raycast(transform.position, Vector3.left, basic.boxSize.x);
                    break;
                case PlayDir.Right:
                    hit2D = Physics2D.Raycast(transform.position, Vector3.right, basic.boxSize.x);
                    break;
            }

            if (hit2D.collider == null) {
                Invoke(nameof(ExitClimb), 0.1f);
                playAnimator.SetTrigger(exitClimbId);
                playAnimator.ResetTrigger(isClimbId); //重置触发器  退出
            }
        }

        /// <summary>
        /// 检测是否在地面
        /// </summary>
        /// <returns></returns>
        private bool CheckIsGround() {
            var hit2D = Physics2D.BoxCast(transform.position, basic.boxSize, 0, Vector2.down, 5f, playerLayerMask);
            // Debug.DrawLine(transform.position, transform.position + Vector3.down * aryDistance, Color.red, 6.0f);
            if (hit2D.collider != null) {
                var tempDistance = Vector3.Distance(transform.position, hit2D.point);
                return !(tempDistance > (basic.boxSize.y * 0.5f + yDistance));
            }
            else {
                return false;
            }
        }

        /// <summary>
        /// 玩家死亡
        /// </summary>
        private void Die() {
            state.isAlive = false;
        }

        private void ExitClimb() {
            state.isClimb = false;
        }
    }
}