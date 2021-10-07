using System.Collections;
using System.Collections.Generic;
using AlsRitter.EventFrame;
using AlsRitter.UIFrame;
using UnityEngine;

namespace AlsRitter.UIFrame.Controller {
    public class GameWinPanelController : BasePanel {
        public override UIPanelType uiType => UIPanelType.GameWinPanel;

        private readonly EventData resetEvent;
        private readonly EventData returnMenuEvent;

        public GameWinPanelController() {
            resetEvent = EventData.CreateEvent(EventID.ResetGame);
        }

        /// <summary>
        /// 不需要特效
        /// </summary>
        public override void OnEnter() {
            gameObject.SetActive(true);
        }

        public override void OnExit() {
            gameObject.SetActive(false);
        }

        public override void DidOnClick(GameObject sender) {
            if (IsPause) return;
            switch (sender.name) {
                case "RetryButton":
                    PanelManager.instance.PopPanel();
                    resetEvent.Send();
                    break;
            }
        }
    }
}