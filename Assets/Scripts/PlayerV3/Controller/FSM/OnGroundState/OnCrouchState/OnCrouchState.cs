using AlsRitter.Global.Store.Player;

namespace AlsRitter.V3.PlayerController.FSM {
    /// <summary>
    /// 下蹲状态
    /// </summary>
    public class OnCrouchState : IBaseState {
        public string name => "OnCrouchState";

        // 蹲下的状态
        private readonly CrouchWalkState crouchWalkState;
        private readonly CrouchIdleState crouchIdleState; // 初始状态
        private readonly StateContext    stateContext;


        public OnCrouchState() {
            crouchWalkState = new CrouchWalkState();
            crouchIdleState = new CrouchIdleState();
            stateContext = new StateContext(crouchIdleState, UseStore.GetStore());
        }


        public void UpdateHandle(UseStore useStore) {
            if (useStore.inputModel.moveDir != 0) {
                stateContext.TransitionState(crouchWalkState);
            }
            else {
                stateContext.TransitionState(crouchIdleState);
            }

            stateContext.UpdateHandle();
        }

        public void FixedUpdateHandle(UseStore useStore) {
            stateContext.FixedUpdateHandle();
        }

        public void Enter(UseStore useStore) {
            // TODO: 这里改变碰撞盒大小
            stateContext.TransitionState(crouchIdleState);
        }

        public void Exit(UseStore useStore) {
        }
    }
}