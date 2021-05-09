using System.Collections;
using System.Collections.Generic;
using AlsRitter.PlayerController.FSM;
using UnityEngine;

namespace AlsRitter.GenerateMap.CustomTileFrame.TileEffect.SpecialEffects
{
    [EffectInfo("空效果", 1, "alsritter")]
    public class EmptyEffect : BaseObjectEffect
    {
        public override void ApplyTo(PlayerFSMSystem player)
        {
        }

        public override int versionUID => 1;
        public override string name => "EmptyEffect";
    }
}
