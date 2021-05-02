using System.Collections;
using System.Collections.Generic;
using UIFrame;
using UnityEngine;

public class SettingPanelController : BasePanel
{
    public override void DidOnClick(GameObject sender)
    {
        if (IsPause) return;

        switch (sender.name) {
            case "Close Button":
                UIManager.instance.PopPanel();
                break;
            default:
                break;
        }
    }
}
