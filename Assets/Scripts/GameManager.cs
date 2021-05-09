using System.Collections;
using System.Collections.Generic;
using AlsRitter.EventFrame;
using AlsRitter.GenerateMap;
using AlsRitter.GenerateMap.CustomTileFrame.MapDataEntity.V1.Dto;
using AlsRitter.PlayerController.FSM;
using AlsRitter.Utilities;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour, IEventObserver
{
    private BuildTileMap buildTileMap;
    private BuildBackground buildBackground;
    private BuildMapProp buildMapProp;

    // 淡入淡出场景
    public Animator fade;
    // 游戏结束
    public GameObject gameOver;


    private Vector2 playerBirth; // 角色出生位置
    private PlayerFSMSystem pm; // 角色
    private MapRootDto mapDto; // 地图数据
    private int hp = 3; // 剩余血量(0 开始)

    private void Awake()
    {
        buildTileMap = GetComponent<BuildTileMap>();
        buildBackground = GetComponent<BuildBackground>();
        buildMapProp = GetComponent<BuildMapProp>();


        pm = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerFSMSystem>();
        EventManager.Register(this, EventID.Harm);
    }

    /// <summary>
    /// 用于初始化
    /// </summary>
    private void Init()
    {
        // 先加载地图数据
        mapDto = LoadJsonTool.ParseMapJsonData();

        // 初始化角色信息
        pm.speed = mapDto.Initial.Speed;
        pm.runDivisor = mapDto.Initial.RunDivisor;
        pm.jumpSleepDivisor = mapDto.Initial.JumpSpeedDivisor;
        pm.climbSpeed = mapDto.Initial.ClimbSpeed;
        pm.crouchSpeedDivisor = mapDto.Initial.CrouchSpeedDivisor;
        pm.jumpForce = mapDto.Initial.JumpForce;
        pm.jump2ForceDivisor = mapDto.Initial.Jump2ForceDivisor;
        pm.climbLateralForce = mapDto.Initial.ClimbLateralForce;
        // 设置角色位置
        pm.transform.position = new Vector3Int(mapDto.Initial.X, mapDto.Initial.Y, 1);
        playerBirth = pm.transform.position;


        // 开始设置背景信息
        buildBackground.StartSetBackground(mapDto);

        // 初始化格子
        buildTileMap.StartCreateMap(mapDto);

        // 初始化道具
        buildMapProp.StartCreateProps(mapDto);
    }


    private void OnGUI()
    {
        if (GUILayout.Button("加载地图"))
        {
            Init();
        }
    }

    IEnumerator PlayerInjured()
    {
        pm.rb.constraints = RigidbodyConstraints2D.FreezePosition;
        fade.SetTrigger("Start");
        yield return new WaitForSeconds(1);
        pm.transform.position = playerBirth;

        if (hp < 0)
        {
            gameOver.SetActive(true);
            yield break;
        }
        fade.SetTrigger("End");
        yield return new WaitForSeconds(1);
        pm.rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        // 给个向下的力，否则动不了
        pm.rb.AddForce(new Vector2(0,-1),ForceMode2D.Impulse);
    }


    public void HandleEvent(EventData resp)
    {
        switch (resp.eid)
        {
            case EventID.Harm:
                StartCoroutine(PlayerInjured());
                hp--;

                // 受伤
                break;
        }
    }
}