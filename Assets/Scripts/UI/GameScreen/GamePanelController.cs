using System;
using System.Collections;
using System.Collections.Generic;
using AlsRitter.EventFrame;
using TMPro;
using AlsRitter.UIFrame;
using UnityEngine;
using UnityEngine.UI;

namespace AlsRitter.UIFrame.Controller {
    public class GamePanelController : BasePanel, IEventObserver {
        public override UIPanelType uiType => UIPanelType.GamePanel;

        private GameObject scores;

        private TextMeshProUGUI text;
        private Image[]         bloods;
        private int             hp = 3; // 剩余血量(0 开始)

        /// <summary>
        /// 初始化时先取得子部件
        /// </summary>
        public override void OnInitUI() {
            scores = GameObject.Find("Scores");
            var temp = GameObject.Find("bloods");
            bloods = temp.GetComponentsInChildren<Image>();
            text = scores.transform.GetComponentInChildren<TextMeshProUGUI>();
            text.text = "000";
            // 别忘了注册自己
            EventManager.Register(this, EventID.Scores, EventID.Harm, EventID.ResetGame);
        }

        /// <summary>
        /// 用来重置 UI 面板上的信息
        /// </summary>
        private void ResetInfo() {
            hp = 3;
            text.text = "000";
            foreach (var blood in bloods) {
                ColorUtility.TryParseHtmlString("#ac3232", out var temp);
                blood.color = temp;
            }
        }

        public override void DidOnClick(GameObject sender) {
            if (IsPause) return;

            switch (sender.name) {
                case "GameMenuButton":
                    PanelManager.instance.PushPanel(UIPanelType.GameMenuPanel);
                    break;
            }
        }

        private void RefreshHp() {
            if (hp < 0) return;
            ColorUtility.TryParseHtmlString("#464646", out var temp);
            bloods[hp].color = temp;
        }

        public void HandleEvent(EventData resp) {
            switch (resp.eid) {
                case EventID.Scores:
                    // 收到得分事件
                    text.text = $"{(Convert.ToInt32(text.text) + 1):d3}";
                    break;
                case EventID.Harm:
                    RefreshHp();
                    hp--;
                    // 受伤
                    break;
                case EventID.ResetGame:
                    // 重新开始游戏，刷新面板信息
                    ResetInfo();
                    break;
            }
        }

        public override void UIOnDestroy() {
            EventManager.Remove(this);
        }
    }
}