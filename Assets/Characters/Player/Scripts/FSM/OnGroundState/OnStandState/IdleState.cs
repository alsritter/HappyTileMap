using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 站立的待机状态
/// </summary>
public class IdleState : PlayerBaseState
{
    public IdleState(OnStandState parentState)
    {
        this.parentState = parentState;
    }

    private readonly OnStandState parentState;

    public override string name => "IdleState";

    public override void Update(PlayerFSMSystem player)
    {
        if (Mathf.Abs(player.xVelocity) > 0.001)
        {
            TransitionOtherState(parentState, parentState.walkState, player);
        }
    }


    public override void FixedUpdate(PlayerFSMSystem player)
    {
    }
}