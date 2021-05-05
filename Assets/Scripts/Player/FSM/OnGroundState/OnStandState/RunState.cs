using System.Collections;
using System.Collections.Generic;
using EventFrame;
using EventFrame.CustomEvent;
using UnityEngine;

namespace PlayerController.FSM
{
    public class RunState : PlayerBaseState
    {

        private readonly OnStandState parentState;
        private readonly PlayerStateEventData isRunEvent;

        public override string name => "RunState";

        public RunState(OnStandState parentState)
        {
            this.parentState = parentState;
            isRunEvent = new PlayerStateEventData(EventID.Run);
        }

        public override void Enter(PlayerFSMSystem player)
        {
            // player.isRun = true;
            isRunEvent.UpdateState(true);
        }

        public override void Update(PlayerFSMSystem player)
        {
            if (Mathf.Abs(player.xVelocity) < 0.001)
            {
                TransitionOtherState(parentState, parentState.idleState, player);
            }

            if (Input.GetButtonUp("Run"))
            {
                TransitionOtherState(parentState, parentState.walkState, player);
            }
        }

        public override void FixedUpdate(PlayerFSMSystem player)
        {
            player.rb.velocity =
                new Vector2(player.xVelocity * (player.speed + player.speed / player.runDivisor),
                    player.rb.velocity.y);
        }

        public override void Exit(PlayerFSMSystem player)
        {
            // player.isRun = false;
            isRunEvent.UpdateState(false);
        }
    }
}