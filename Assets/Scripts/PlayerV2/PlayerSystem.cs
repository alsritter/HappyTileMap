using System;
using System.Collections;
using AlsRitter.GlobalControl.Store;
using AlsRitter.Store.Model;
using UnityEngine;

namespace AlsRitter.Player.System.FSM {
    /**
     * 这里就是角色的控制层
     */
    public class PlayerSystem : MonoBehaviour {
        private PlayerInputModel input;
        private PlayerBasicModel basic;
        private PlayerStateModel state;
        private Rigidbody2D      rig;

        private RaycastHit2D   DownBox;
        private RaycastHit2D[] UpBox;
        private RaycastHit2D[] RightBox;
        private RaycastHit2D[] LeftBox;
        private RaycastHit2D[] HorizontalBox;


        private void OnCollisionStay2D(Collision2D collision) {
            if (collision.contacts != null) {
            }
        }

        protected void Awake() {
            input = UseStore.GetStore().inputModel;
            basic = UseStore.GetStore().basicModel;
            state = UseStore.GetStore().stateModel;
            rig = basic.rb;
        }

        protected void Update() {
            if (!state.isAlive) {
                return; //  死亡不进行任何操作
            }

            RayCastBox();
            CheckDir();

            if (basic.velocity.x >= basic.moveSpeed) {
                CheckHorizontalMove();
            }

            if (basic.velocity.y > 6) {
                CheckUpMove();
            }

            DebugBoxRay(); // 显示碰撞盒射线

            if (input.DashKeyDown && basic.dashCount > 0) {
                // 在攀爬的情况下，没有方向不要冲刺
                if (input.ClimbKey && input.h == 0) {
                }
                else {
                    Dash();
                }
            }

            switch (state.playState) {
                case PlayState.Normal:
                    Normal();
                    break;
                case PlayState.Climb:
                    Climb();
                    break;
                case PlayState.Fall:
                    Fall();
                    break;
                case PlayState.Dash:
                    CheckDashJump();
                    break;
            }
        }

        protected void FixedUpdate() {
            if (basic.coyotetimeFram > 0) {
                basic.coyotetimeFram--;
            }

            HorizontalMove();
            rig.MovePosition(transform.position + basic.velocity * Time.fixedDeltaTime);
        }


        /**
         * 四个方向上的射线检查
         */
        private void RayCastBox() {
            RightBox =
                Physics2D.BoxCastAll(basic.Position,
                                     basic.boxSize, 0,
                                     Vector3.right,
                                     basic.rightCheckDistance,
                                     basic.playerLayerMask);
            LeftBox =
                Physics2D.BoxCastAll(basic.Position,
                                     basic.boxSize, 0,
                                     Vector3.left,
                                     basic.leftCheckDistance,
                                     basic.playerLayerMask);
            UpBox =
                Physics2D.BoxCastAll(basic.Position,
                                     basic.boxSize, 0,
                                     Vector3.up,
                                     basic.upCheckDistance,
                                     basic.playerLayerMask);
            DownBox =
                Physics2D.BoxCast(basic.Position,
                                  basic.boxSize, 0,
                                  Vector3.down,
                                  basic.downCheckDistance,
                                  basic.playerLayerMask);
        }

        /**
         * 检查方向
         */
        private void CheckDir() {
            if (state.playState == PlayState.Climb || state.playState == PlayState.Dash)
                return;

            state.lastDir = state.playDir;
            if (input.MoveDir > 0) {
                state.playDir = PlayDir.Right;
            }
            else if (input.MoveDir < 0) {
                state.playDir = PlayDir.Left;
            }

            // if (state.lastDir != state.playDir) {
            //     transform.localScale = new Vector3(GetDirInt, transform.localScale.y, transform.localScale.z);
            // }
        }


        /// <summary>
        /// 普通陆地状态
        /// </summary>
        private void Normal() {
            if (!isGround) {
                basic.coyotetimeFram = 4;
                state.playState = PlayState.Fall;
                return;
            }

            basic.velocity.y = 0;
            basic.curStamina = basic.climbMaxStamina;
            basic.dashCount = 1;

            if (input.JumpKeyDown) {
                Jump();
                return;
            }
        }

        private bool isGround => DownBox.collider != null;

        #region 横向移动

        private bool IsCanMove() {
            return state.playState != PlayState.Dash && state.playState != PlayState.Climb && state.isCanControl &&
                   state.isMove;
        }

        /// <summary>
        /// 横向移动
        /// </summary>
        private void HorizontalMove() {
            if (!IsCanMove()) return;

            //减速速阶段
            if ((basic.velocity.x > 0 && input.MoveDir == -1) || (basic.velocity.x < 0 && input.MoveDir == 1) ||
                input.MoveDir == 0 ||
                (isGround && input.v < 0) || Mathf.Abs(basic.velocity.x) > basic.moveSpeed) {
                basic.introDir = basic.velocity.x > 0 ? 1 : -1;
                basic.moveH = Mathf.Abs(basic.velocity.x);
                if (isGround) {
                    basic.moveH -= basic.moveSpeed / 3;
                }
                else {
                    basic.moveH -= basic.moveSpeed / 6;
                }

                if (basic.moveH < 0.01f) {
                    basic.moveH = 0;
                }

                basic.velocity.x = basic.moveH * basic.introDir;
            }
            else {
                //蹲下不允许移动
                if (isGround && input.v < 0)
                    return;

                if (input.MoveDir == 1 && !(isGround && input.v < 0)) {
                    if (isGround) {
                        basic.velocity.x += basic.moveSpeed / 6;
                    }
                    else {
                        basic.velocity.x += basic.moveSpeed / 15f;
                    }

                    if (basic.velocity.x > basic.moveSpeed)
                        basic.velocity.x = basic.moveSpeed;
                }
                else if (input.MoveDir == -1 && !(isGround && input.v < 0)) {
                    if (isGround) {
                        basic.velocity.x -= basic.moveSpeed / 6;
                    }
                    else {
                        basic.velocity.x -= basic.moveSpeed / 12f;
                    }

                    if (basic.velocity.x < -basic.moveSpeed)
                        basic.velocity.x = -basic.moveSpeed;
                }
            }
        }

        #endregion

        #region 落地

        /// <summary>
        /// 落下状态
        /// </summary>
        private void Fall() {
            if (isGround) {
                state.playState = PlayState.Normal;
                return;
            }

            if (basic.coyotetimeFram > 0 && input.JumpKeyDown) {
                basic.coyotetimeFram = 0;
                basic.velocity.y = 0;
                Jump();
                return;
            }

            //落下时如果在处在可以爬墙的位置，按下跳跃键即使不爬墙仍能进行小型蹬墙跳
            if (input.JumpKeyDown && BoxCheckCanClimb() && !input.ClimbKey && !CheckIsClimb()) {
                basic.velocity.y = 0;
                basic.velocity.x = 0;
                Jump(new Vector2(4 * -GetClimbDirInt, 0), new Vector2(24, 0));
                return;
            }

            if (IsCanFall()) {
                basic.velocity.y -= 150f * Time.deltaTime;
                basic.velocity.y = Mathf.Clamp(basic.velocity.y, -25, basic.velocity.y);
                if (IsCanClimb() && (CheckIsClimb() || input.ClimbKey)) {
                    state.playState = PlayState.Climb;
                }
            }
        }

        private bool IsCanFall() {
            return state.playState != PlayState.Dash && state.playState != PlayState.Jump &&
                   state.playState != PlayState.Climb;
        }

        #endregion

        #region 位移修正

        /**
         * 冲刺的时候为了使移动看起来更加顺滑，会在碰到各种碰撞的边缘处进行处理
         * 当角色在移动中碰到碰撞体的边缘时应该要像果冻一样划过去。当然不只是在冲刺中会用到，跳跃时也会用到。
         * 
         * 实现原理：当前玩家位置与碰撞点位置之差来判断是否应该对位移进行修正。
         * 由于地图使用 tilemap 进行绘制，所以瓦片的位置都是能够确定，在此基础上进行修正。
         *
         * 
         * 检测水平方向位移：
         */
        private void CheckHorizontalMove() {
            if (basic.fixHorizon) return;

            HorizontalBox = state.playDir == PlayDir.Right ? RightBox : LeftBox;
            if (HorizontalBox.Length != 1) return;

            var pointDis = HorizontalBox[0].point.y - basic.Position.y;

            if (pointDis > 0.34f) {
                var offsetPos = Mathf.Ceil(basic.Position.y);
                transform.position = new Vector3(transform.position.x, offsetPos - 0.22f, 0);
            }
            else if (pointDis < -0.42f) {
                var offsetPos = Mathf.Ceil(transform.position.y);
                transform.position = new Vector3(transform.position.x, offsetPos + 0.035f, 0);
            }

            basic.fixHorizon = true;
        }

        /**
         * 检测并修正垂直方向的位移
         */
        private bool CheckUpMove() {
            if (UpBox.Length != 1) return true;
            var pointDis = UpBox[0].point.x - transform.position.x;
            if (pointDis > 0.34f) {
                var offsetPos = Mathf.Floor(transform.position.x);
                transform.position = new Vector3(offsetPos + 0.48f, transform.position.y, 0);
                return true;
            }
            else if (pointDis < -0.34f) {
                var offsetPos = Mathf.Floor(transform.position.x);
                transform.position = new Vector3(offsetPos + 0.52f, transform.position.y, 0);
                return true;
            }
            else {
                basic.velocity.y = 0;
                state.playState = PlayState.Fall;
                return false;
            }
        }

        //玩家朝向的 int 值（1为 right， -1为 left）
        private int GetDirInt => state.playDir == PlayDir.Right ? 1 : -1;

        #endregion

        #region 冲刺

        /**
         * 在蔚蓝中，冲刺的前 0.15s 是不允许移动的，所以冲刺也是通过协程来实现。
         * 在冲刺前我们要获取到冲刺方向，这里有几个小细节，什么都不按的情况下按冲刺是以角色当前方向进行冲刺，
         * 在地面上按左下或者右下冲刺时是普通的横向冲刺，
         */
        private void Dash() {
            basic.velocity = Vector2.zero;
            basic.dashCount--;
            state.playState = PlayState.Dash;
            StopAllCoroutines();
            StartCoroutine(nameof(IntroDash));
        }

        private IEnumerator IntroDash() {
            //获取输入时的按键方向
            float verticalDir;
            if (isGround && input.v < 0) //在地面上并且按住下时不应该有垂直方向
            {
                verticalDir = 0;
            }
            else {
                verticalDir = input.v;
            }

            //冲刺方向注意归一化
            state.dashDir = new Vector2(input.MoveDir, verticalDir).normalized;
            if (state.dashDir == Vector2.zero) {
                //此处为 0 说明只按下了冲刺键。
                //没有按方向键就朝玩家正前方冲刺 
                state.dashDir = Vector3.right * GetDirInt;
            }

            var i = 0;
            state.isCanControl = false;
            basic.fixHorizon = false;
            // 冲刺 9 帧
            while (i < 9) {
                // 虽然在冲刺的 0.15s 中不允许操控，但有些情况下是可以跳跃的，所以会切换到跳跃状态，
                // 这时不能停止冲刺的协程，因为还要等循环结束把操作权还给玩家，
                // 所以在代码中加了一个判断，当前在冲刺状态时才给角色冲刺速度。
                if (state.playState == PlayState.Dash) {
                    basic.velocity = state.dashDir * 30f * basic.dashSpeed;
                }

                i++;
                CheckHorizontalMove();
                // 用 Time.time 来控制时间是很不准确的，会出现冲刺距离时远时近。
                // 所以在协程里想要精确到帧的控制可以使用 WaitForFixedUpdate。
                // 0.15s 在 1秒 60 帧的情况下就是 9 帧。（注意：FixedUpdate 默认是 0.02s 执行一次，即 50帧
                // 可以在 Edit->Project Setting->time ->Fixed Timestep 设置
                yield return new WaitForFixedUpdate();
            }

            state.isCanControl = true;

            // 如果最后不在冲刺状态了（没有再次按下冲刺按钮），给予一个向下的力
            if (state.playState == PlayState.Dash) {
                if (state.dashDir.y > 0) {
                    basic.velocity.y = 24;
                }

                state.playState = isGround ? PlayState.Normal : PlayState.Fall;
            }
        }

        private void CheckDashJump() {
            if (state.playState == PlayState.Dash) {
                if (input.JumpKeyDown) {
                    if (state.dashDir == Vector2.up && BoxCheckCanClimbDash()) {
                        Jump(new Vector2(4 * -GetClimbDirInt, 24 - basic.jumpSpeed + 6), new Vector2(24, 0));
                    }
                    else if (isGround) {
                        basic.velocity.y = 0;
                        if (input.v < 0) {
                            if (input.MoveDir != 0) {
                                basic.dashCount = 1;
                                basic.velocity = new Vector3(30 * input.MoveDir, 0);
                                Jump(new Vector2(4 * input.MoveDir, -4), new Vector2(42, 0));
                            }
                            else {
                                Jump(new Vector2(0, -4), new Vector2(0, 0));
                            }
                        }
                        else {
                            Jump();
                        }
                    }
                }
            }
        }

        #endregion

        #region 跳跃

        /**
         * 蔚蓝中跳跃是会有一个最小跳跃高度和最大跳跃高度，并且跳跃分成了三个阶段：
         * 1、加速上升阶段
         * 2、到达最小跳跃高度后如果仍然按主跳跃键则继续升高阶段
         * 3、到达最高点或者放开跳跃键后的减速上升阶段，上升速度为0时切换到落下状态。
         */
        private void Jump() {
            state.playState = PlayState.Jump;
            basic.startJumpPos = transform.position.y;
            state.isIntroJump = true;
            StartCoroutine(IntroJump(Vector2.zero, Vector2.zero));
        }


        /**
         * 记录初始位置和计算最高能跳到的位置，根据按键时间进行跳跃高度判断
         */
        private void Jump(Vector2 vel, Vector2 maxVel) {
            state.playState = PlayState.Jump;
            basic.startJumpPos = transform.position.y;
            state.isIntroJump = true;
            if (vel.y >= 0) basic.velocity.y = vel.y;

            StartCoroutine(IntroJump(vel, maxVel));
        }

        private IEnumerator IntroJump(Vector2 vel, Vector2 maxVel) {
            float dis = 0;
            // move up
            var curJumpMin = basic.jumpMin * (vel.y + basic.jumpSpeed) / basic.jumpSpeed;
            var curJumpMax = basic.jumpMax * (vel.y + basic.jumpSpeed) / basic.jumpSpeed;
            var curJumpSpeed = basic.jumpSpeed + vel.y;

            while (state.playState == PlayState.Jump
                && dis <= curJumpMin
                && basic.velocity.y < curJumpSpeed) {
                if (vel.x != 0 && Mathf.Abs(basic.velocity.x) < maxVel.x) {
                    state.isMove = false;
                    basic.velocity.x += vel.x;
                    if (Mathf.Abs(basic.velocity.x) > maxVel.x) {
                        basic.velocity.x = maxVel.x * GetDirInt;
                    }
                }

                if (!CheckUpMove()) //返回 false 说明撞到墙，结束跳跃
                {
                    basic.velocity.y = 0;
                    state.isIntroJump = false;
                    state.isMove = true;
                    yield break;
                }

                //获取当前角色相对于初始跳跃时的高度
                dis = transform.position.y - basic.startJumpPos;
                if (vel.y <= 0) {
                    basic.velocity.y += 240 * Time.fixedDeltaTime; //加速移动
                }

                yield return new WaitForFixedUpdate();
            }

            basic.velocity.y = curJumpSpeed;
            state.isMove = true;
            while (state.playState == PlayState.Jump && input.JumpKey && dis < curJumpMax) {
                if (!CheckUpMove()) {
                    basic.velocity.y = 0;
                    state.isIntroJump = false;
                    yield break;
                }

                if (input.JumpKeyDown && BoxCheckCanClimb() && !input.ClimbKey && !CheckIsClimb()) {
                    basic.velocity.y = 0;
                    state.isIntroJump = false;
                    Jump(new Vector2(4 * -GetDirInt, 0), new Vector2(24, 0));
                    yield break;
                }

                dis = transform.position.y - basic.startJumpPos;
                basic.velocity.y = curJumpSpeed;
                yield return new WaitForFixedUpdate();
            }

            // slow down
            while (state.playState == PlayState.Jump && basic.velocity.y > 0) {
                if (!CheckUpMove()) {
                    break;
                }

                if (input.JumpKeyDown && BoxCheckCanClimb() && !input.ClimbKey && !CheckIsClimb()) {
                    basic.velocity.y = 0;
                    state.isIntroJump = false;
                    Jump(new Vector2(4 * -GetDirInt, 0), new Vector2(24, 0));
                    yield break;
                }

                if (dis > basic.jumpMax) {
                    basic.velocity.y -= 100 * Time.fixedDeltaTime;
                }
                else {
                    basic.velocity.y -= 200 * Time.fixedDeltaTime;
                }

                yield return new WaitForFixedUpdate();
            }

            // fall down
            basic.velocity.y = 0;
            yield return 0.1f;
            state.isIntroJump = false;
            state.playState = PlayState.Fall;
        }

        #endregion

        #region 爬墙

        //正确情况的蹬墙跳应该是墙壁相对于玩家的反方向，爬墙的时候对玩家朝向进行了修正，所以玩家的反方向就是跳跃方向，
        //但是在 fall 状态下没有对玩家方向进行修改，所以只能通过墙的位置进行判断
        private int GetClimbDirInt => HorizontalBox == RightBox ? 1 : -1;

        /// <summary>
        /// 检测是周围是否有墙壁，既是否可以爬墙。
        /// </summary>
        /// <returns></returns>
        private bool BoxCheckCanClimbDash() {
            RightBox = Physics2D.BoxCastAll(basic.Position, basic.boxSize, 0, Vector3.right, 0.4f,
                                            basic.playerLayerMask);
            LeftBox = Physics2D.BoxCastAll(basic.Position, basic.boxSize, 0, Vector3.left, 0.4f, basic.playerLayerMask);
            if (RightBox.Length > 0) {
                HorizontalBox = RightBox;
            }
            else if (LeftBox.Length > 0) {
                HorizontalBox = LeftBox;
            }

            return RightBox.Length > 0 || LeftBox.Length > 0;
        }

        private bool BoxCheckCanClimb() {
            if (RightBox.Length > 0) {
                HorizontalBox = RightBox;
            }
            else if (LeftBox.Length > 0) {
                HorizontalBox = LeftBox;
            }

            return RightBox.Length > 0 || LeftBox.Length > 0;
        }

        private bool CheckIsClimb() {
            return (input.MoveDir < 0 && LeftBox.Length > 0) || (input.MoveDir > 0 && RightBox.Length > 0);
        }

        private bool IsCanClimb() {
            return (state.playState != PlayState.Dash && state.playState != PlayState.Jump) && BoxCheckCanClimb() &&
                   state.isCanControl &&
                   !state.isIntroJump;
        }

        /// <summary>
        /// 攀爬主方法
        /// </summary>
        private void Climb() {
            var checkBox = BoxCheckCanClimb();
            if (!input.ClimbKey || !checkBox) {
                if (isGround) {
                    state.playState = PlayState.Normal;
                    return;
                }

                if (!CheckIsClimb()) {
                    state.playState = PlayState.Fall;
                    return;
                }
            }

            basic.velocity.x = 0;
            state.playDir = HorizontalBox == RightBox ? PlayDir.Right : PlayDir.Left;
            //爬墙时，检测是否接近墙的最上端，小于一定距离时自动跳到平台上
            if (IsCanClimb()) {
                if (UpBox.Length == 0) {
                    if (input.v > 0 && transform.position.y - HorizontalBox[0].point.y > 0.9f) {
                        StartCoroutine(nameof(ClimbAutoJump));
                        return;
                    }
                }

                //如果爬在墙的最上端要么自动跳到平台上，要么滑落一段距离
                if (input.v <= 0 && transform.position.y - HorizontalBox[0].point.y > 0.7f || !input.ClimbKey) {
                    basic.velocity.y = -basic.climbSpeed;
                }
                else if (transform.position.y - HorizontalBox[0].point.y <= 0.7f || input.ClimbKey) {
                    basic.velocity.y = input.v * basic.climbSpeed;
                }
            }

            //蹬墙跳
            if (input.JumpKeyDown) {
                if (input.ClimbKey) {
                    if ((input.h > 0 && GetDirInt < 0) || (input.h < 0 && GetDirInt > 0)) {
                        Jump(new Vector2(8 * -GetDirInt, 0), new Vector2(24, 0));
                    }
                    else {
                        // Jump(new Vector2(8 * -GetDirInt, 0), new Vector2(12, 0));
                    }
                }
                else {
                    Jump(new Vector2(8 * -GetDirInt, 0), new Vector2(24, 0));
                }
            }
        }

        /// <summary>
        /// 攀爬到墙壁最上沿时如果有可跳跃平台，则自动跳跃到平台上
        /// </summary>
        private IEnumerator ClimbAutoJump() {
            var posY = Mathf.Ceil(transform.position.y);
            state.isCanControl = false;
            basic.velocity = Vector3.zero;
            while (posY + 1f - transform.position.y > 0) {
                basic.velocity.y = basic.jumpSpeed;
                basic.velocity.x = GetDirInt * 15;
                yield return null;
            }

            basic.velocity = Vector3.zero;
            state.playState = PlayState.Fall;
            state.isCanControl = true;
        }

        #endregion


        /**
         * 显示碰撞盒的射线
         */
        private void DebugBoxRay() {
            //碰撞点位 debug
            if (HorizontalBox != null && HorizontalBox.Length > 0 && HorizontalBox[0]) {
                Debug.DrawLine(basic.Position, HorizontalBox[0].point, Color.yellow);
            }

            if (UpBox != null && UpBox.Length > 0 && UpBox[0]) {
                Debug.DrawLine(basic.Position, UpBox[0].point, Color.red);
            }

            if (RightBox != null && RightBox.Length > 0 && RightBox[0]) {
                Debug.DrawLine(basic.Position, RightBox[0].point, Color.green);
            }

            if (LeftBox != null && LeftBox.Length > 0 && LeftBox[0]) {
                Debug.DrawLine(basic.Position, LeftBox[0].point, Color.blue);
            }
        }
    }
}