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
                    UIManager.instance.PushPanel(UIPanelType.SelectModePanel);
                    break;
                case "Setting Button":
                    UIManager.instance.PushPanel(UIPanelType.SettingPanel);
                    break;
                default:
                    break;
            }
        }
    }
}