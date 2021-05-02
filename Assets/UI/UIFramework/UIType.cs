using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIFrame
{
    public enum UIPanelType
    {
        /// <summary>
        /// 主界面的登陆窗口
        /// </summary>
        LoginPanel,
        /// <summary>
        /// 选择模式的窗口
        /// </summary>
        SelectModePanel,
        /// <summary>
        /// 设置窗口
        /// </summary>
        SettingPanel,
        /// <summary>
        /// 开始界面创建
        /// </summary>
        StartPanel,
        /// <summary>
        /// 故事模式窗口
        /// </summary>
        StoryModePanel,
        /// <summary>
        /// 游戏 UI 窗口
        /// </summary>
        GamePanel,
        /// <summary>
        /// 游戏菜单窗口
        /// </summary>
        GameMenuPanel
    }
}