using System.Collections;
using System.Collections.Generic;
using AlsRitter.UIFrame;
using UnityEngine;

namespace AlsRitter.UIFrame.Controller
{
    public class StoryModePanelController : BasePanel
    {
        public override UIPanelType uiType => UIPanelType.StoryModePanel;

        public override void DidOnClick(GameObject sender)
        {
            if (IsPause) return;

            switch (sender.name)
            {
                case "Return Button":
                    UIManager.instance.PopPanel();
                    break;
                default:
                    break;
            }
        }
    }
}