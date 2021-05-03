using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerController.FSM
{
    public class ClimbingState : PlayerBaseState
    {
        public readonly InClimbState parentState;

        public override string name => "Climbing";


        public override void Update(PlayerFSMSystem player)
        {
            if (Mathf.Abs(player.yVelocity) < 0.01)
            {
                TransitionOtherState(parentState, parentState.hangWallState, player);
            }
        }

        public override void FixedUpdate(PlayerFSMSystem player)
        {
            switch (player.isOnGround)
            {
                // 如果已经在地面了就无法再下降了
                case true when player.yVelocity < 0:
                    return;
                // 如果到顶端了无法向上移动
                case false when player.isOnWallTap && player.yVelocity > 0:
                    //player.rb.bodyType = RigidbodyType2D.Dynamic;
                    //player.rb.AddForce(new Vector2(0, player.jumpForce / player.jump2ForceDivisor / 2),ForceMode2D.Impulse);
                    player.yVelocity = 0;
                    break;
            }

            player.transform.position =
                new Vector2(player.transform.position.x,
                    player.transform.position.y + player.climbSpeed * player.yVelocity);
        }

        public ClimbingState(InClimbState parentState)
        {
            this.parentState = parentState;
        }
    }
}