using System.Collections;
using System.Collections.Generic;
using AlsRitter.EventFrame;
using AlsRitter.UIFrame;
using UnityEngine;

namespace AlsRitter.UIFrame.Controller {
    public class GameOverPanelController : BasePanel {
        public override UIPanelType uiType => UIPanelType.GameOverPanel;

        private readonly EventData resetEvent;

        public GameOverPanelController() {
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
                    
                    Debug.Log("VAR");
                    
                    resetEvent.Send();
                    PanelManager.instance.PopPanel();
                    break;
            }
        }
    }
}