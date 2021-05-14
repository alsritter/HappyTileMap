using System.Collections;
using System.Collections.Generic;
using AlsRitter.UIFrame;
using UnityEngine;

namespace AlsRitter.UIFrame.Controller
{
    public class SelectMapPanelController : BasePanel
    {
        public override UIPanelType uiType => UIPanelType.SelectMapPanel;

        public override void DidOnClick(GameObject sender)
        {
            // 如果暂停了则不再执行按钮操作
            if (IsPause) return;

            switch (sender.name)
            {
                case "Story Mode Button":
                    PanelManager.instance.PushPanel(UIPanelType.SelectMapPanel);
                    break;
                case "Return Button":
                    PanelManager.instance.PopPanel();
                    break;
                default:
                    break;
            }
        }
    }
}