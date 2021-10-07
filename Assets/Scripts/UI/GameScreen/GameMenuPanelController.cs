using System.Collections;
using System.Collections.Generic;
using AlsRitter.EventFrame;
using AlsRitter.UIFrame;
using UnityEngine;

namespace AlsRitter.UIFrame.Controller {
    public class GameMenuPanelController : BasePanel {
        public override UIPanelType uiType => UIPanelType.GameMenuPanel;

        private readonly EventData resetEvent;

        public GameMenuPanelController() {
            resetEvent = EventData.CreateEvent(EventID.ResetGame);
        }


        public override void DidOnClick(GameObject sender) {
            if (IsPause) return;

            switch (sender.name) {
                case "CloseButton":
                    PanelManager.instance.PopPanel();
                    break;
                case "RetryButton":
                    resetEvent.Send();
                    PanelManager.instance.PopPanel();
                    break;
            }
        }
    }
}