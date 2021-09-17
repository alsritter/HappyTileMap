using AlsRitter.Store.Model;
using AlsRitter.Utilities;
using UnityEngine;

namespace AlsRitter.GlobalControl.Store {
    /**
     * 用于取得全局数据的工具
     */
    [RequireComponent(typeof(PlayerBasicModel))]
    [RequireComponent(typeof(PlayerViewModel))]
    [RequireComponent(typeof(PlayerStateModel))]
    [RequireComponent(typeof(PlayerInputModel))]
    [DisallowMultipleComponent]
    public class UseStore : MonoBehaviour {
        private static UseStore _instance;

        public PlayerBasicModel basicModel { get; private set; }
        public PlayerStateModel stateModel { get; private set; }
        public PlayerViewModel  viewModel  { get; private set; }
        public PlayerInputModel inputModel { get; private set; }
        
        /**
         * 需要在这里初始化数据
         */
        private void Awake() {
            if (_instance != null) {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            // DontDestroyOnLoad(gameObject);

            basicModel = GetComponent<PlayerBasicModel>();
            stateModel = GetComponent<PlayerStateModel>();
            viewModel = GetComponent<PlayerViewModel>();
            inputModel = GetComponent<PlayerInputModel>();
        }

        /**
         * 取得当前实例
         */
        public static UseStore GetStore() {
            return _instance;
        }
    }
}