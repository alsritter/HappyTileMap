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

        /// <summary>
        /// 当前玩的地图数据
        /// </summary>
        public MapRootDto currentMapData { get; private set; }

        /// <summary>
        /// 当前玩的地图编号
        /// </summary>
        public string currentMapId { get; private set; }

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
        /// 通过地图 Id 取得地图数据
        /// </summary>
        /// <param name="mapId"></param>
        /// <param name="callback"></param>
        private void GetMap(string mapId, NetworkTool.RequestCallback callback)
        {
            // 先检查缓存里面有没有
            if (mapCache.TryGetValue(mapId, out var value))
            {
                callback(value);
                return;
            }

            StartCoroutine(NetworkTool.GetRequest($"/get_map/{mapId}", result =>
            {
                // 先添加到缓存里面
                mapCache.Add(mapId, result);
                callback(result);
            }));
        }

        /// <summary>
        /// 根据传入的地址下载地图并开始游戏
        /// </summary>
        /// <param name="mapId">使用的地图编号</param>
        public void StartGame(string mapId)
        {
            currentMapId = mapId;
            GetMap(mapId, result =>
            {
                if (string.IsNullOrEmpty(result))
                {
                    Debug.LogError($"This {mapId} does not exist!!!");
                }

                currentMapData = string.IsNullOrEmpty(result)
                    ? null
                    : JsonConvert.DeserializeObject<MapRootDto>(result);

                SceneManager.LoadScene("Scenes/GameScene");
            });
        }

        /// <summary>
        /// 返回菜单界面
        /// </summary>
        public void ReturnMenu()
        {
            SceneManager.LoadScene("Scenes/MenuScene");
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

        /// <summary>
        /// 发送受伤信息
        /// </summary>
        /// <param name="pos">角色位置</param>
        public void SendHarmInfo(Vector2 pos)
        {
            var deathInfo = new GameDeathInfoDTO(pos.x, pos.y);
            StartCoroutine(NetworkTool.PostJsonRequest(
                "/push_death_info"
                , JsonConvert.SerializeObject(deathInfo),
                result =>  {}));
        }


        /// <summary>
        /// 发送开始游戏的信息
        /// </summary>
        public void SendStartGame()
        {
            StartCoroutine(NetworkTool.GetRequest($"/play_map/{currentMapId}", result => { }));
        }

        /// <summary>
        /// 发送游戏结束的信息
        /// </summary>
        /// <param name="score"></param>
        /// <param name="time"></param>
        /// <param name="hp"></param>
        /// <param name="isWin"></param>
        public void SendGameEnd(int score, float time, int hp, bool isWin)
        {
            StartCoroutine(NetworkTool.PostJsonRequest("/push_end_info",
                JsonConvert.SerializeObject(new GameEndInfoDTO(score, time, hp, isWin)), result => { }));
        }
    }
}