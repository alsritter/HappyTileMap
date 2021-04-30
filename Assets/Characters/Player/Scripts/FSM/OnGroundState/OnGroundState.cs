using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 在地面的状态
/// </summary>
public class OnGroundState : PlayerBaseState
{
    // 地面的状态
    public readonly OnStandState onStandState;
    public readonly OnCrouchState onCrouchState;

    public override string name => "OnGroundState";

    /// <summary>
    /// 在入口点重置当前状态
    /// </summary>
    /// <param name="player"></param>
    public override void Enter(PlayerFSMSystem player)
    {
        TransitionState(onStandState, player);
    }

    public override void Update(PlayerFSMSystem player)
    {
        currentState.Update(player);
    }

    public override void FixedUpdate(PlayerFSMSystem player)
    {
        currentState.FixedUpdate(player);
    }

    public OnGroundState()
    {
        onStandState = new OnStandState(this);
        onCrouchState = new OnCrouchState(this);
        TransitionState(onStandState,null);
    }

    /// <summary>
    /// 在入口点重置当前状态
    /// </summary>
    /// <param name="player"></param>
    public override void Exit(PlayerFSMSystem player)
    {
        // 退出状态时也修改一下当前的状态
        TransitionState(onStandState, player);
    }
}