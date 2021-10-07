using AlsRitter.EventFrame;
using AlsRitter.Global.Store.Player;
using AlsRitter.Global.Store.Player.Model;
using UnityEngine;


namespace AlsRitter.V3.PlayerController {
    [DisallowMultipleComponent]
    public class PlayerAnimation : MonoBehaviour, IEventObserver {
        private PlayerInputModel input;
        private PlayerBasicModel basic;
        private PlayerViewModel  view;
        private PlayerStateModel state;

        private Animator anim;

        private int isRunId;
        private int yVelocityId;
        private int xVelocityId;
        private int isJumpId;
        private int inGroundId;
        private int isMoveId;
        private int isFullId;
        private int isDieTriggerId;

        public PlayDir nowDir; //现在的玩家的方向

        private void Awake() {
            input = UseStore.GetStore().inputModel;
            basic = UseStore.GetStore().basicModel;
            view = UseStore.GetStore().viewModel;
            state = UseStore.GetStore().stateModel;
            
            EventManager.Register(this, EventID.Harm);
        }

        private void Start() {
            anim = view.playAnimator;
            inGroundId = Animator.StringToHash("inGround");
            isRunId = Animator.StringToHash("isRun");
            isMoveId = Animator.StringToHash("isMove");
            isJumpId = Animator.StringToHash("isJump");
            isFullId = Animator.StringToHash("isFull");
            yVelocityId = Animator.StringToHash("yVelocity");
            xVelocityId = Animator.StringToHash("xVelocity");
            isDieTriggerId = Animator.StringToHash("IsDieTrigger");
        }


        // Update is called once per frame
        private void Update() {
            anim.SetFloat(yVelocityId, basic.moveSpeed.y);
            anim.SetFloat(xVelocityId, basic.moveSpeed.x);

            anim.SetBool(inGroundId, state.isGround);

            DirToRotate();

            var move = input.MoveKey;

            if (move) {
                moveFrame = 6;
            }

            if (state.isGround) {
                anim.SetBool(isJumpId, false);
                anim.SetBool(isFullId, false);
            }


            if (!state.isGround) {
                if (state.playState == PlayState.Jump) {
                    anim.SetBool(isJumpId, true);
                }
                else if (state.playState == PlayState.Fall) {
                    anim.SetBool(isJumpId, false);
                    anim.SetBool(isFullId, true);
                }
            }

            if (state.isGround && (move || moveFrame != 0)) {
                anim.SetBool(isMoveId, true);
                if (state.playState == PlayState.Run) {
                    anim.SetBool(isRunId, true);
                }
                else {
                    anim.SetBool(isRunId, false);
                }
            }
            else {
                anim.SetBool(isRunId, false);
                anim.SetBool(isMoveId, false);
            }

            // JumpCheck();
        }

        private int moveFrame;

        // 缓存几帧移动状态
        private void FixedUpdate() {
            if (moveFrame != 0) {
                moveFrame--;
            }
        }

        /// <summary>
        /// 根据方向进行旋转
        /// </summary>
        private void DirToRotate() {
            if (nowDir == PlayDir.Left && basic.moveSpeed.x > 0) {
                anim.transform.Rotate(0, 180, 0);
                nowDir = PlayDir.Right;
            }
            else if (nowDir == PlayDir.Right && basic.moveSpeed.x < 0) {
                anim.transform.Rotate(0, -180, 0);
                nowDir = PlayDir.Left;
            }
        }

        public void HandleEvent(EventData resp) {
            switch (resp.eid) {
                // 受伤事件
                case EventID.Harm:
                    if (state.isDie) {
                        anim.SetTrigger(isDieTriggerId);
                    }
                    // 受伤
                    break;
            }
        }
        
        public void OnDestroy() {
            EventManager.Remove(this);
        }
    }
}