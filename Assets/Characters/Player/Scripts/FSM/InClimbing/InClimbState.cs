using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InClimbState : PlayerBaseState
{
    public readonly HangWallState hangWallState;
    public readonly ClimbingState climbingState;


    public override string name => "InClimbState";

    public override void Enter(PlayerFSMSystem player)
    {
        player.rb.bodyType = RigidbodyType2D.Static;
        TransitionState(hangWallState, player);
    }

    public override void Update(PlayerFSMSystem player)
    {
        currentState.Update(player);
        // 如果跳跃
        if (Input.GetButtonDown("Jump"))
        {
            player.rb.bodyType = RigidbodyType2D.Dynamic;
            player.rb.AddForce(
                // 这个 transform.localScale.x 用于判断方向
                new Vector2(-player.climbLateralForce * player.transform.localScale.x,
                    player.jumpForce), ForceMode2D.Impulse);
        }
    }

    public override void FixedUpdate(PlayerFSMSystem player)
    {
        currentState.FixedUpdate(player);
    }

    public InClimbState()
    {
        hangWallState = new HangWallState(this);
        climbingState = new ClimbingState(this);
        TransitionState(hangWallState, null);
    }

    public override void Exit(PlayerFSMSystem player)
    {
        player.rb.bodyType = RigidbodyType2D.Dynamic;
        currentState.Exit(player);
    }
}