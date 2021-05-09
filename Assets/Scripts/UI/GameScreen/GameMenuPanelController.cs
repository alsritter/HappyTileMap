﻿using System.Collections;
using System.Collections.Generic;
using AlsRitter.UIFrame;
using UnityEngine;

namespace AlsRitter.UIFrame.Controller
{
    public class GameMenuPanelController : BasePanel
    {
        public override UIPanelType uiType => UIPanelType.GameMenuPanel;

        public override void DidOnClick(GameObject sender)
        {
            if (IsPause) return;

            switch (sender.name)
            {
                case "CloseButton":
                    PanelManager.instance.PopPanel();
                    break;
                default:
                    break;
            }
        }
    }
}