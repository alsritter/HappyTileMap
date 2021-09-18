using UnityEngine;

namespace AlsRitter.Global.Store.Player.Model {
    /**
     * 存储输入
     */
    public class PlayerInputModel : MonoBehaviour {
        public float v = 0;
        public float h = 0;

        [Header("控制是否使用自定义按键")]
        public bool keyIsSet;


        [Header("左移动")]
        public KeyCode leftMoveKeyCode;
        [Header("右移动")]
        public KeyCode rightMoveKeyCode;
        [Header("跳跃按键")]
        public KeyCode jumpKeyCode;

        [Header("下蹲按键")]
        public KeyCode crouchKeyCode;
        [Header("加速按键")]
        public KeyCode runKeyCode;

        [SerializeField]
        public float moveStartTime;
        [SerializeField]
        public float moveEndTime;
        


        // 当前移动的方向
        public int moveDir;

        private int jumpFrame;
        private int crouchFrame;

        public bool JumpKey   => Input.GetKey(jumpKeyCode);
        /**
         * 如果在落地前几帧之内按下跳跃键，游戏应该要记住指令使角色仍能在落地后跳跃，这会让角色跳跃的更加顺滑。
         * 为了精确到帧的控制，可以通过把一个 int 变量放在 FixedUpdate 里每帧减 1 来控制。
         * 在按下跳跃键时开启一个帧数倒计时，如果落地后该 int 变量还不为 0 则仍然返回 true，即按下跳跃按键。
         */
        public bool JumpKeyDown {
            get {
                if (Input.GetKeyDown(jumpKeyCode)) {
                    return true;
                }
                else if (jumpFrame > 0) {
                    return true;
                }

                return false;
            }
        }
        public bool JumpKeyUp => Input.GetKeyUp(jumpKeyCode);

        /**
         * 按下下蹲按钮，在下蹲的3帧内无法下蹲
         */
        public bool CrouchKeyDown => crouchFrame <= 0 && Input.GetKeyDown(jumpKeyCode);
        public bool CrouchKeyUp => Input.GetKeyUp(crouchKeyCode);
        public bool CrouchKey   => Input.GetKey(crouchKeyCode);

        public bool RunKey     => Input.GetKey(runKeyCode);
        public bool RunKeyUp   => Input.GetKeyUp(runKeyCode);
        public bool RunKeyDown => Input.GetKeyDown(runKeyCode);
        
        
        private void Awake() {
            KeyInit();
        }

        private void KeyInit() {
            if (keyIsSet) return;
            jumpKeyCode = KeyCode.J;
            runKeyCode = KeyCode.LeftShift;
            leftMoveKeyCode = KeyCode.A;
            rightMoveKeyCode = KeyCode.D;
            crouchKeyCode = KeyCode.S;
        }

        private void FixedUpdate() {
            if (jumpFrame >= 0) {
                jumpFrame--;
            }

            if (crouchFrame >= 0) {
                crouchFrame--;
            }
        }

        private void Update() {
            CheckHorizontalMove();
            v = Input.GetAxisRaw("Vertical");
            h = Input.GetAxisRaw("Horizontal");

            if (Input.GetKeyDown(jumpKeyCode)) {
                jumpFrame = 3; //在落地前3帧按起跳仍然能跳
            }

            if (Input.GetKeyDown(crouchKeyCode)) {
                crouchFrame = 3; //在下蹲的3帧内无法下蹲
            }
        }

        private void CheckHorizontalMove() {
            if (Input.GetKeyDown(rightMoveKeyCode) && h <= 0) {
                moveDir = 1;
                moveStartTime = Time.time;
                moveEndTime = 0;
            }
            else if (Input.GetKeyDown(leftMoveKeyCode) && h >= 0) {
                moveDir = -1;
                moveStartTime = Time.time;
                moveEndTime = 0;
            }
            else if (Input.GetKeyUp(rightMoveKeyCode)) {
                if (Input.GetKey(leftMoveKeyCode)) //放开右键的时候仍按着左键
                {
                    moveDir = -1;
                    moveStartTime = Time.time;
                    moveEndTime = 0;
                }
                else {
                    moveDir = 0;
                    moveEndTime = Time.time;
                }
            }
            else if (Input.GetKeyUp(leftMoveKeyCode)) {
                if (Input.GetKey(rightMoveKeyCode)) {
                    moveDir = 1;
                    moveStartTime = Time.time;
                    moveEndTime = 0;
                }
                else {
                    moveDir = 0;
                    moveEndTime = Time.time;
                }
            }
        }
    }
}