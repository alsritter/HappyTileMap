using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlsRitter.PlayerController.FSM
{
    /// <summary>
    /// 下蹲状态走路
    /// </summary>
    public class CrouchWalkState : PlayerBaseState
    {
        public override string name => "CrouchWalkState";

        private readonly OnCrouchState parentState;

        public CrouchWalkState(OnCrouchState parentState)
        {
            this.parentState = parentState;
        }

        public override void Update(PlayerFSMSystem player)
        {
            if (Mathf.Abs(player.xVelocity) < 0.001)
            {
                TransitionOtherState(parentState, parentState.crouchIdleState, player);
            }
        }

        public override void FixedUpdate(PlayerFSMSystem player)
        {
            player.rb.velocity =
                new Vector2(player.xVelocity * (player.speed / player.crouchSpeedDivisor),
                    player.rb.velocity.y);
        }
    }
}