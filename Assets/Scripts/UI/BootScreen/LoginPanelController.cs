using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlsRitter.GlobalControl;
using AlsRitter.UIFrame;
using TMPro;
using UnityEngine;

namespace AlsRitter.UIFrame.Controller
{
    public class LoginPanelController : BasePanel
    {
        public override UIPanelType uiType => UIPanelType.LoginPanel;

        public TMP_InputField input;

        public override void DidOnClick(GameObject sender)
        {
            // 如果暂停了则不再执行按钮操作
            if (IsPause) return;

            switch (sender.name)
            {
                case "Login Button":
                    if (input.text == string.Empty) return;
                    GameManager.instance.Login(input.text, result =>
                    {
                        PanelManager.instance.PopPanel();
                    });
                    
                    break;
                case "Close Button":
                    PanelManager.instance.PopPanel();
                    break;
                default:
                    break;
            }
        }
    }
}