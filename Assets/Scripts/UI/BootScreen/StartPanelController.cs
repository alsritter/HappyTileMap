using System.Collections;
using System.Collections.Generic;
using AlsRitter.UIFrame;
using UnityEngine;

namespace AlsRitter.UIFrame.Controller
{
    public class StartPanelController : BasePanel
    {
        public override UIPanelType uiType => UIPanelType.StartPanel;
        
        public override void DidOnClick(GameObject sender)
        {
            if (IsPause) return;

            switch (sender.name)
            {
                case "Start Game Button":
                    // Debug.Log("游戏开始了~");
                    PanelManager.instance.PushPanel(UIPanelType.SelectMapPanel);
                    break;
                case "UserArea":
                    PanelManager.instance.PushPanel(UIPanelType.LoginPanel);
                    break;
                default:
                    break;
            }
        }
    }
}