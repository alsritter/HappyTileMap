using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;


namespace UIFrame
{
    /// <summary>
    /// UI控制器基类
    /// </summary>
    public abstract class UIBase : MonoBehaviour
    {
        private Transform mTransform;

        //UI是否已经显示
        [HideInInspector]
        public bool mSelfShowed = false;

        // UI是否被初始化
        private bool isInit = false;
        private Vector3 mDefaultPos = Vector3.zero;
        private CanvasGroup mCanvasGroup;

        private void Awake()
        {
            mTransform = transform;
            mDefaultPos = mTransform.position;
        }

        private void Start()
        {
            InitSuper();
        }

        public void InitSuper()
        {
            if (isInit)
                return;
            isInit = true;
            mCanvasGroup = GetComponent<CanvasGroup>();
            if (mCanvasGroup == null)
            {
                gameObject.AddComponent<CanvasGroup>();
                mCanvasGroup = GetComponent<CanvasGroup>();
                mCanvasGroup.alpha = 1;
                mCanvasGroup.ignoreParentGroups = false;
                mCanvasGroup.interactable = false;
                mCanvasGroup.blocksRaycasts = false;
            }

            gameObject.SetActive(true);
            var buttons = GetComponentsInChildren<Button>();
            foreach (var item in buttons)
            {
                var btn = (Button) item;
                btn.onClick.AddListener(delegate { this.DidOnClick(btn.gameObject); });
            }

            DidInitUI();
        }

        public void ShowUI()
        {
            if (!mSelfShowed)
            {
                InitSuper();
                DidShowUI();
            }
        }

        public void HideUI()
        {
            if (mSelfShowed)
            {
                InitSuper();
                DidHideUI();
            }
        }

        public void OpenActivity()
        {
            mSelfShowed = true;
            gameObject.SetActive(true);
            mTransform.position = mDefaultPos + new Vector3(Screen.width, 0, 0);
            
            mTransform.DOMoveX(mDefaultPos.x, 0.2f);
        }

        public void CloseActivity()
        {
            mSelfShowed = false;
            var outPos = mTransform.position.x - Screen.width;
            mTransform.DOMoveX(outPos, 0.2f).OnComplete(delegate { gameObject.SetActive(false); });
        }

        /// <summary>
        /// 在Start中初始化UI的操作
        /// </summary>
        public abstract void DidInitUI();

        /// <summary>
        /// 执行显示UI的操作
        /// </summary>
        public abstract void DidShowUI();

        /// <summary>
        /// 执行关闭UI的操作
        /// </summary>
        public abstract void DidHideUI();

        /// <summary>
        /// 注册按钮点击事件
        /// </summary>
        /// <param name="sender">Sender.</param>
        public abstract void DidOnClick(GameObject sender);
    }
}