using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace PlayerController.FSM
{
    /// <summary>
    /// 在空中的状态
    /// </summary>
    public class InTheAirState : PlayerBaseState
    {
        // 空中的状态
        public readonly JumpState jumpState; // 初始状态
        public readonly Jump2State jump2State;

        public override string name => "InTheAirState";

        /// <summary>
        /// 在入口点重置当前状态
        /// </summary>
        /// <param name="player"></param>
        public override void Enter(PlayerFSMSystem player)
        {
            // 只有按下 Jump 进入的状态才需要进入跳跃（注意这个 GetButtonDown 是获取的同一帧的是否按下）
            TransitionState(jumpState, player, Input.GetButtonDown("Jump"), true);
            player.inTheAir = true;
            // 跳跃时无摩擦
            player.rb.sharedMaterial = player.noFriction;
        }

        public override void Update(PlayerFSMSystem player)
        {
            currentState.Update(player);
        }

        public override void FixedUpdate(PlayerFSMSystem player)
        {
            // 在空中也可以移动
            player.rb.velocity =
                new Vector2(player.xVelocity * (player.speed / player.jumpSleepDivisor),
                    player.rb.velocity.y);

            currentState.FixedUpdate(player);
        }

        public InTheAirState()
        {
            jumpState = new JumpState(this);
            jump2State = new Jump2State(this);
            TransitionState(jumpState, null);
        }

        public override void Exit(PlayerFSMSystem player)
        {
            currentState.Exit(player);
            player.inTheAir = false;
            // 站在时有摩擦
            player.rb.sharedMaterial = player.hasFriction;
        }
    }
}