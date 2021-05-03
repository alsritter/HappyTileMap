using System;
using System.Collections;
using System.Collections.Generic;
using EventFrame;
using TMPro;
using UIFrame;
using UnityEngine;

public class GamePanelController : BasePanel,IEventObserver
{
    public override UIPanelType uiType => UIPanelType.GamePanel;

    private GameObject scores;
    private TextMeshProUGUI text;

    /// <summary>
    /// 初始化时先取得子部件
    /// </summary>
    public override void OnInitUI()
    {
        scores = GameObject.Find("Scores");
        text = scores.transform.GetComponentInChildren<TextMeshProUGUI>();
        // 别忘了注册自己
        EventManager.Register(this,EventID.Scores);
    }


    public override void DidOnClick(GameObject sender)
    {
        if (IsPause) return;

        switch (sender.name)
        {
            case "GameMenuButton":
                UIManager.instance.PushPanel(UIPanelType.GameMenuPanel);
                break;
            default:
                break;
        }
    }

    public void HandleEvent(EventData resp)
    {
        // 收到得分事件
        if (resp.eid != EventID.Scores) return;
        text.text = (Convert.ToInt32(text.text) + 1).ToString();
    }
}