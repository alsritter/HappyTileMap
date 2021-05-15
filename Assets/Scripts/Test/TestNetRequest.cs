using System.Collections;
using System.Collections.Generic;
using AlsRitter.Net;
using AlsRitter.Net.Entity;
using Newtonsoft.Json;
using UnityEngine;

public class TestNetRequest : MonoBehaviour
{
    private void OnGUI()
    {
        if (GUILayout.Button("你好"))
        {
            StartCoroutine(NetworkTool.GetRequest("/hello", Debug.Log));
        }

        if (GUILayout.Button("测试登陆"))
        {
            StartCoroutine(NetworkTool.GetRequest("/login_code/ALSRITTER", Debug.Log));
        }

        if (GUILayout.Button("取得地图信息"))
        {
            StartCoroutine(NetworkTool.GetRequest("/get_map_infos/ALSRITTER", Debug.Log));
        }

        if (GUILayout.Button("发送死亡信息"))
        {
            var deathInfo = new GameDeathInfoDTO("alsritter", 12, 3);
            StartCoroutine(NetworkTool.PostJsonRequest(
                "/push_death_info"
                , JsonConvert.SerializeObject(deathInfo),
                Debug.Log));
        }

        if (GUILayout.Button("游戏结束的信息"))
        {
            var endInfo = new GameEndInfoDTO("alsritter", 32, 11.01f, 4, true);

            StartCoroutine(NetworkTool.PostJsonRequest(
                "/push_end_info"
                , JsonConvert.SerializeObject(endInfo),
                Debug.Log));
        }
    }
}