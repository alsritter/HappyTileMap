using System.Collections;
using System.Collections.Generic;
using AlsRitter.EventFrame;
using AlsRitter.EventFrame.CustomEvent;
using UnityEngine;

namespace AlsRitter.PlayerController.FSM
{
    /// <summary>
    /// 下蹲状态
    /// </summary>
    public class OnCrouchState : PlayerBaseState, IEventObserver
    {
        // 蹲下的状态
        public readonly CrouchWalkState crouchWalkState;
        public readonly CrouchIdleState crouchIdleState; // 初始状态

        private readonly OnGroundState parentSate;

/*        // 站立时的状态
        private Vector2 colliderStandSize; // 保存站立时的 BoxCollider2D 大小
        private Vector2 colliderStandOffset; // 保存站立时的 BoxCollider2D 位置偏移


        // 下蹲时的状态
        private Vector2 colliderCrouchSize;
        private Vector2 colliderCrouchOffset;*/

        private readonly PlayerStateEventData isCrouchingEvent;

        private bool isInit = false;
        private bool isTopEmpty = true;

        
        public override string name => "OnCrouchState";

        public OnCrouchState(OnGroundState parentSate)
        {
            this.parentSate = parentSate;
            crouchWalkState = new CrouchWalkState(this);
            crouchIdleState = new CrouchIdleState(this);

            isCrouchingEvent = new PlayerStateEventData(EventID.IsCrouching);
            EventManager.Register(this,EventID.OnTopWall);

            TransitionState(crouchIdleState, null);
        }


        /// <summary>
        /// 在入口点重置当前状态
        /// </summary>
        /// <param name="player"></param>
        public override void Enter(PlayerFSMSystem player)
        {
            if (!isInit)
            {
                /*colliderStandSize = player.coll.size;
                colliderStandOffset = player.coll.offset; // 实际就是中心点
                colliderCrouchSize = new Vector2(player.coll.size.x, player.coll.size.y / 2);
                colliderCrouchOffset =
                    new Vector2(player.coll.offset.x, player.coll.offset.y + (player.coll.offset.y / 2));*/
                isInit = true;
            }

            TransitionState(crouchIdleState, player);
            /*// 修改当前角色的碰撞盒大小
            player.coll.size = colliderCrouchSize;
            player.coll.offset = colliderCrouchOffset;*/

            // player.isCrouching = true;
            isCrouchingEvent.UpdateState(true);
        }


        public override void Update(PlayerFSMSystem player)
        {
            if (!Input.GetButton("Crouch") && isTopEmpty)
            {
                TransitionOtherState(parentSate, parentSate.onStandState, player);
                return;
            }

            currentState.Update(player);
        }

        public override void FixedUpdate(PlayerFSMSystem player)
        {
            currentState.FixedUpdate(player);
        }

        /// <summary>
        /// 在入口点重置当前状态
        /// </summary>
        /// <param name="player"></param>
        public override void Exit(PlayerFSMSystem player)
        {
            // 改回当前角色的碰撞盒大小
           /* player.coll.size = colliderStandSize;
            player.coll.offset = colliderStandOffset;*/

            // player.isCrouching = false;
            isCrouchingEvent.UpdateState(false);

            currentState.Exit(player);
        }

        public void HandleEvent(EventData resp)
        {
            switch (resp.eid)
            {
                case EventID.OnTopWall:
                    isTopEmpty = !((PlayerStateEventData) resp).trigger;
                    break;
            }
        }
    }
}