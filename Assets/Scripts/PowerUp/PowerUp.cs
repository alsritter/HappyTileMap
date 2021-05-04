using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using PlayerController.FSM;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PowerUpSystem
{
    /// <summary>
    /// Game independent Power Up logic supporting 2D and 3D modes.
    /// When collected, a Power Up has visuals switched off, but the Power Up gameobject exists until it is time for it to expire
    /// Subclasses of this must:
    /// 1. Implement PowerUpPayload()
    /// 2. Optionally Implement PowerUpHasExpired() to remove what was given in the payload
    /// 3. Call PowerUpHasExpired() when the power up has expired or tick ExpiresImmediately in inspector
    /// </summary>
    public abstract class PowerUp : MonoBehaviour
    {
        // 道具名称
        public string powerUpName;
        // 道具说明
        public string powerUpExplanation;
        // 道具具体效果（例如失效时间之类的）
        public string powerUpQuote;

        // 注意，如果为 false 需要自己手动调用 PowerUpHasExpired() 方法使之过期
        [Tooltip("这个效果是否是一次性的")]
        public bool expiresImmediately = true;


        /// <summary>
        /// 拾取的特效
        /// </summary>
        public GameObject specialEffect;

        /// <summary>
        /// 拾取的音效
        /// </summary>
        public AudioClip soundEffect;

        /// <summary>
        /// 这里则是游戏对象
        /// </summary>
        private PlayerFSMSystem player;


        protected SpriteRenderer spriteRenderer;
        protected BoxCollider2D box;

        /// <summary>
        /// 内部维护着一个状态
        /// </summary>
        protected enum PowerUpState
        {
            InAttractMode,
            IsCollected,
            IsExpiring
        }

        protected PowerUpState powerUpState;

        protected virtual void Awake()
        {
            // 取得碰撞盒
            box = GetComponent<BoxCollider2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();

            // 如果没有精灵渲染器要给它加上
            if (spriteRenderer != null) return;
            gameObject.AddComponent<SpriteRenderer>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        protected virtual void Start()
        {
            // 道具的初始状态是 “吸引模式”
            powerUpState = PowerUpState.InAttractMode;
            // 找到玩家
            player = GameObject.FindObjectOfType<PlayerFSMSystem>();
        }

        /// <summary>
        /// 2D support
        /// </summary>
        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            // 必须是玩家才能触发
            // 判断这个道具的状态，如果已经给拾取了，则不执行
            if (other.gameObject.tag != "Player" || powerUpState == PowerUpState.IsCollected ||
                powerUpState == PowerUpState.IsExpiring) return;

            PowerUpCollected(other.gameObject);
        }

        /// <summary>
        /// 拾取这个道具时要做的事情
        /// </summary>
        /// <param name="gameObjectCollectingPowerUp"></param>
        protected virtual void PowerUpCollected(GameObject gameObjectCollectingPowerUp)
        {
            // 将道具的状态修改为已经拾取了
            powerUpState = PowerUpState.IsCollected;

            // 这里可以把道具的位置移动到玩家那里去（这块使用 Tween 可以平滑移动）
            // 将升级游戏对象移动到收集它的玩家的下方，这对于功能来说并不是必要的
            // 但它在游戏对象层次结构中更简洁
            gameObject.transform.DOLocalMove(player.gameObject.transform.position, .5f)
                .OnComplete(() => gameObject.transform.parent = player.gameObject.transform);

            // 执行道具的效果
            PowerUpEffects();

            // Payload      
            PowerUpPayload();

            // 现在可以让当前对象消失了（只是关闭了渲染，但是这个道具本身还在 Player 身上）
            spriteRenderer.enabled = false;
            box.enabled = false;
        }

        /// <summary>
        /// 物品被拾取时的消息（特效）
        /// </summary>
        protected virtual void PowerUpEffects()
        {
            if (specialEffect != null)
            {
                Instantiate(specialEffect, transform.position, transform.rotation, transform);
            }

            if (soundEffect != null)
            {
                // 播放音乐
                //MainGameController.main.PlaySound(soundEffect);
            }
        }

        /// <summary>
        /// 道具开始作用，这个一定会被调用
        /// </summary>
        protected virtual void PowerUpPayload()
        {
            //Debug.Log("Power Up collected, issuing payload for: " + gameObject.name);

            // 如果这个道具是一次性的则直接消失
            if (expiresImmediately)
            {
                PowerUpHasExpired();
            }
        }

        /// <summary>
        /// 道具过期时调用
        /// </summary>
        protected virtual void PowerUpHasExpired()
        {
            if (powerUpState == PowerUpState.IsExpiring) return;
            powerUpState = PowerUpState.IsExpiring;

            //Debug.Log("Power Up has expired, removing after a delay for: " + gameObject.name);
            DestroySelfAfterDelay();
        }

        protected virtual void DestroySelfAfterDelay()
        {
            // 任意延迟几秒钟允许粒子，音频全部完成
            // TODO could tighten this and inspect the sfx? Hard to know how many, as subclasses could have spawned their own
            Destroy(gameObject, 10f);
        }
    }
}