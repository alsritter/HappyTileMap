using System;
using AlsRitter.EventFrame;
using AlsRitter.EventFrame.CustomEvent;
using AlsRitter.Global.Store.Player;
using AlsRitter.Global.Store.Player.Model;
using AlsRitter.PlayerController.FSM;
using UnityEngine;


namespace AlsRitter.V3.PlayerController {

    /**
     * Unity - Ray射线检测
     * https://blog.csdn.net/qq_28299311/article/details/103120607
     */
    [DisallowMultipleComponent]
    public class RayCheck : MonoBehaviour {
        [Header("当前需要检查的Layer")]
        public LayerMask groundLayer; // 当前需要检查的“地面”的 Layer

        private float footDistance; // 脚距离中心点的距离
        private float topDistance;

        private PlayerInputModel input;
        private PlayerBasicModel basic;
        private PlayerViewModel  view;
        private PlayerStateModel state;

        private void Awake() {
            input = UseStore.GetStore().inputModel;
            basic = UseStore.GetStore().basicModel;
            view = UseStore.GetStore().viewModel;
            state = UseStore.GetStore().stateModel;
        }


        // Start is called before the first frame update
        // private void Start() {
        //     playerLayerMask = LayerMask.GetMask("Player");
        //     playerLayerMask = ~playerLayerMask; //获得当前玩家层级的mask值，并使用~运算，让射线忽略玩家层检测
        // }

        private void Update() {
            RayCastBox();
            CheckDir();
            CheckHorizontalMove();
            CheckUpMove();
            // DebugBoxRay();
        }

        
        /**
         * Unity Debug Tool
         */
        private void OnDrawGizmos() {
            // 避免报错，运行时才执行
            if (!Application.isPlaying) return;
            
            //设置颜色
            // Gizmos.color = Color.yellow;
            //绘制射线
            // Gizmos.DrawRay (transform.position, transform.forward * distance);
            //绘制立方体线框
            // Gizmos.DrawWireCube (transform.position + transform.forward * distance, transform.localScale);
            
            if (view.HorizontalBox != null && view.HorizontalBox.Length > 0 && view.HorizontalBox[0]) {
                Gizmos.color = Color.yellow;
                Debug.DrawLine(view.Position, view.HorizontalBox[0].point, Color.yellow);
                Gizmos.DrawWireCube (view.Position + Vector3.right * basic.horizCheckDistance 
                                                                   * (state.playDir == PlayDir.Right ? 1 : -1), 
                                     view.boxSize);
            }
            
            if (view.UpBox != null && view.UpBox.Length > 0 && view.UpBox[0]) {
                Gizmos.color = Color.blue;
                Debug.DrawLine(view.Position, view.UpBox[0].point, Color.red);
                Gizmos.DrawWireCube (view.Position + Vector3.up * basic.upCheckDistance, view.boxSize);
            }

            if (view.DownBox.collider != null) {
                Gizmos.color = Color.cyan;
                Debug.DrawLine(view.Position, view.DownBox.point, Color.cyan);
                Gizmos.DrawWireCube (view.Position + Vector3.down * basic.downCheckDistance, view.boxSize);
            }
        }

        private void FixedUpdate() {
            FootRayCheck(); // 脚底检查
        }


        /**
         * 碰撞盒四个方向上的射线检查
         */
        private void RayCastBox() {
            // 只要在箱形投射的上方都能检测到
            view.RightBox =
                Physics2D.BoxCastAll(view.Position,
                                     view.boxSize, 0,
                                     Vector3.right,
                                     basic.horizCheckDistance,
                                     groundLayer);

            view.LeftBox =
                Physics2D.BoxCastAll(view.Position,
                                     view.boxSize, 0,
                                     Vector3.left,
                                     basic.horizCheckDistance,
                                     groundLayer);
            view.UpBox =
                Physics2D.BoxCastAll(view.Position,
                                     view.boxSize, 0,
                                     Vector3.up,
                                     basic.upCheckDistance,
                                     groundLayer);
            view.DownBox =
                Physics2D.BoxCast(view.Position,
                                  view.boxSize, 0,
                                  Vector3.down,
                                  basic.downCheckDistance,
                                  groundLayer);
        }


        /**
         * 检查方向
         */
        private void CheckDir() {
            state.lastDir = state.playDir;
            if (input.moveDir > 0) {
                state.playDir = PlayDir.Right;
            }
            else if (input.moveDir < 0) {
                state.playDir = PlayDir.Left;
            }
        }

        /**
         * 检测水平方向位移：
         */
        private void CheckHorizontalMove() {
            view.HorizontalBox = state.playDir == PlayDir.Right ? view.RightBox : view.LeftBox;
            if (view.HorizontalBox.Length != 1) return; // 判断是否碰撞了

            if (!view.HorizontalBox[0].collider.CompareTag("Trap")) //如果左右不是陷阱
            {
                // Do Some thing
            }
            else {
                // Die();
            }
        }

        /**
         * 检测并修正垂直方向的位移
         */
        private void CheckUpMove() {
            state.isGround = view.DownBox.collider != null;

            if (view.UpBox.Length != 1) {
                state.isExistTop = false;
                return;
            }

            // 检测是否碰到头了
            state.isExistTop = true;
        }

        /**
         * 脚底两个射线检查
         */
        private void FootRayCheck() {
            // 每帧更新长度
            footDistance = Mathf.Abs(view.leftFoot.transform.position.y - view.Position.y);

            Vector2 leftPos = view.leftFoot.transform.position;
            Vector2 rightPos = view.rightFoot.transform.position;

            var leftCheck =
                Physics2D.Raycast(leftPos, Vector2.up, footDistance, groundLayer);

            var rightCheck =
                Physics2D.Raycast(rightPos, Vector2.up, footDistance, groundLayer);

            Debug.DrawRay(leftPos, Vector2.up * footDistance, leftCheck ? Color.red : Color.green);
            Debug.DrawRay(rightPos, Vector2.up * footDistance, rightCheck ? Color.red : Color.green);

            // 单脚着地
            var temp3 = (leftCheck && !rightCheck) || (!leftCheck && rightCheck);

            state.isHalfFoot = temp3;
        }

    }
}