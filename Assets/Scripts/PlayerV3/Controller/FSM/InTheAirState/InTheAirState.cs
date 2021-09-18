using System.Collections;
using System.Collections.Generic;
using AlsRitter.EventFrame;
using AlsRitter.EventFrame.CustomEvent;
using AlsRitter.Global.Store.Player;
using AlsRitter.Global.Store.Player.Model;
using UnityEngine;
using UnityEngine.UIElements;

namespace AlsRitter.V3.PlayerController.FSM {
    /// <summary>
    /// 在空中的状态
    /// </summary>
    public class InTheAirState : IBaseState {
        public string name => "InTheAirState";

        // 空中的状态
        private readonly JumpState    jumpState; // 初始状态
        private readonly FallState    fallState;
        private readonly StateContext stateContext;

        public InTheAirState() {
            jumpState = new JumpState();
            fallState = new FallState();
            stateContext = new StateContext(fallState, UseStore.GetStore());
        }


        public void UpdateHandle(UseStore useStore) {
            if (useStore.stateModel.playState == PlayState.Jump) {
                stateContext.TransitionState(jumpState);
            }
            else if (useStore.stateModel.playState == PlayState.Fall) {
                stateContext.TransitionState(fallState);
            }

            stateContext.UpdateHandle();
        }

        public void FixedUpdateHandle(UseStore useStore) {
            stateContext.FixedUpdateHandle();
        }

        public void Enter(UseStore useStore) {
            stateContext.TransitionState(fallState);
        }

        public void Exit(UseStore useStore) {
        }
    }
}