using System.Collections;
using System.Collections.Generic;
using AlsRitter.EventFrame;
using AlsRitter.GlobalControl;
using AlsRitter.UIFrame;
using TMPro;
using UnityEngine;

namespace AlsRitter.UIFrame.Controller
{
    public class StartPanelController : BasePanel, IEventObserver
    {
        public override UIPanelType uiType => UIPanelType.StartPanel;

        public TextMeshProUGUI text;

        public override void OnInitUI()
        {
            EventManager.Register(this, EventID.LoginSucceed);
        }

        public override void DidOnClick(GameObject sender)
        {
            if (IsPause) return;

            switch (sender.name)
            {
                case "Start Game Button":
                    GameManager.instance.GetMapInfoList(result =>
                    {
                        // Debug.Log("游戏开始了~");
                        PanelManager.instance.PushPanel(UIPanelType.SelectMapPanel);
                    });

                    break;
                case "UserArea":
                    PanelManager.instance.PushPanel(UIPanelType.LoginPanel);
                    break;
                default:
                    break;
            }
        }

        public void HandleEvent(EventData resp)
        {
            switch (resp.eid)
            {
                case EventID.LoginSucceed:
                    var info = GameManager.instance.user;
                    text.text = info.username.Length > 6 ? info.username.Substring(0, 6) + "..." : info.username;
                    break;
            }
        }
    }
}