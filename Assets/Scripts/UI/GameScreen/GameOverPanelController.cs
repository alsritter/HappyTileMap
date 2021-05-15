using System.Collections;
using System.Collections.Generic;
using AlsRitter.EventFrame;
using AlsRitter.UIFrame;
using UnityEngine;

namespace AlsRitter.UIFrame.Controller
{
    public class GameOverPanelController : BasePanel
    {
        public override UIPanelType uiType => UIPanelType.GameOverPanel;

        /// <summary>
        /// 不需要特效
        /// </summary>
        public override void OnEnter()
        {
            gameObject.SetActive(true);
        }

        public override void DidOnClick(GameObject sender)
        {
            if (IsPause) return;
            switch (sender.name)
            {
                case "RetryButton":

                    break;
                case "BreakButton":

                    break;
                default:
                    break;
            }
        }
    }
}
