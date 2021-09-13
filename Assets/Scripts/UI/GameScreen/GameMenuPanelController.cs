using System.Collections;
using System.Collections.Generic;
using AlsRitter.EventFrame;
using AlsRitter.UIFrame;
using UnityEngine;

namespace AlsRitter.UIFrame.Controller
{
    public class GameMenuPanelController : BasePanel
    {

        private readonly EventData resetEvent;
        private readonly EventData returnMenuEvent;

        public GameMenuPanelController()
        {
            resetEvent = EventData.CreateEvent(EventID.ResetGame);
            returnMenuEvent = EventData.CreateEvent(EventID.ReturnMenu);
        }

        public override UIPanelType uiType => UIPanelType.GameMenuPanel;

        public override void DidOnClick(GameObject sender)
        {
            if (IsPause) return;

            switch (sender.name)
            {
                case "CloseButton":
                    PanelManager.instance.PopPanel();
                    break;
                case "RetryButton":
                    PanelManager.instance.PopPanel();
                    resetEvent.Send();
                    break;
                case "BreakButton":
                    returnMenuEvent.Send();
                    break;
            }
        }
    }
}