using System.Collections;
using System.Collections.Generic;
using UIFrame;
using UnityEngine;

public class StoryModePanelController : BasePanel
{
    public override void DidOnClick(GameObject sender)
    {
        if (IsPause) return;

        switch (sender.name) {
            case "Return Button":
                UIManager.instance.PopPanel();
                break;
            default:
                break;
        }
    }
}
