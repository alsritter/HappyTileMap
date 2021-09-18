using System.Collections;
using System.Collections.Generic;
using AlsRitter.Global.Store.Player;
using AlsRitter.Global.Store.Player.Model;
using UnityEngine;

namespace AlsRitter.V3.PlayerController.FSM
{
    /// <summary>
    /// 下蹲状态走路
    /// </summary>
    public class CrouchWalkState : IBaseState
    {
        public string name => "CrouchWalkState";
        public void UpdateHandle(UseStore useStore) {
            
        }

        public void FixedUpdateHandle(UseStore useStore) {
            HorizontalMove(useStore.inputModel, useStore.basicModel, useStore.basicModel.crouchSpeed);
            var rig = useStore.basicModel.rb;
            rig.MovePosition(rig.transform.position + useStore.basicModel.moveSpeed * Time.fixedDeltaTime);
        }

        public void Enter(UseStore useStore) {
            
        }

        public void Exit(UseStore useStore) {
            
        }

        
        /// <summary>
        /// 横向移动
        /// </summary>
        private void HorizontalMove(PlayerInputModel input, PlayerBasicModel basic, float speed) {
            //减速速阶段（反向移动时需要先减速）
            if ((basic.moveSpeed.x > 0 && input.moveDir == -1) || (basic.moveSpeed.x < 0 && input.moveDir == 1) ||
                input.moveDir == 0 || (input.v < 0) || Mathf.Abs(basic.moveSpeed.x) > speed) {
                
                basic.introDir = basic.moveSpeed.x > 0 ? 1 : -1;
                basic.moveHSpeed = Mathf.Abs(basic.moveSpeed.x);

                basic.moveHSpeed -= speed / 3;

                if (basic.moveHSpeed < 0.01f) {
                    basic.moveHSpeed = 0;
                }

                basic.moveSpeed.x = basic.moveHSpeed * basic.introDir;
            }
            else {
                if (input.moveDir == 1 && !(input.v < 0)) {
                    basic.moveSpeed.x += speed / 6;

                    if (basic.moveSpeed.x > speed)
                        basic.moveSpeed.x = speed;
                }
                else if (input.moveDir == -1 && !(input.v < 0)) {
                    basic.moveSpeed.x -= speed / 6;
                    if (basic.moveSpeed.x < -speed)
                        basic.moveSpeed.x = -speed;
                } 
            }
        }
    }
}