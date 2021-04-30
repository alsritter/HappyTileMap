using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 状态基类
/// </summary>
public abstract class PlayerBaseState
{
    public PlayerBaseState currentState { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="state"></param>
    /// <param name="player"></param>
    /// <param name="executeEnter">是否执行 Enter</param>
    /// <param name="executeExit">是否执行 Exit</param>
    public void TransitionState(PlayerBaseState state, PlayerFSMSystem player, bool executeEnter, bool executeExit)
    {
        if (executeExit) currentState?.Exit(player);
        currentState = state;
        if (executeEnter) currentState?.Enter(player); // 如果为空则不执行
    }


    /// <summary>
    /// 重置方法，默认都执行
    /// </summary>
    /// <param name="state"></param>
    /// <param name="player"></param>
    public void TransitionState(PlayerBaseState state, PlayerFSMSystem player)
    {
        // Debug.Log(currentState.name + "====>" + state.name);
        currentState?.Exit(player); // 如果为空则不执行
        currentState = state;
        currentState?.Enter(player);
    }

    /// <summary>
    /// 设置其它对象的状态
    /// </summary>
    /// <param name="target">被设置状态的目标</param>
    /// <param name="state"></param>
    /// <param name="player"></param>
    public void TransitionOtherState(PlayerBaseState target, PlayerBaseState state, PlayerFSMSystem player)
    {
        target.TransitionState(state, player);
    }

    /// <summary>
    /// 设置其它对象的状态
    /// </summary>
    /// <param name="target">被设置状态的目标</param>
    /// <param name="state"></param>
    /// <param name="player"></param>
    /// <param name="executeEnter"></param>
    /// <param name="executeExit"></param>
    public void TransitionOtherState(PlayerBaseState target, PlayerBaseState state, PlayerFSMSystem player,
        bool executeEnter, bool executeExit)
    {
        target.TransitionState(state, player, executeEnter, executeExit);
    }

    public abstract string name { get; }


    public abstract void Update(PlayerFSMSystem player);

    public abstract void FixedUpdate(PlayerFSMSystem player);

    /// <summary>
    /// 新状态的入口点，例如切换新状态前需要修改角色的动画
    /// </summary>
    /// <param name="player">可能为空</param>
    public virtual void Enter(PlayerFSMSystem player)
    {
    }

    /// <summary>
    /// 在离开现有状态，转换到新状态之前调用的方法。
    /// </summary>
    /// <param name="player">可能为空</param>
    public virtual void Exit(PlayerFSMSystem player)
    {
    }

    /// <summary>
    /// 取得当前的状态
    /// </summary>
    /// <param name="state"></param>
    /// <returns></returns>
    public string GetCurrentStateName(PlayerBaseState state)
    {
        var str = "";
        var i = 0;
        while (i < 10) // 防止死循环,最大遍历 10 次
        {
            i++;
            if (state != null)
            {
                str += state.name + "==>";
                state = state.currentState;
                continue;
            }

            break;
        }

        return str + i;
    }
}