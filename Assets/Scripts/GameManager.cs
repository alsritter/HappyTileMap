using System.Collections;
using System.Collections.Generic;
using AlsRitter.GenerateMap.CustomTileFrame.MapDataEntity.V1.Dto;
using AlsRitter.PlayerController.FSM;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Animator transition;

    // 角色出生位置
    private Vector2 playerBirth;
    // 角色
    private PlayerFSMSystem pm;

    // 地图数据
    private MapRootDto mapDto;

    /// <summary>
    /// 用于初始化
    /// </summary>
    private void Init()
    {
        mapDto = LoadJsonTool.ParseMapJsonData();

    }


    private void OnGUI ()
    {
        if(GUILayout.Button("淡入"))
        {
            StartCoroutine(Crossfade());
        }
    }

    IEnumerator Crossfade()
    {
        transition.SetTrigger("Start");
        // 等待一秒（等动画结束）
        yield return new WaitForSeconds(1);
        Debug.Log("结束");
    }

}
