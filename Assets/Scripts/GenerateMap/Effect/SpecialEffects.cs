using System.Collections;
using System.Collections.Generic;
using AlsRitter.EventFrame;
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

    [EffectInfo("游戏胜利", 1, "alsritter")]
    public class GameVictoryEffect : BaseObjectEffect
    {
        private readonly EventData harmEvent;
        private bool flag = true;

        public GameVictoryEffect()
        {
            harmEvent = EventData.CreateEvent(EventID.Win);
        }

        public override void ApplyTo(PlayerFSMSystem player)
        {
            // 只执行一次
            if (!flag) return;

            flag = false;
            harmEvent.Send();
        }

        public override int versionUID => 1;
        public override string name => "GameVictoryEffect";
    }
}