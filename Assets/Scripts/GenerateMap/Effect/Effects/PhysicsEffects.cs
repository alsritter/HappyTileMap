using System.Collections;
using System.Collections.Generic;
using AlsRitter.PlayerController.FSM;
using UnityEngine;

namespace AlsRitter.V3.CustomTileFrame.TileEffect.PhysicsEffects {
    [EffectInfo("蹦床效果", 1, "alsritter")]
    public class TrampolineEffect : IBaseEffect {
        public           int    versionUID => 1;
        public           string name       => "TrampolineEffect";
        private readonly float  jumpForce;

        /// <summary>
        /// 蹦床效果
        /// </summary>
        /// <param name="jumpForce">跳跃的力</param>
        public TrampolineEffect(float jumpForce) {
            this.jumpForce = jumpForce;
        }

        public void ApplyTo(IPlayer player) {
            if (player != null) {
                // player.rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
                player.Jump(jumpForce);
            }
        }
    }

    [EffectInfo("传送带效果", 1, "alsritter")]
    public class ConveyorEffect : IBaseEffect {
        public           int    versionUID => 1;
        public           string name       => "ConveyorEffect";
        private readonly bool   isLeft;
        private readonly float  speed;

        /// <summary>
        /// 传送带效果
        /// </summary>
        /// <param name="isLeft">是否向左（false 为右）</param>
        public ConveyorEffect(bool isLeft) {
            this.isLeft = isLeft;
            this.speed = speed;
        }

        public void ApplyTo(IPlayer player) {
            if (player != null) {
                if (isLeft) {
                    player.MoveLeft();
                }
                else {
                    player.MoveRight();
                }
                
            }
        }
    }
}