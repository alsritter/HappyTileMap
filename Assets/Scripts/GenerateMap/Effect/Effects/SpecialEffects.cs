using AlsRitter.EventFrame;
using AlsRitter.V3.Player;

namespace AlsRitter.V3.CustomTileFrame.TileEffect.SpecialEffects {
    [EffectInfo("空效果", 1, "alsritter")]
    public class EmptyEffect : IBaseEffect {
        public int    versionUID => 1;
        public string name       => "EmptyEffect";

        public void ApplyTo(IPlayer player) {
        }
    }

    [EffectInfo("游戏胜利", 1, "alsritter")]
    public class GameVictoryEffect : IBaseEffect, IEventObserver {
        public int    versionUID => 1;
        public string name       => "GameVictoryEffect";

        private readonly EventData winEvent;
        private          bool      flag = true;

        public GameVictoryEffect() {
            winEvent = EventData.CreateEvent(EventID.Win);
            EventManager.Register(this, EventID.ResetGame, EventID.ReturnMenu);
        }

        public void ApplyTo(IPlayer player) {
            // 只执行一次
            if (!flag) return;
            //Debug.Log("发送了");
            flag = false;
            winEvent.Send();
        }
        
        public void HandleEvent(EventData resp) {
            //Debug.Log("重置了");
            // 收到重新开始信号需要重置 flag
            switch (resp.eid) {
                case EventID.ResetGame:
                    flag = true;
                    break;
                case EventID.ReturnMenu:
                    flag = true;
                    break;
            }
        }

        ~GameVictoryEffect() {
            EventManager.Remove(this);
        }
    }
}