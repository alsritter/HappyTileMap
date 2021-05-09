using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlsRitter.GenerateMap.CustomTileFrame.MapDataEntity.V1.Dto;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace AlsRitter.Utilities
{
    /// <summary>
    /// 这个工具类用于，通过 id 返回资源
    /// </summary>
    public class LoadResourceByIdTool
    {
        // 用于表示它是否初始化
        private static bool _isTileInit = false;
        private static bool _isBgInit = false;
        private static bool _isPropInit = false;

        // 这里存储本地的全部 TileSprite 资源路径
        private static Dictionary<string, TileResourcePath> _tileDict = new Dictionary<string, TileResourcePath>();
        // 这里用于装载背景图片
        private static Dictionary<string, string> _bgDict = new Dictionary<string, string>();
        // 这里用于存储预制件的信息
        private static Dictionary<string, PropResourcePath> _propDict = new Dictionary<string, PropResourcePath>();


        /// <summary>
        /// 取得预制件
        /// </summary>
        /// <param name="propId"></param>
        /// <returns>返回的对象还需要实例化</returns>
        public static GameObject GetProp(string propId)
        {
            // 如果没有初始化
            if (!_isPropInit)
            {
                LoadJsonTool.ParsePropPathJsonData(ref _propDict);
                _isPropInit = true;
            }

            _propDict.TryGetValue(propId, out var propInfo);
            

            if (propInfo == null)
            {
                Debug.LogError($"Prefab: \"{propId}\" Can't find! Please check whether the key exists");
                return new GameObject("error object");
            }

            GameObject go = null;
            go = Resources.Load<GameObject>(propInfo.path);
            if (go != null) return go;
            go = new GameObject("error object");
            Debug.LogError($"Prefab: \"{propId}\" prefab cannot be loaded, please check the path in props. Json is correct");
            return go;
        }


        /// <summary>
        /// 取得 Tile 的贴图
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Sprite GetTileSprite(string name)
        {
            // 如果没有初始化
            if (!_isTileInit)
            {
                LoadJsonTool.ParseTileSpritePathJsonData(ref _tileDict);
                _isTileInit = true;
            }

            _tileDict.TryGetValue(name, out var saPathInfo);
            Sprite sa = null;

            if (saPathInfo == null)
            {
                sa = Resources.Load<Sprite>(_tileDict["000"].path);
                Debug.LogError($"sprite: \"{name}\" Can't find! Please check whether the key exists");
                return sa;
            }

            // 先判断贴图类型
            switch (saPathInfo.mode)
            {
                case TileResourcePath.SpriteMode.Single:
                    sa = Resources.Load<Sprite>(saPathInfo.path);
                    break;
                case TileResourcePath.SpriteMode.Multiple:
                    var sprites = Resources.LoadAll<Sprite>(saPathInfo.path);
                    foreach (var s in sprites)
                    {
                        if (s.name == saPathInfo.spriteId)
                        {
                            sa = s;
                        }
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (sa != null) return sa;
            // 找不到也返回错误贴图
            sa = Resources.Load<Sprite>(_tileDict["000"].path);
            Debug.LogError(
                $"sprite: \"{saPathInfo}\" Can't find! The address of this Sprite may be incorrectly written, please contact the administrator");

            return sa;
        }

        /// <summary>
        /// 取得背景图片
        /// </summary>
        /// <param name="bgId"></param>
        /// <returns></returns>
        public static Sprite GetBackgroundSprite(string bgId)
        {
            // 如果没有初始化
            if (!_isBgInit)
            {
                LoadJsonTool.ParseBackgroundPathJsonData(ref _bgDict);
                _isBgInit = true;
            }

            // 先判断当前传入的 key 是否为空
            _bgDict.TryGetValue(bgId, out var path);

            Sprite sa = null;

            if (path == null)
            {
                // 加载错误贴图
                sa = Resources.Load<Sprite>(_bgDict["000"]);
                Debug.LogError($"sprite: \"{bgId}\" Can't find! Please check whether the key exists");
                return sa;
            }

            sa = Resources.Load<Sprite>(path);
            // 找不到也返回错误贴图
            if (sa != null) return sa;
            Debug.LogError(
                $"sprite: \"{bgId}\" Can't find! The address of this Sprite may be incorrectly written, please contact the administrator");
            sa = Resources.Load<Sprite>(_bgDict["000"]);
            return sa;
        }
    }
}