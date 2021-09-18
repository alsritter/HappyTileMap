using UnityEngine;

namespace AlsRitter.Global.Store.Player.Model {
    /**
     * 角色资源的引用：角色模型，动画，特性，音效等待
     * 
     * 角色的四肢
     */
    public class PlayerViewModel : MonoBehaviour {
        public Animator      playAnimator;
        public BoxCollider2D boxCollider2D;

        [Header("角色身体部件")]
        public GameObject rightFoot;
        public GameObject leftFoot;

        [Header("身体中心点")]
        public GameObject bodyCentre; // 身体中心点
        
        public Vector2 boxSize;

        public Vector3 Position => bodyCentre.transform.position;
        // public Vector3 Position => boxCollider2D.transform.position;

        // 用于解决卡墙的问题
        // 参考资料：Unity 横版2D移动跳跃问题——关于一段跳与二段跳 https://www.cnblogs.com/AMzz/p/11802502.html
        [Header("物理")]
        public PhysicsMaterial2D hasFriction; //有摩擦力的
        public PhysicsMaterial2D noFriction; //无摩擦力的

        public RaycastHit2D DownBox;

        public RaycastHit2D[] UpBox;
        public RaycastHit2D[] RightBox;
        public RaycastHit2D[] LeftBox;

        public RaycastHit2D[] HorizontalBox;
        
        private void Start() {
            boxCollider2D = GetComponent<BoxCollider2D>();
            //设置盒子射线的大小
            boxSize = boxCollider2D.size * 0.9f;
        }
    }
}