using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerController.FSM
{
    public class WalkState : PlayerBaseState
    {
        public WalkState(OnStandState parentState)
        {
            this.parentState = parentState;
        }

        private readonly OnStandState parentState;

        public override string name => "WalkState";

        public override void Update(PlayerFSMSystem player)
        {
            if (Mathf.Abs(player.xVelocity) < 0.001)
            {
                TransitionOtherState(parentState, parentState.idleState, player);
            }

            if (Input.GetButton("Run"))
            {
                TransitionOtherState(parentState, parentState.runState, player);
            }
        }

        public override void FixedUpdate(PlayerFSMSystem player)
        {
            player.rb.velocity =
                new Vector2(player.xVelocity * player.speed,
                    player.rb.velocity.y);
        }
    }
}
