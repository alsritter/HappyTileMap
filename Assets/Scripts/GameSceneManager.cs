﻿using System.Collections;
using System.Collections.Generic;
using AlsRitter.EventFrame;
using AlsRitter.GenerateMap;
using AlsRitter.GenerateMap.CustomTileFrame.MapDataEntity.V1.Dto;
using AlsRitter.PlayerController.FSM;
using AlsRitter.UIFrame;
using AlsRitter.Utilities;
using UnityEngine;
using UnityEngine.Tilemaps;
using DG.Tweening; //引入命名空间

namespace AlsRitter.GlobalControl
{
    public class GameSceneManager : MonoBehaviour, IEventObserver
    {
        private BuildTileMap buildTileMap;
        private BuildBackground buildBackground;
        private BuildMapProp buildMapProp;

        // 淡入淡出场景
        public Animator fade;

        private Vector2 playerBirth; // 角色出生位置
        private PlayerFSMSystem pm; // 角色
        private MapRootDto mapDto; // 地图数据
        private int hp = 3; // 剩余血量(0 开始)
        private int scores = 0; // 得分
        private Timer timer;

        private void Awake()
        {
            buildTileMap = GetComponent<BuildTileMap>();
            buildBackground = GetComponent<BuildBackground>();
            buildMapProp = GetComponent<BuildMapProp>();

            // 创建一个Timer
            timer = gameObject.AddComponent<Timer>();
            pm = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerFSMSystem>();
            EventManager.Register(this, EventID.Scores, EventID.Harm, EventID.Win, EventID.ResetGame,
                EventID.ReturnMenu);
        }

        private void Start()
        {
            GameStart();
        }


        /// <summary>
        /// 用于初始化
        /// </summary>
        private void Init()
        {
            // 先加载地图数据
            var map = GameManager.instance.currentMapData;
            // 地图数据为空则加载本地资源
            mapDto = map ?? LoadJsonTool.ParseLocalMapJsonData();
            
            // 初始化角色信息
            pm.speed = mapDto.Initial.Speed;
            pm.runDivisor = mapDto.Initial.RunDivisor;
            pm.jumpSleepDivisor = mapDto.Initial.JumpSpeedDivisor;
            pm.climbSpeed = mapDto.Initial.ClimbSpeed;
            pm.crouchSpeedDivisor = mapDto.Initial.CrouchSpeedDivisor;
            pm.jumpForce = mapDto.Initial.JumpForce;
            pm.jump2ForceDivisor = mapDto.Initial.Jump2ForceDivisor;
            pm.climbLateralForce = mapDto.Initial.ClimbLateralForce;
            pm.transform.position = new Vector3Int(mapDto.Initial.X, mapDto.Initial.Y, 1);
            playerBirth = pm.transform.position;

            // 设置角色
            pm.GetComponent<SpriteRenderer>().color = Color.white;
            pm.rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            // 避免歪了
            pm.transform.rotation = Quaternion.identity;
            // 给个向下的力，否则动不了
            pm.rb.AddForce(new Vector2(0, -1), ForceMode2D.Impulse);

            // 开始设置背景信息
            buildBackground.StartSetBackground(mapDto);

            // 初始化格子
            buildTileMap.StartCreateMap(mapDto);

            // 初始化道具
            buildMapProp.StartCreateProps(mapDto);
        }

        /// <summary>
        /// 游戏开始
        /// </summary>
        private void GameStart()
        {
            // 发送当前游玩数据
            GameManager.instance.SendStartGame();
            // 先播特效
            StartCoroutine(GameStartEffect());
            // 同时初始化数据
            Init();
            // 初始化数据
            timer.reset();
            scores = 0;
            hp = 3;
            // 开始计数 无限计数( repeatCount 为 <=0 时 无限重复)
            timer.start(1, -1, null, null);
        }

        /// <summary>
        /// 游戏开始的特效
        /// </summary>
        /// <returns></returns>
        private IEnumerator GameStartEffect()
        {
            fade.SetBool("Black", true);
            yield return new WaitForSeconds(1);
            fade.SetBool("Black", false);
            yield return new WaitForSeconds(.3f);
            fade.SetTrigger("End");
        }


        /// <summary>
        /// 游戏结束
        /// </summary>
        /// <param name="isDie">是否是输</param>
        private void GameEnd(bool isDie)
        {
            pm.rb.constraints = RigidbodyConstraints2D.FreezePosition;
            StartCoroutine(GameEndEffect(isDie));
            timer.stop();
            var thp = hp < 0 ? 0 : hp + 1;
            //Debug.Log($"当前剩余 HP：{thp}, 游戏花费时间：{timer.currentTime} 秒，当前得分：{scores}");
            GameManager.instance.SendGameEnd(scores, timer.currentTime, thp, !isDie);
        }


        /// <summary>
        /// 游戏结束的特效
        /// </summary>
        /// <param name="isDie"></param>
        /// <returns></returns>
        private IEnumerator GameEndEffect(bool isDie)
        {
            var sprite = pm.GetComponent<SpriteRenderer>();
            sprite.DOColor(new Color(0, 0, 0, 0), 1);
            //fade.SetTrigger("Start");
            yield return new WaitForSeconds(1);
            // 还需要重置角色位置
            pm.transform.position = playerBirth;
            // 弹出面板
            PanelManager.instance.PushPanel(isDie ? UIPanelType.GameOverPanel : UIPanelType.GameWinPanel);
        }


        public void HandleEvent(EventData resp)
        {
            switch (resp.eid)
            {
                case EventID.Scores:
                    // 收到得分事件
                    scores++;
                    break;
                // 受伤事件
                case EventID.Harm:
                    hp--;
                    StartCoroutine(PlayerInjured());
                    GameManager.instance.SendHarmInfo(pm.transform.position);
                    // 受伤
                    break;
                case EventID.Win:
                    //Debug.Log("You Win!!");
                    GameEnd(false);
                    break;
                case EventID.ResetGame:
                    //Debug.Log("ResetGame");
                    GameStart();
                    break;
                case EventID.ReturnMenu:
                    GameManager.instance.ReturnMenu();
                    break;
            }
        }

        /// <summary>
        /// 受伤时的效果
        /// </summary>
        /// <returns></returns>
        private IEnumerator PlayerInjured()
        {
            timer.stop();
            pm.rb.constraints = RigidbodyConstraints2D.FreezePosition;

            if (hp < 0)
            {
                GameEnd(true);
                yield break; // 结束协程
            }

            var sprite = pm.GetComponent<SpriteRenderer>();
            // 变红
            sprite.color = new Color(0.6509434f, 0.138172f, 0.138172f);
            // 渐变到透明
            sprite.DOColor(new Color(0, 0, 0, 0), 1);
            fade.SetTrigger("Start");
            yield return new WaitForSeconds(1);

            fade.SetTrigger("End");
            pm.transform.position = playerBirth;

            yield return new WaitForSeconds(.5f);

            // 设置角色
            pm.GetComponent<SpriteRenderer>().color = Color.white;
            pm.rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            pm.transform.rotation = Quaternion.identity; // 避免歪了
            pm.rb.AddForce(new Vector2(0, -1), ForceMode2D.Impulse); // 给个向下的力，否则动不了

            timer.start();
        }

        public void OnDestroy() {
            EventManager.Remove(this);
        }
    }
}