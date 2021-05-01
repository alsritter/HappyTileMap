using System.Collections;
using System.Collections.Generic;
using EffectDocumentTools.EffectDocumentAttributeNamespace;
using UnityEngine;

namespace PhysicsEffects
{
    [EffectInfo("蹦床效果", 1, "alsritter")]
    public class TrampolineEffect : BaseObjectEffect
    {
        private readonly float jumpForce;
        
        /// <summary>
        /// 蹦床效果
        /// </summary>
        /// <param name="jumpForce">跳跃的力</param>
        public TrampolineEffect(float jumpForce)
        {
            this.jumpForce = jumpForce;
        }

        public override void ApplyTo(PlayerFSMSystem player)
        {
            if (player != null)
            {
                player.rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
            }
        }

        public override int versionUID => 1;

        public override string name => "TrampolineEffect";
    }

    [EffectInfo("传送带效果", 1, "alsritter")]
    public class ConveyorEffect : BaseObjectEffect
    {
        private readonly bool isLeft;
        private readonly float speed;

        /// <summary>
        /// 传送带效果
        /// </summary>
        /// <param name="isLeft">是否向左（false 为右）</param>
        /// <param name="speed">传送带速度</param>
        public ConveyorEffect(bool isLeft, float speed)
        {
            this.isLeft = isLeft;
            this.speed = speed;
        }

        public override void ApplyTo(PlayerFSMSystem player)
        {
            if (player != null)
            {
                player.rb.AddForce(new Vector2(speed * (isLeft ? -1 : 1), 0), ForceMode2D.Force);
            }
        }

        public override int versionUID => 1;
        public override string name => "ConveyorEffect";
    }
}