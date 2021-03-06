using AlsRitter.Global.Store.Player;
using AlsRitter.Global.Store.Player.Model;

namespace AlsRitter.V3.PlayerController.FSM {
    /// <summary>
    /// 在地面的状态
    /// </summary>
    public class OnGroundState : IBaseState {
        public string name => "OnGroundState";

        // 地面的状态
        private readonly StateContext stateContext;

        private readonly OnStandState  onStandState;

        public OnGroundState() {
            onStandState = new OnStandState();
            stateContext = new StateContext(onStandState, UseStore.GetStore());
        }

        public void UpdateHandle(UseStore useStore) {
            // // 一只脚着地无法蹲下
            // if (useStore.inputModel.CrouchKey && !useStore.stateModel.isExistTop) {
            //     if (!useStore.stateModel.isHalfFoot) {
            //         useStore.stateModel.isStand = false;
            //     }
            // }
            // else {
            //     useStore.stateModel.isStand = true;
            // }
            //
            //
            // if (!useStore.stateModel.isStand) {
            // }
            // else {
            //     stateContext.TransitionState(onStandState);
            // }


            stateContext.UpdateHandle();
        }

        public void FixedUpdateHandle(UseStore useStore) {
            stateContext.FixedUpdateHandle();
        }

        public void Enter(UseStore useStore) {
            stateContext.TransitionState(onStandState);
        }

        public void Exit(UseStore useStore) {
            stateContext.TransitionState(onStandState);
        }
    }
}