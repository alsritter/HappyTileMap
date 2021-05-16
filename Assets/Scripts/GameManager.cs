using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlsRitter.EventFrame;
using AlsRitter.GenerateMap.CustomTileFrame.MapDataEntity.V1.Dto;
using AlsRitter.Net;
using AlsRitter.Net.Entity;
using AlsRitter.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AlsRitter.GlobalControl
{
    /// <summary>
    /// 这里面存放一些全局数据（主要就是一些网络下载的资源）
    /// </summary>
    public class GameManager : Singleton<GameManager>
    {
        public GameUserInfoDTO user { get; private set; }
        public List<GameMapInfoDTO> mapInfos { get; private set; }
        private bool isInitMapInfos = false;

        public MapRootDto currentMapData { get; private set; }

        /// <summary>
        /// 用于缓存地图下载的结果
        /// </summary>
        public Dictionary<string, string> mapCache { get; private set; }

        /// <summary>
        /// 用于缓存图片下载的结果
        /// </summary>
        public Dictionary<string, Sprite> imgCache { get; private set; }

        private readonly EventData loginSucceedEvent;

        public GameManager()
        {
            // 未登录初始名字是 Login
            user = new GameUserInfoDTO {username = "Login"};
            mapInfos = new List<GameMapInfoDTO>();
            mapCache = new Dictionary<string, string>();
            imgCache = new Dictionary<string, Sprite>();

            loginSucceedEvent = EventData.CreateEvent(EventID.LoginSucceed);
        }


        public void Login(string code, NetworkTool.RequestCallback callback)
        {
            StartCoroutine(NetworkTool.GetRequest($"/login_code/{code}", (result =>
            {
                user = JsonConvert.DeserializeObject<GameUserInfoDTO>(result);
                loginSucceedEvent.Send();
                // Debug.Log(user);
                isInitMapInfos = false; // 让地图信息也初始化
                callback(result);
            })));
        }

        /// <summary>
        /// 用于初始化地图信息
        /// </summary>
        /// <param name="callback"></param>
        public void InitMapInfoList(NetworkTool.RequestCallback callback)
        {
            if (isInitMapInfos)
            {
                callback("");
                return;
            }

            StartCoroutine(NetworkTool.GetRequest("/get_map_infos", result =>
            {
                // 先清空
                mapInfos.Clear();
                var array = JArray.Parse(result);
                foreach (var item in array)
                {
                    mapInfos.Add(JsonConvert.DeserializeObject<GameMapInfoDTO>(item.ToString()));
                }

                callback(result);
                isInitMapInfos = true;
            }));
        }

        /// <summary>
        /// 取得地图信息
        /// </summary>
        /// <param name="path"></param>
        /// <param name="callback"></param>
        private void GetMap(string path, NetworkTool.RequestCallback callback)
        {
            // 先检查缓存里面有没有
            if (mapCache.TryGetValue(path, out var value))
            {
                callback(value);
                return;
            }

            StartCoroutine(NetworkTool.GetRequestByFullPath(path, result =>
            {
                // 先添加到缓存里面
                mapCache.Add(path, result);
                callback(result);
            }));
        }

        /// <summary>
        /// 根据传入的地址下载地图并开始游戏
        /// </summary>
        /// <param name="fullPath"></param>
        public void StartGame(string fullPath)
        {
            GetMap(fullPath, result =>
            {
                Debug.Log(result);
                currentMapData = string.IsNullOrEmpty(result) ? null : JsonConvert.DeserializeObject<MapRootDto>(result);
                SceneManager.LoadScene("Scenes/GameScene");
            });
        }

        /// <summary>
        /// 取得图片
        /// </summary>
        /// <param name="path"></param>
        /// <param name="callback"></param>
        public void GetSpriteByPath(string path, NetworkTool.RequestSpriteCallback callback)
        {
            // 先检查缓存里面有没有
            if (imgCache.TryGetValue(path, out var value))
            {
                callback(value);
                return;
            }

            StartCoroutine(NetworkTool.GetSpriteRequest(path, result =>
            {
                // 先添加到缓存里面
                imgCache.Add(path, result);
                callback(result);
            }));
        }
    }
}