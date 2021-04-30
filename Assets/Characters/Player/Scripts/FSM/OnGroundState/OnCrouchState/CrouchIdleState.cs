using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 下蹲时的走路状态
/// </summary>
public class CrouchIdleState : PlayerBaseState
{
    public override string name => "CrouchIdleState";

    private readonly OnCrouchState parentState;

    public CrouchIdleState(OnCrouchState parentState)
    {
        this.parentState = parentState;
    }

    public override void Update(PlayerFSMSystem player)
    {
        if (Mathf.Abs(player.xVelocity) > 0.001)
        {
            TransitionOtherState(parentState, parentState.crouchWalkState, player);
        }
    }

    public override void FixedUpdate(PlayerFSMSystem player)
    {
    }
}