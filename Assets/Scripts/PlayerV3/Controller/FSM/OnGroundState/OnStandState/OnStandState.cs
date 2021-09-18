using AlsRitter.Global.Store.Player;
using AlsRitter.Global.Store.Player.Model;

namespace AlsRitter.V3.PlayerController.FSM {
    /// <summary>
    /// 站立状态
    /// </summary>
    public class OnStandState : IBaseState {
        public string name => "OnStandState";

        // 站起的状态
        private readonly WalkState walkState;
        private readonly IdleState idleState; // 初始状态
        private readonly RunState  runState;

        private readonly StateContext stateContext;

        public OnStandState() {
            idleState = new IdleState();
            runState = new RunState();
            walkState = new WalkState();
            stateContext = new StateContext(idleState, UseStore.GetStore());
        }


        public void UpdateHandle(UseStore useStore) {
            if (useStore.inputModel.moveDir != 0) {
                if (!useStore.inputModel.RunKey) {
                    useStore.stateModel.playState = PlayState.Normal;
                    stateContext.TransitionState(walkState);
                }
                else {
                    useStore.stateModel.playState = PlayState.Run;
                    stateContext.TransitionState(runState);
                }
            }
            else {
                useStore.stateModel.playState = PlayState.Normal;
                stateContext.TransitionState(idleState);
            }
            
            stateContext.UpdateHandle();
        }

        public void FixedUpdateHandle(UseStore useStore) {
            stateContext.FixedUpdateHandle();
        }

        public void Enter(UseStore useStore) {
            stateContext.TransitionState(idleState);
        }

        public void Exit(UseStore useStore) {
        }
    }
}