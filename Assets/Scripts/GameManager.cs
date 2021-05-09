using System.Collections;
using System.Collections.Generic;
using AlsRitter.GenerateMap;
using AlsRitter.GenerateMap.CustomTileFrame.MapDataEntity.V1.Dto;
using AlsRitter.PlayerController.FSM;
using AlsRitter.Utilities;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{

    private BuildTileMap buildTileMap;
    private BuildBackground buildBackground;
    private BuildMapProp buildMapProp;
    // 这个 tilemap 主要用来计算角色位置
    public Tilemap tilemap;

    // 角色出生位置
    private Vector2 playerBirth;
    // 角色
    private PlayerFSMSystem pm;
    // 地图数据
    private MapRootDto mapDto;

    private void Awake()
    {
        buildTileMap = GetComponent<BuildTileMap>();
        buildBackground = GetComponent<BuildBackground>();
        buildMapProp = GetComponent<BuildMapProp>();

        pm = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerFSMSystem>();
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
        pm.transform.position = tilemap.GetCellCenterWorld(new Vector3Int(mapDto.Initial.X, mapDto.Initial.Y, 1));


        // 开始设置背景信息
        buildBackground.StartSetBackground(mapDto);

        // 初始化格子
        buildTileMap.StartCreateMap(mapDto);
    }


    private void OnGUI ()
    {
        if(GUILayout.Button("加载地图"))
        {
            //StartCoroutine(Crossfade());
            Init();
        }
    }






    public Animator transition;


    IEnumerator Crossfade()
    {
        transition.SetTrigger("Start");
        // 等待一秒（等动画结束）
        yield return new WaitForSeconds(1);
        Debug.Log("结束");
    }

}
