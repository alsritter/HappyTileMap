using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlsRitter.EventFrame;
using AlsRitter.Net;
using AlsRitter.Net.Entity;
using AlsRitter.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace AlsRitter.GlobalControl
{
    /// <summary>
    /// 这里面存放一些全局数据（主要就是一些网络下载的资源）
    /// </summary>
    public class GameManager : Singleton<GameManager>
    {
        public GameUserInfoDTO user { get; private set; }
        public List<GameMapInfoDTO> mapInfos { get; private set; }

        private readonly EventData loginSucceedEvent;

        public GameManager()
        {
            // 未登录初始名字是 Login
            user = new GameUserInfoDTO {username = "Login"};
            mapInfos = new List<GameMapInfoDTO>();
            loginSucceedEvent = EventData.CreateEvent(EventID.LoginSucceed);
        }

        public delegate void RequestCallback(string result);

        public void Login(string code, RequestCallback callback)
        {
            StartCoroutine(NetworkTool.GetRequest($"/login_code/{code}", (result =>
            {
                user = JsonConvert.DeserializeObject<GameUserInfoDTO>(result);
                loginSucceedEvent.Send();
                // Debug.Log(user);
                callback(result);
            })));
        }


        public void GetMapInfoList(RequestCallback callback)
        {
            StartCoroutine(NetworkTool.GetRequest("/get_map_infos", result =>
            {
                var array = JArray.Parse(result);
                foreach (var item in array)
                {
                    mapInfos.Add( JsonConvert.DeserializeObject<GameMapInfoDTO>(item.ToString()));
                }
                callback(result);
            }));
        }
    }
}