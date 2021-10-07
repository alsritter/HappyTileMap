using System;
using System.Collections;
using AlsRitter.CallWeb;
using AlsRitter.EventFrame;
using AlsRitter.GenerateMap;
using AlsRitter.GenerateMap.Interface.Do;
using AlsRitter.GenerateMap.V1;
using AlsRitter.UIFrame;
using AlsRitter.Utilities;
using AlsRitter.V3.Player;
using UnityEngine;

namespace AlsRitter.GlobalControl {
    public class GameSceneManager : MonoBehaviour, IEventObserver {
        public UnityToWeb callJs;

        private BuildTileMap    buildTileMap;
        private BuildBackground buildBackground;
        private BuildMapProp    buildMapProp;

        private GameMap gameMap { get; set; } // 地图数据

        // 淡入淡出场景
        public Animator fade;

        private Vector2 playerBirth; // 角色出生位置
        private IPlayer pm; // 角色
        private int     hp     = 3; // 剩余血量(0 开始)
        private int     scores = 0; // 得分
        private Timer   timer;

        private int startTriggerId; // 画布开始变暗
        private int endTriggerId; // 画布开始恢复正常
        private int blackTriggerId; // 黑屏

        private void Awake() {
            buildTileMap = GetComponent<BuildTileMap>();
            buildBackground = GetComponent<BuildBackground>();
            buildMapProp = GetComponent<BuildMapProp>();
            var loader = new LoadingMapData();

            var json = "";
            try {
                json = callJs.GetJsonData();
            }
            catch (Exception e) {
                Debug.LogError("读取失败...");
                Debug.LogError(e);
            }

            gameMap = loader.GetGameMapData(json);

            // 创建一个Timer
            timer = gameObject.AddComponent<Timer>();
            pm = GlobalPlayer.GetPlayer();

            startTriggerId = Animator.StringToHash("Start");
            endTriggerId = Animator.StringToHash("End");
            blackTriggerId = Animator.StringToHash("Black");

            EventManager.Register(this, EventID.Scores, EventID.Harm, EventID.Win, EventID.ResetGame,
                                  EventID.ReturnMenu);
        }

        private void Start() {
            InitGame();
        }

        /**
         * 游戏开始
         */
        private void InitGame() {
            fade.SetTrigger(blackTriggerId);
            scores = 0;
            hp = 3;
            // 先设置角色远一点的位置，避免影响创建地图
            pm.SetPos(new Vector3Int(9999, 9999, 1));
            pm.StopMove();
            // 初始化数据
            // 开始设置背景信息
            buildBackground.StartSetBackground(gameMap);
            // 初始化道具
            buildMapProp.StartCreateProps(gameMap);
            // 初始化格子
            buildTileMap.StartCreateMap(gameMap);
            // 开启一个协程等待
            StartCoroutine(WaitLoadingMap());
        }

        private IEnumerator WaitLoadingMap() {
            // 检查地图是否初始化完成
            while (!buildTileMap.finish) {
                yield return null;
            }

            // 初始化数据
            timer.reset();
            // 开始计数 无限计数( repeatCount 为 <=0 时 无限重复)
            timer.start(1, -1, null, null);

            // 初始化角色信息
            pm.SetSpeed(gameMap.initial.Speed);
            pm.SetPos(new Vector3Int(gameMap.initial.X, gameMap.initial.Y, 1));
            playerBirth = pm.GetPos();
            // 播放游戏开始特效
            StartCoroutine(GameStartEffect());
            pm.CanMove();
        }

        /**
         * 游戏开始的特效
         */
        private IEnumerator GameStartEffect() {
            yield return new WaitForSeconds(1.5f);
            fade.SetTrigger(endTriggerId);
        }


        /**
         * 游戏结束的特效
         */
        private IEnumerator GameEndEffect(bool isDie) {
            fade.SetTrigger(startTriggerId);
            scores = 0;
            // 临时让角色血量
            hp = 30;
            pm.StopMove();
            yield return new WaitForSeconds(1);
            // 先设置角色远一点的位置，避免影响创建地图
            pm.SetPos(new Vector3Int(9999, 9999, 1));
            // 弹出面板
            PanelManager.instance.PushPanel(isDie ? UIPanelType.GameOverPanel : UIPanelType.GameWinPanel);
        }


        /**
         * 受伤时的效果
         */
        private IEnumerator PlayerInjured() {
            pm.StopMove();
            timer.stop();
            if (hp < 0) {
                StartCoroutine(GameEndEffect(true));
                timer.stop();
                yield break; // 结束协程
            }

            fade.SetTrigger(startTriggerId);
            yield return new WaitForSeconds(1);
            pm.SetPos(playerBirth);
            fade.SetTrigger(endTriggerId);
            yield return new WaitForSeconds(.5f);
            pm.CanMove();
            timer.start();
        }

        public void HandleEvent(EventData resp) {
            switch (resp.eid) {
                case EventID.Scores:
                    // 收到得分事件
                    scores++;
                    break;
                // 受伤事件
                case EventID.Harm:
                    hp--;
                    StartCoroutine(PlayerInjured());
                    // 受伤
                    break;
                case EventID.Win:
                    //Debug.Log("You Win!!");
                    StartCoroutine(GameEndEffect(false));
                    timer.stop();
                    break;
                case EventID.ResetGame:
                    //Debug.Log("ResetGame");
                    InitGame();
                    break;
            }
        }

        public void OnDestroy() {
            EventManager.Remove(this);
        }
    }
}