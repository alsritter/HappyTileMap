using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump2State : PlayerBaseState
{
    public override string name => "Jump2State";

    private readonly InTheAirState parentSate;

    public override void Enter(PlayerFSMSystem player)
    {
        // 先跳跃
        if (player != null)
        {
            player.rb.AddForce(
                new Vector2(0f, player.jumpForce / player.jump2ForceDivisor), ForceMode2D.Impulse);
        }
    }

    public Jump2State(InTheAirState parentSate)
    {
        this.parentSate = parentSate;
    }

    public override void Update(PlayerFSMSystem player)
    {
    }

    public override void FixedUpdate(PlayerFSMSystem player)
    {
    }
}