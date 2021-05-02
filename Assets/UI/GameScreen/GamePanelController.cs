using System.Collections;
using System.Collections.Generic;
using UIFrame;
using UnityEngine;

public class GamePanelController : BasePanel
{
    public override void DidOnClick(GameObject sender)
    {
        switch (sender.name)
        {
            case "Setting Button":
                Debug.Log("游戏开始了~");
                break;
            default:
                break;
        }
    }
}