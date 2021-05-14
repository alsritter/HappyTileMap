using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlsRitter.UIFrame
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
        SelectMapPanel,
        /// <summary>
        /// 设置窗口
        /// </summary>
        SettingPanel,
        /// <summary>
        /// 开始界面创建
        /// </summary>
        StartPanel,
        /// <summary>
        /// 游戏 UI 窗口
        /// </summary>
        GamePanel,
        /// <summary>
        /// 游戏菜单窗口
        /// </summary>
        GameMenuPanel,
        /// <summary>
        /// 游戏结束面板
        /// </summary>
        GameOverPanel,
        /// <summary>
        /// 游戏胜利面板
        /// </summary>
        GameWinPanel
    }
}