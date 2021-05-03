using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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
    public class PowerUp : MonoBehaviour
    {
        public string powerUpName;
        public string powerUpExplanation;
        public string powerUpQuote;

        [Tooltip("这个效果是否是有时效限制的")]
        public bool expiresImmediately;
        public GameObject specialEffect;
        public AudioClip soundEffect;

        /// <summary>
        /// 这里则是游戏对象
        /// </summary>
        public GameObject playerBrain;

        protected SpriteRenderer spriteRenderer;

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


        protected virtual void PowerUpCollected(GameObject gameObjectCollectingPowerUp)
        {
            // 将道具的状态修改为已经拾取了
            powerUpState = PowerUpState.IsCollected;

            // 这里可以把道具的位置移动到玩家那里去（这块使用 Tween 可以平滑移动）
            // 将升级游戏对象移动到收集它的玩家的下方，这对于功能来说并不是必要的
            // 但它在游戏对象层次结构中更简洁
            gameObject.transform.DOLocalMove(playerBrain.gameObject.transform.position, .5f)
                .OnComplete(() => gameObject.transform.parent = playerBrain.gameObject.transform);

            // 执行道具的效果
            PowerUpEffects();

            // Payload      
            PowerUpPayload();

            // Send message to any listeners
            foreach (GameObject go in EventSystemListeners.main.listeners)
            {
                ExecuteEvents.Execute<IPowerUpEvents>(go, null, (x, y) => x.OnPowerUpCollected(this, playerBrain));
            }

            // Now the power up visuals can go away
            spriteRenderer.enabled = false;
        }

        protected virtual void PowerUpEffects()
        {
            if (specialEffect != null)
            {
                Instantiate(specialEffect, transform.position, transform.rotation, transform);
            }

            if (soundEffect != null)
            {
                MainGameController.main.PlaySound(soundEffect);
            }
        }

        protected virtual void PowerUpPayload()
        {
            Debug.Log("Power Up collected, issuing payload for: " + gameObject.name);

            // If we're instant use we also expire self immediately
            if (expiresImmediately)
            {
                PowerUpHasExpired();
            }
        }

        protected virtual void PowerUpHasExpired()
        {
            if (powerUpState == PowerUpState.IsExpiring)
            {
                return;
            }

            powerUpState = PowerUpState.IsExpiring;

            // 发送消息给事件监听器
            foreach (GameObject go in EventSystemListeners.main.listeners)
            {
                ExecuteEvents.Execute<IPowerUpEvents>(go, null, (x, y) => x.OnPowerUpExpired(this, playerBrain));
            }

            Debug.Log("Power Up has expired, removing after a delay for: " + gameObject.name);
            DestroySelfAfterDelay();
        }

        protected virtual void DestroySelfAfterDelay()
        {
            // Arbitrary delay of some seconds to allow particle, audio is all done
            // TODO could tighten this and inspect the sfx? Hard to know how many, as subclasses could have spawned their own
            Destroy(gameObject, 10f);
        }
    }
}