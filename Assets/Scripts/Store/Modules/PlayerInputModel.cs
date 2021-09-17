using UnityEngine;

namespace AlsRitter.Store.Model {
    /**
     * 存储输入
     */
    public class PlayerInputModel : MonoBehaviour {
        
        public float v = 0;
        public float h = 0;
        
        [Header("控制是否使用自定义按键")]
        public bool keyIsSet;

        
        [Header("左移动")]
        public KeyCode leftMoveKey;
        [Header("右移动")]
        public KeyCode rightMoveKey;
        [Header("跳跃按键")]
        public KeyCode jump;
        [Header("冲刺按键")]
        public KeyCode dash;
        [Header("爬墙按键")]
        public KeyCode climb;
        
        public bool ClimbKey => Input.GetKey(climb);
        public bool ClimbKeyDown => Input.GetKeyDown(climb);
        public bool ClimbKeyUp => Input.GetKeyUp(climb);
        public bool JumpKey => Input.GetKey(jump);

        /**
         * 如果在落地前几帧之内按下跳跃键，游戏应该要记住指令使角色仍能在落地后跳跃，这会让角色跳跃的更加顺滑。
         * 为了精确到帧的控制，可以通过把一个 int 变量放在 FixedUpdate 里每帧减 1 来控制。
         * 在按下跳跃键时开启一个帧数倒计时，如果落地后该 int 变量还不为 0 则仍然返回 true，即按下跳跃按键。
         */
        public bool JumpKeyDown {
            get {
                if (Input.GetKeyDown(jump)) {
                    return true;
                }
                else if (JumpFrame > 0) {
                    return true;
                }

                return false;
            }
        }
        [HideInInspector]
        public bool JumpKeyUp => Input.GetKeyUp(jump);
        [HideInInspector]
        public bool DashKey => Input.GetKey(dash);
        [HideInInspector]
        public bool DashKeyDown => Input.GetKeyDown(dash);
        [HideInInspector]
        public bool DashKeyUp => Input.GetKeyUp(dash);


        [SerializeField]
        float MoveStartTime;
        [SerializeField]
        float MoveEndTime;
        
        public int MoveDir;

        int JumpFrame;

        private void Awake() {
            KeyInit();
        }

        private void KeyInit() {
            if (keyIsSet) return;
            jump = KeyCode.K;
            dash = KeyCode.J;
            climb = KeyCode.L;
            leftMoveKey = KeyCode.A;
            rightMoveKey = KeyCode.D;
        }

        private void FixedUpdate() {
            if (JumpFrame >= 0) {
                JumpFrame--;
            }
        }

        private void Update() {
            CheckHorizontalMove();
            v = Input.GetAxisRaw("Vertical");
            h = Input.GetAxisRaw("Horizontal");
            if (Input.GetKeyDown(jump)) {
                JumpFrame = 3; //在落地前3帧按起跳仍然能跳
            }
        }

        private void CheckHorizontalMove() {
            if (Input.GetKeyDown(rightMoveKey) && h <= 0) {
                MoveDir = 1;
            }
            else if (Input.GetKeyDown(leftMoveKey) && h >= 0) {
                MoveDir = -1;
            }
            else if (Input.GetKeyUp(rightMoveKey)) {
                if (Input.GetKey(leftMoveKey)) //放开右键的时候仍按着左键
                {
                    MoveDir = -1;
                    MoveStartTime = Time.time;
                }
                else {
                    MoveDir = 0;
                }
            }
            else if (Input.GetKeyUp(leftMoveKey)) {
                if (Input.GetKey(rightMoveKey)) {
                    MoveDir = 1;
                }
                else {
                    MoveDir = 0;
                }
            }
        }
    }
}