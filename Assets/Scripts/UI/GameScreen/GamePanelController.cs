using System.Collections;
using System.Collections.Generic;
using UIFrame;
using UnityEngine;

public class GamePanelController : BasePanel
{
    public override UIPanelType uiType => UIPanelType.GamePanel;

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
}