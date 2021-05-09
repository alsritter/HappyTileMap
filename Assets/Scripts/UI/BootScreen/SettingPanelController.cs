using System.Collections;
using System.Collections.Generic;
using AlsRitter.UIFrame;
using UnityEngine;

namespace AlsRitter.UIFrame.Controller
{
    public class SettingPanelController : BasePanel
    {
        public override UIPanelType uiType => UIPanelType.SettingPanel;

        public override void DidOnClick(GameObject sender)
        {
            if (IsPause) return;

            switch (sender.name)
            {
                case "Close Button":
                    UIManager.instance.PopPanel();
                    break;
                default:
                    break;
            }
        }
    }
}