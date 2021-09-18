using AlsRitter.Global.Store.Player;

namespace AlsRitter.V3.PlayerController.FSM {
    public class WalkState : IBaseState {
        public string name => "WalkState";

        public void UpdateHandle(UseStore useStore) {
        }

        public void FixedUpdateHandle(UseStore useStore) {
        }

        public void Enter(UseStore useStore) {
            useStore.basicModel.currentSpeed = useStore.basicModel.speed;
        }

        public void Exit(UseStore useStore) {
        }
    }
}