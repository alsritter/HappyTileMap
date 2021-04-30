using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : PlayerBaseState
{
    public override string name => "JumpState";

    private readonly InTheAirState parentSate;

    private int jumpNumber; // 跳跃次数，避免第一次就切换到二段跳了

    public override void Enter(PlayerFSMSystem player)
    {
        jumpNumber = 0;
        // 先跳跃
        if (player != null)
        {
            player.rb.AddForce(new Vector2(0f, player.jumpForce), ForceMode2D.Impulse);
        }
    }

    public JumpState(InTheAirState parentSate)
    {
        this.parentSate = parentSate;
    }

    public override void Update(PlayerFSMSystem player)
    {
        if (Input.GetButtonDown("Jump") && jumpNumber > 0)
        {
            TransitionOtherState(parentSate, parentSate.jump2State, player);
        }
        jumpNumber++;
    }

    public override void FixedUpdate(PlayerFSMSystem player)
    {
        // Debug.Log("当前是 Jump 状态");
    }


    public override void Exit(PlayerFSMSystem player)
    {
        jumpNumber = 0;
    }
}