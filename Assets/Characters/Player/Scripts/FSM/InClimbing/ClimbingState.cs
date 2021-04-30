using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        // 如果已经在地面了就无法再下降了
        if (player.isOnGround && player.yVelocity < 0) return;

        // 如果到顶端了会自动跳跃
        if (!player.isOnGround && player.isOnWallTap && player.yVelocity > 0)
        {
            player.rb.bodyType = RigidbodyType2D.Dynamic;
            player.rb.AddForce(new Vector2(0, player.jumpForce / player.jump2ForceDivisor), ForceMode2D.Impulse);
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