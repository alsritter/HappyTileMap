using System.Collections;
using System.Collections.Generic;
using AlsRitter.UIFrame;
using UnityEngine;

public class TestControllerGUI : MonoBehaviour
{
    private void OnGUI()
    {
        if (GUILayout.Button("打开开始界面"))
        {
            UIManager.instance.PushPanel(UIPanelType.StartPanel);
        }
    }
}