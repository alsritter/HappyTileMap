using AlsRitter.Global.Store.Player;
using AlsRitter.Utilities;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace AlsRitter.V3.Player {
    /**
     * 角色的实现类
     */
    public class GlobalPlayer : Singleton<GlobalPlayer>, IPlayer {
        private GameObject player;

        /**
         * 返回自己
         */
        public static IPlayer GetPlayer() {
            return instance;
        }

        public override void AwakeInitInfo() {
            player = UseStore.GetStore().transform.gameObject;
        }

        public void MoveLeft() {
        }

        public void MoveRight() {
        }

        public void StopMove() {
            UseStore.GetStore().stateModel.isMove = false;
        }

        public void CanMove() {
            UseStore.GetStore().stateModel.isMove = true;
        }

        public void Jump(float jumpDynamics) {
        }

        public GameObject PlayerSelf() {
            return player;
        }

        public void SetSpeed(float speed) {
        }

        public void SetPos(Vector3 pos) {
            player.transform.position = pos;
        }

        public Vector3 GetPos() {
            return player.transform.position;
        }

        public SpriteRenderer GetSpriteRenderer() {
            return UseStore.GetStore().viewModel.spriteRenderer;
        }

        public Rigidbody2D GetRigidbody2D() {
            return UseStore.GetStore().basicModel.rb;
        }
    }
}