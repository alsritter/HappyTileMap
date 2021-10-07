using System;
using System.Collections;
using AlsRitter.Global.Store.Player;
using AlsRitter.Global.Store.Player.Model;
using UnityEngine;


namespace AlsRitter.V3.PlayerController.FSM {
    /**
     * 用于移动之类，修改参数之类的操作交给各个状态
     */
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
    public class PlayerCharacter : MonoBehaviour {
        private StateContext stateContext;

        // 这里只存根状态
        private IBaseState onGroundState; // 地面的状态
        private IBaseState inTheAirState; // 空中的状态

        private PlayerViewModel  view;
        private PlayerInputModel input;
        private PlayerBasicModel basic;
        private PlayerStateModel state;

        private void Awake() {
            view = UseStore.GetStore().viewModel;
            input = UseStore.GetStore().inputModel;
            basic = UseStore.GetStore().basicModel;
            state = UseStore.GetStore().stateModel;

            onGroundState = new OnGroundState();
            inTheAirState = new InTheAirState();
            stateContext = new StateContext(onGroundState, UseStore.GetStore());
        }

        private void Update() {
            if (!state.isMove) return;
            
            if (basic.moveSpeed.x >= basic.currentSpeed) {
                RepairHorizontalMove();
            }

            if (basic.moveSpeed.y > 6) {
                CheckUpMove();
            }

            if (state.playState == PlayState.Fall) {
                Fall();
            }
            else if (state.playState == PlayState.Normal
                  || state.playState == PlayState.Run) {
                Normal();
            }


            stateContext.UpdateHandle();
        }

        private void Normal() {
            if (!state.isGround) {
                basic.coyotetimeFram = 4;
                state.playState = PlayState.Fall;
                stateContext.TransitionState(inTheAirState);
                return;
            }

            // 在地面才能跳跃
            if (input.JumpKeyDown) {
                stateContext.TransitionState(inTheAirState);
                // Jump();
                if (input.RunKey && (input.h > 0 && GetDirInt < 0) || (input.h < 0 && GetDirInt > 0)) {
                    Jump(new Vector2(4 * input.moveDir, 0), new Vector2(42, 0));
                }
                else {
                    Jump();
                }
                return;
            }
            else {
                stateContext.TransitionState(onGroundState);
            }

            basic.moveSpeed.y = 0;
        }

        private void FixedUpdate() {
            if (!state.isMove) return;
            
            stateContext.FixedUpdateHandle();

            if (basic.coyotetimeFram > 0) {
                basic.coyotetimeFram--;
            }

            HorizontalMove(basic.currentSpeed);
            var rig = basic.rb;
            rig.MovePosition(rig.transform.position + basic.moveSpeed * Time.fixedDeltaTime);
        }

        /// <summary>
        /// 横向移动
        /// </summary>
        private void HorizontalMove(float speed) {
            //减速速阶段（反向移动时需要先减速）
            if ((basic.moveSpeed.x > 0 && input.moveDir == -1)
             || (basic.moveSpeed.x < 0 && input.moveDir == 1)
             || input.moveDir == 0
             || (input.v < 0)
             || Mathf.Abs(basic.moveSpeed.x) > speed) {
                basic.introDir = basic.moveSpeed.x > 0 ? 1 : -1;
                basic.moveHSpeed = Mathf.Abs(basic.moveSpeed.x);

                basic.moveHSpeed -= speed / 3;

                if (basic.moveHSpeed < 0.01f) {
                    basic.moveHSpeed = 0;
                }

                basic.moveSpeed.x = basic.moveHSpeed * basic.introDir;
            }
            else {
                if (input.moveDir == 1 && !(input.v < 0)) {
                    basic.moveSpeed.x += speed / 6;
                    if (basic.moveSpeed.x > speed)
                        basic.moveSpeed.x = speed;
                }
                else if (input.moveDir == -1 && !(input.v < 0)) {
                    basic.moveSpeed.x -= speed / 6;
                    if (basic.moveSpeed.x < -speed)
                        basic.moveSpeed.x = -speed;
                }
            }
        }

        /**
         * 因为下面的位置修正可能会有穿墙的问题，所以创建一个检查是否可以更改位置
         * 
         * 参考：Unity 碰撞检测 中高速物体 直接穿透问，子弹发射的方向 Vector3.forward 题射线检测的几种用法
         * https://blog.csdn.net/qq_42838904/article/details/91358261
         */
        private void CheckThroughWalls(Vector3 target) {
            // 解决穿墙问题，先发射射线，记录下射线与墙壁的碰撞点
            var oriPos = transform.position; //记录原来的位置
            transform.position = target;
            var length = (transform.position - oriPos).magnitude; //射线的长度
            var direction = transform.position - oriPos; //方向
            RaycastHit hitinfo;
            //在两个位置之间发起一条射线，然后通过这条射线去检测有没有发生碰撞
            var isCollider = Physics.Raycast(oriPos, direction, out hitinfo, length);
            if (!isCollider) {
                // 前后发生了碰撞说明穿墙了
                transform.position = oriPos;
            }
        }

        /**
         * 检测并修正垂直方向的位移
         */
        private bool CheckUpMove() {
            if (view.UpBox.Length != 1) return true;
            var pointDis = view.UpBox[0].point.x - transform.position.x;
            if (pointDis > 0.34f) {
                var offsetPos = Mathf.Floor(transform.position.x);
                CheckThroughWalls(new Vector3(offsetPos + 0.48f, transform.position.y, 0));
                // transform.position = new Vector3(offsetPos + 0.48f, transform.position.y, 0);
                return true;
            }
            else if (pointDis < -0.34f) {
                var offsetPos = Mathf.Floor(transform.position.x);
                // transform.position = new Vector3(offsetPos + 0.52f, transform.position.y, 0);
                CheckThroughWalls(new Vector3(offsetPos + 0.52f, transform.position.y, 0));
                return true;
            }
            else {
                basic.moveSpeed.y = 0;
                state.playState = PlayState.Fall;
                return false;
            }
        }


        /**
         * 跑步的时候为了使移动看起来更加顺滑，会在碰到各种碰撞的边缘处进行处理
         * 当角色在移动中碰到碰撞体的边缘时应该要像果冻一样划过去。当然不只是在跑步中会用到，跳跃时也会用到。
         * 
         * 实现原理：当前玩家位置与碰撞点位置之差来判断是否应该对位移进行修正。
         * 由于地图使用 tilemap 进行绘制，所以瓦片的位置都是能够确定，在此基础上进行修正。
         *
         * 
         * 检测水平方向位移：
         */
        private void RepairHorizontalMove() {
            if (basic.fixHorizon) return;
            if (view.HorizontalBox == null) return;

            view.HorizontalBox = state.playDir == PlayDir.Right ? view.RightBox : view.LeftBox;
            if (view.HorizontalBox.Length != 1) return;

            var pointDis = view.HorizontalBox[0].point.y - basic.moveSpeed.y;

            if (pointDis > 0.34f) {
                var offsetPos = Mathf.Ceil(basic.moveSpeed.y);
                // transform.position = new Vector3(transform.position.x, offsetPos - 0.22f, 0);
                CheckThroughWalls(new Vector3(transform.position.x, offsetPos - 0.22f, 0));
            }
            else if (pointDis < -0.42f) {
                var offsetPos = Mathf.Ceil(transform.position.y);
                // transform.position = new Vector3(transform.position.x, offsetPos + 0.035f, 0);
                CheckThroughWalls(new Vector3(transform.position.x, offsetPos + 0.035f, 0));
            }

            basic.fixHorizon = true;
        }


        /// <summary>
        /// 落下状态
        /// </summary>
        private void Fall() {
            if (state.isGround) {
                state.playState = PlayState.Normal;
                stateContext.TransitionState(onGroundState);
                return;
            }

            if (basic.coyotetimeFram > 0 && input.JumpKeyDown) {
                basic.coyotetimeFram = 0;
                basic.moveSpeed.y = 0;
                Jump();
                return;
            }

            if (IsCanFall()) {
                basic.moveSpeed.y -= 150f * Time.deltaTime;
                basic.moveSpeed.y = Mathf.Clamp(basic.moveSpeed.y, -25, basic.moveSpeed.y);
            }
        }

        private bool IsCanFall() {
            return state.playState != PlayState.Jump;
        }

        //玩家朝向的 int 值（1为 right， -1为 left）
        private int GetDirInt => state.playDir == PlayDir.Right ? 1 : -1;

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
         * 
         * vel.x 为传入的横向加速度，maxVel.x 为最大横向加速度
         */
        private void Jump(Vector2 vel, Vector2 maxVel) {
            state.playState = PlayState.Jump;
            basic.startJumpPos = transform.position.y;
            state.isIntroJump = true;

            if (vel.y >= 0) basic.moveSpeed.y = vel.y;

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
                && basic.moveSpeed.y < curJumpSpeed) {
                if (vel.x != 0 && Mathf.Abs(basic.moveSpeed.x) < maxVel.x) {
                    state.isMove = false;
                    basic.moveSpeed.x += vel.x;
                    if (Mathf.Abs(basic.moveSpeed.x) > maxVel.x) {
                        basic.moveSpeed.x = maxVel.x * GetDirInt;
                    }
                }

                if (!CheckUpMove()) //返回 false 说明撞到墙，结束跳跃
                {
                    basic.moveSpeed.y = 0;
                    state.isIntroJump = false;
                    state.isMove = true;
                    yield break;
                }

                //获取当前角色相对于初始跳跃时的高度
                dis = transform.position.y - basic.startJumpPos;
                if (vel.y <= 0) {
                    basic.moveSpeed.y += 240 * Time.fixedDeltaTime; //加速移动
                }

                yield return new WaitForFixedUpdate();
            }

            basic.moveSpeed.y = curJumpSpeed;
            state.isMove = true;

            // 按住 up 的阶段
            while (state.playState == PlayState.Jump
                && input.JumpKey
                && dis < curJumpMax) {
                if (!CheckUpMove()) {
                    basic.moveSpeed.y = 0;
                    state.isIntroJump = false;
                    yield break;
                }

                // TODO: 这里可以拓展
                // if (input.Jump2KeyDown && state.jumpCount == 1) {
                //     basic.moveSpeed.y = 0;
                //     state.isIntroJump = false;
                //     state.jumpCount = 2;
                //     Jump(new Vector2(5 * GetDirInt, 0), new Vector2(10, 0));
                //     yield break;
                // }

                dis = transform.position.y - basic.startJumpPos;
                basic.moveSpeed.y = curJumpSpeed;
                yield return new WaitForFixedUpdate();
            }

            // slow down
            while (state.playState == PlayState.Jump && basic.moveSpeed.y > 0) {
                if (!CheckUpMove()) {
                    break;
                }


                if (dis > basic.jumpMax) {
                    basic.moveSpeed.y -= 100 * Time.fixedDeltaTime;
                }
                else {
                    basic.moveSpeed.y -= 200 * Time.fixedDeltaTime;
                }

                yield return new WaitForFixedUpdate();
            }


            // fall down
            basic.moveSpeed.y = 0;
            yield return 0.1f;
            state.isIntroJump = false;
            state.playState = PlayState.Fall;
        }
    }
}