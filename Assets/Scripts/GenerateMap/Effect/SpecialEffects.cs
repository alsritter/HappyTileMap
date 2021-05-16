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
    public class GameVictoryEffect : BaseObjectEffect, IEventObserver
    {
        private readonly EventData harmEvent;
        private bool flag = true;

        public GameVictoryEffect()
        {
            harmEvent = EventData.CreateEvent(EventID.Win);
            EventManager.Register(this, EventID.ResetGame, EventID.ReturnMenu);
        }

        public override void ApplyTo(PlayerFSMSystem player)
        {
            // 只执行一次
            if (!flag) return;
            Debug.Log("发送了");
            flag = false;
            harmEvent.Send();
        }

        public override int versionUID => 1;
        public override string name => "GameVictoryEffect";

        public void HandleEvent(EventData resp)
        {
            Debug.Log("重置了");
            // 收到重新开始信号需要重置 flag
            switch (resp.eid)
            {
                case EventID.ResetGame:
                    flag = true;
                    break;
                case EventID.ReturnMenu:
                    flag = true;
                    break;
            }
        }
    }
}