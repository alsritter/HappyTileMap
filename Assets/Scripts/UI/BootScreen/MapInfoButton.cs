using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 这个用于装载按钮的信息
/// </summary>
public class MapInfoButton : MonoBehaviour
{
    public delegate void ButtonCallback(int i);

    private int index;
    private ButtonCallback callback;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="index0"></param>
    /// <param name="callback0"></param>
    public void InitButton(int index0, ButtonCallback callback0)
    {
        this.index = index0;
        this.callback = callback0;
    }
    

    /// <summary>
    /// 点击了这个按钮
    /// </summary>
    public void ClickInfoButton()
    {
        callback(index);
    }
}