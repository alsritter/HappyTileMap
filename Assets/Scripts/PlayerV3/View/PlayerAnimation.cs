using AlsRitter.EventFrame;
using AlsRitter.EventFrame.CustomEvent;
using AlsRitter.Global.Store.Player;
using AlsRitter.Global.Store.Player.Model;
using AlsRitter.PlayerController.FSM;
using UnityEngine;


namespace AlsRitter.V3.PlayerController
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Animator), typeof(PlayerFSMSystem))]
    public class PlayerAnimation : MonoBehaviour, IEventObserver
    {
        private PlayerInputModel input;
        private PlayerBasicModel basic;
        private PlayerViewModel  view;
        private PlayerStateModel state;
        
        private Animator anim;
        private Rigidbody2D rb;
        
        private int isCrouchingId;
        private int isRunId;
        private int verticalVelocityId;
        private int inTheAirId;
        private int isRotateTriggerId;

        private int IsDieTriggerId;
        private int inTheAirTriggerId;
        private int isCrouchingTriggerId;

        public PlayDir nowDir; //现在的玩家的方向
        
        private void Awake()
        {
            input = UseStore.GetStore().inputModel;
            basic = UseStore.GetStore().basicModel;
            view = UseStore.GetStore().viewModel;
            state = UseStore.GetStore().stateModel;
            EventManager.Register(this, EventID.IsCrouching, EventID.Run, EventID.InTheAir, EventID.GraspWall);
        }

        private void Start() {
            anim = view.playAnimator;
            rb = basic.rb;

            isCrouchingId = Animator.StringToHash("isCrouching");
            isRunId = Animator.StringToHash("isRun");
            inTheAirId = Animator.StringToHash("inTheAir");
            verticalVelocityId = Animator.StringToHash("verticalVelocity");

            
            isRotateTriggerId = Animator.StringToHash("isRotate");
            IsDieTriggerId = Animator.StringToHash("IsDieTrigger");
            inTheAirTriggerId = Animator.StringToHash("InTheAirTrigger");
            isCrouchingTriggerId = Animator.StringToHash("IsCrouchingTrigger");
        }

        // Update is called once per frame
        private void Update() {
            DirToRotate();
            anim.SetFloat(verticalVelocityId, rb.velocity.y);
        }

        public void HandleEvent(EventData resp)
        {
            switch (resp.eid)
            {
                case EventID.IsCrouching:
                    anim.SetBool(isCrouchingId, ((PlayerStateEventData) resp).trigger);
                    if (((PlayerStateEventData) resp).trigger)
                    {
                        anim.SetTrigger(isCrouchingTriggerId);
                    }
                    break;
                case EventID.Run:
                    anim.SetBool(isRunId, ((PlayerStateEventData) resp).trigger);
                    break;
                case EventID.InTheAir:
                    anim.SetBool(inTheAirId, ((PlayerStateEventData) resp).trigger);
                    if (((PlayerStateEventData) resp).trigger)
                    {
                        anim.SetTrigger(inTheAirTriggerId);
                    }
                    break;
            }
        }

        /// <summary>
        /// 根据方向进行旋转
        /// </summary>
        private void DirToRotate() {
            if (nowDir == PlayDir.Left && basic.moveSpeed.x > 0) {
                anim.transform.Rotate(0, 180, 0);
                nowDir = PlayDir.Right;
                if (state.isGround) {
                    anim.SetTrigger(IsDieTriggerId);
                }
            }
            else if (nowDir == PlayDir.Right && basic.moveSpeed.x < 0) {
                anim.transform.Rotate(0, -180, 0);
                nowDir = PlayDir.Left;
                if (state.isGround) //在地面才播放转向动画
                {
                    anim.SetTrigger(IsDieTriggerId);
                }
            }
            else if (nowDir == PlayDir.Right && basic.moveSpeed.x > 0) {
                anim.SetBool(isRunId, true);
            }
            else if (nowDir == PlayDir.Left && basic.moveSpeed.x < 0) {
                anim.SetBool(isRunId, true);
            }
        }
        
        public void OnDestroy() {
            EventManager.Remove(this);
        }
    }
}