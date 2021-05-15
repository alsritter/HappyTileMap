using System.Collections;
using System.Collections.Generic;
using AlsRitter.UIFrame;
using UnityEngine;

namespace AlsRitter.UIFrame.Controller
{
    public class SelectMapPanelController : BasePanel
    {
        public override UIPanelType uiType => UIPanelType.SelectMapPanel;

        /// <summary>
        /// 不需要特效
        /// </summary>
        public override void OnEnter()
        {
            gameObject.SetActive(true);
        }

        public override void OnExit()
        {
            gameObject.SetActive(false);
        }

        public override void DidOnClick(GameObject sender)
        {
            // 如果暂停了则不再执行按钮操作
            if (IsPause) return;

            switch (sender.name)
            {
                case "Return Button":
                    PanelManager.instance.PopPanel();
                    break;
                default:
                    break;
            }
        }
    }
}