using System;
using System.Collections;
using System.Collections.Generic;
using AlsRitter.GlobalControl;
using AlsRitter.UIFrame;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AlsRitter.UIFrame.Controller
{
    public class SelectMapPanelController : BasePanel
    {
        public override UIPanelType uiType => UIPanelType.SelectMapPanel;

        [Header("Map Items")]
        [Tooltip("装 Map Item 的容器")]
        public GameObject container;
        [Tooltip("需要被创建的 Item 模板")]
        public GameObject mapItemPrefab;

        [Header("信息面板")]
        public Image cover;
        public TextMeshProUGUI author;
        public TextMeshProUGUI down;
        public TextMeshProUGUI pass;
        public TextMeshProUGUI playNum;
        public TextMeshProUGUI version;
        public TextMeshProUGUI grade;

        private int currentIndex = -1;

        /// <summary>
        /// 不需要特效
        /// </summary>
        public override void OnEnter()
        {
            gameObject.SetActive(true);
            currentIndex = -1; // 每次先初始成 -1 避免空加载
            // 读取地图信息
            LoadingItem();
        }

        private void ClearItem()
        {
            // 清空 Props 里面的内容
            for (var i = 0; i < container.transform.childCount; i++)
            {
                Destroy(container.transform.GetChild(i).gameObject);
            }
        }

        private void LoadingItem()
        {
            var list = GameManager.instance.mapInfos;

            for (var i = 0; i < list.Count; i++)
            {
                var o = Instantiate(mapItemPrefab);
                o.transform.SetParent(container.transform);
                o.GetComponent<MapInfoButton>().InitButton(i, index =>
                {
                    currentIndex = index;
                    var item = GameManager.instance.mapInfos[index];
                    author.text = item.author.Length > 6 ? item.author.Substring(0, 6) + "..." : item.author;
                    down.text = item.downCount.ToString();
                    pass.text = item.passCount.ToString();
                    playNum.text = item.sumCount.ToString();
                    version.text = item.version;
                    grade.text = item.grade.ToString();

                    GameManager.instance.GetSpriteByPath(item.coverPath, result =>
                    {
                        if (cover == null)
                        {
                            return;
                        }

                        cover.sprite = result;
                    });
                });
            }
        }

        public override void OnExit()
        {
            // 先清空
            ClearItem();
            gameObject.SetActive(false);
        }

        public override void DidOnClick(GameObject sender)
        {
            // 如果暂停了则不再执行按钮操作
            if (IsPause) return;

            switch (sender.name)
            {
                case "Return Button":
                    PanelManager.instance.PopPanel();
                    break;
                case "StartGameButton":
                    if (currentIndex < 0) return;
                    GameManager.instance.StartGame(GameManager.instance.mapInfos[currentIndex].downPath);
                    break;
                default:
                    break;
            }
        }
    }
}