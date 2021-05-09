using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;


namespace AlsRitter.UIFrame
{
    /// <summary>
    /// 所有UI的父类
    /// 用来控制UI的状态
    /// </summary>
    public abstract class BasePanel : MonoBehaviour
    {
        // UI 是否被初始化
        private bool isInit = false;
        private bool isPause = false;

        public abstract UIPanelType uiType { get; }

        // UI 是否被冻结（暂停）
        internal bool IsPause
        {
            get => this.isPause;
            set
            {
                this.isPause = value;
                if (value)
                {
                    OnPause();
                }
                else
                {
                    OnResume();
                }
            }
        }

        private CanvasGroup mCanvasGroup;

        private void Start()
        {
            InitSuper();
        }

        /// <summary>
        /// 启动时初始化，它会自动被调用，UI Manager 无需关注这块内容
        /// </summary>
        public void InitSuper()
        {
            if (isInit)
                return;

            isInit = true;

            // CanvasGroup 组件可以用来控制一组 UI 元素的某些方面（如同它名字一样，用于管理一组 UI），
            // CanvasGroup 的属性会影响他所有 children 的 GameObject

            mCanvasGroup = GetComponent<CanvasGroup>();
            
            // 注意，因为这里是基类，所以无法通过 [RequireComponent(typeof(CanvasGroup))]  影响子类，
            // 所以需要这里手动添加
            if(mCanvasGroup == null){
                gameObject.AddComponent<CanvasGroup> ();
                mCanvasGroup = GetComponent<CanvasGroup> ();
            }

            gameObject.SetActive(true);

            // 这里会自动把当前面板下的所有 Button 注册进这个委托里面
            var buttons = GetComponentsInChildren<Button>();
            foreach (var item in buttons)
            {
                var btn = (Button) item;
                btn.onClick.AddListener(delegate { this.DidOnClick(btn.gameObject); });
            }

            OnInitUI();
        }

        /// <summary>
        /// 打开时的动画
        /// </summary>
        public void OpenActivity()
        {
            // 可以通过这个 mCanvasGroup 控制全部子对象
            // mCanvasGroup.alpha = 1;

            gameObject.SetActive(true);
            // 从左向右的滑动动画
            var temp = transform.localPosition;
            temp.x = -800;
            transform.localPosition = temp;
            transform.DOLocalMoveX(0, 0.5f);
        }

        /// <summary>
        /// 关闭时的动画
        /// </summary>
        public void CloseActivity()
        {
            transform.DOLocalMoveX(-800, .5f).OnComplete(() => gameObject.SetActive(false));

            /*var outPos = mTransform.position.x - Screen.width;
            mTransform.DOMoveX(outPos, 0.2f).OnComplete(delegate {  });*/
        }

        /// <summary>
        /// 在Start中初始化UI的操作
        /// </summary>
        public virtual void OnInitUI()
        {
        }

        /// <summary>
        /// 界面显示出来
        /// </summary>
        public virtual void OnEnter()
        {
            OpenActivity();
        }

        /// <summary>
        /// 界面暂停(弹出了其他界面)
        /// </summary>
        public virtual void OnPause()
        {
        }

        /// <summary>
        /// 界面继续(其他界面移除，回复本来的界面交互)
        /// </summary>
        public virtual void OnResume()
        {
        }

        /// <summary>
        /// 界面不显示,退出这个界面，界面被关闭
        /// </summary>
        public virtual void OnExit()
        {
            CloseActivity();
        }

        /// <summary>
        /// 注册按钮点击事件
        /// </summary>
        /// <param name="sender">发送事件的按钮</param>
        public abstract void DidOnClick(GameObject sender);
    }
}