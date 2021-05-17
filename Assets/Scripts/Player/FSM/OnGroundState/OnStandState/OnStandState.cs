using System.Collections;
using System.Collections.Generic;
using AlsRitter.EventFrame;
using AlsRitter.EventFrame.CustomEvent;
using UnityEngine;

namespace AlsRitter.PlayerController.FSM
{
    /// <summary>
    /// 站立状态
    /// </summary>
    public class OnStandState : PlayerBaseState, IEventObserver
    {
        // 站起的状态
        public readonly WalkState walkState;
        public readonly IdleState idleState; // 初始状态
        public readonly RunState runState;

        private readonly OnGroundState parentSate;

        private bool isHalfFoot = false;

        /// <summary>
        /// 在入口点重置当前状态
        /// </summary>
        /// <param name="player"></param>
        public override void Enter(PlayerFSMSystem player)
        {
            TransitionState(idleState, player);
        }

        public OnStandState(OnGroundState parentSate)
        {
            this.parentSate = parentSate;
            idleState = new IdleState(this);
            runState = new RunState(this);
            walkState = new WalkState(this);
            TransitionState(idleState, null);
            EventManager.Register(this, EventID.HalfFoot);
        }

        public override string name => "OnStandState";

        public override void Update(PlayerFSMSystem player)
        {
            // 一只脚着地无法蹲下
            if (Input.GetButton("Crouch") && !isHalfFoot)
            {
                TransitionOtherState(parentSate, parentSate.onCrouchState, player);
                return;
            }

            currentState.Update(player);
        }

        public override void FixedUpdate(PlayerFSMSystem player)
        {
            currentState.FixedUpdate(player);
        }

        public override void Exit(PlayerFSMSystem player)
        {
            currentState.Exit(player);
        }

        public void HandleEvent(EventData resp)
        {
            switch (resp.eid)
            {
                case EventID.HalfFoot:
                    isHalfFoot = ((PlayerStateEventData) resp).trigger;
                    break;
            }
        }

        ~OnStandState() {
            EventManager.Remove(this);
        }
    }
}