using System.Collections;
using System.Collections.Generic;
using AlsRitter.GlobalControl;
using AlsRitter.UIFrame;
using UnityEngine;

namespace AlsRitter.UIFrame.Controller
{
    public class SelectMapPanelController : BasePanel
    {
        public override UIPanelType uiType => UIPanelType.SelectMapPanel;

        [Header("装 Map Item 的容器")]
        public GameObject container;
        [Header("需要被创建的 Item 模板")]
        public GameObject mapItemPrefab;

        /// <summary>
        /// 不需要特效
        /// </summary>
        public override void OnEnter()
        {
            gameObject.SetActive(true);
            // 先清空
            ClearItem();
            // 读取地图信息
            LoadingItem();
        }

        private void ClearItem()
        {
            // 清空 Props 里面的内容
            for (var i = 0; i < container.transform.childCount; i++) {  
                Destroy(container.transform.GetChild (i).gameObject);  
            } 
        }

        private void LoadingItem()
        {
            var list = GameManager.instance.mapInfos;
            list.ForEach(x =>
            {
                var o = Instantiate(mapItemPrefab);
                o.transform.SetParent(container.transform);
            });
        }

        public override void OnExit()
        {
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
                default:
                    break;
            }
        }
    }
}