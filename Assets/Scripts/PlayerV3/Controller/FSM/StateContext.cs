using AlsRitter.Global.Store.Player;
using UnityEngine.SocialPlatforms;

namespace AlsRitter.V3.PlayerController.FSM {
    
    /**
     * 状态的上下文，主要用于调度状态
     */
    public class StateContext {
        private IBaseState _currentState;
        private UseStore   _useStore; // 把耦合转移到这个全局对象上来

        public StateContext(IBaseState state, UseStore useStore) {
            _currentState = state;
            _useStore = useStore;
            state.Enter(_useStore);
        }
        
        /**
         * 切换状态
         */
        public void TransitionState(IBaseState state) {
            // 避免重复转移状态
            if (state == _currentState) return;

            _currentState.Exit(_useStore);
            _currentState = state;
            state.Enter(_useStore);
        }

        public void UpdateHandle() {
            _currentState.UpdateHandle(_useStore);
        }

        public void FixedUpdateHandle() {
            _currentState.FixedUpdateHandle(_useStore);
        }
    }
}