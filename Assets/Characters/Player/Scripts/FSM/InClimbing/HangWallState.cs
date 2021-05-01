using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerController.FSM
{
    public class HangWallState : PlayerBaseState
    {
        public readonly InClimbState parentState;

        public override string name => "HangWallState";

        public override void Update(PlayerFSMSystem player)
        {
            if (Mathf.Abs(player.yVelocity) > 0.01)
            {
                TransitionOtherState(parentState, parentState.climbingState, player);
            }
        }

        public override void FixedUpdate(PlayerFSMSystem player)
        {
        }

        public HangWallState(InClimbState parentState)
        {
            this.parentState = parentState;
        }
    }
}