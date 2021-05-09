using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlsRitter.ExceptionHandler;
using AlsRitter.GenerateMap.CustomTileFrame.MapDataEntity.V1.Dto;
using UnityEngine;

namespace AlsRitter.Utilities
{
    /// <summary>
    /// 这个工具类用于，通过 id 返回资源
    /// </summary>
    public class LoadResourceByIdTool
    {
        // 用于表示它是否初始化
        private static bool _isTileInit = false;
        // 用于表示它是否初始化
        private static bool _isBgInit = false;

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
        /// <returns></returns>
        public static GameObject GetProp(string propId)
        {
            return null;
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
                var message = $"sprite: \"{name}\" Can't find! Please check whether the key exists";
                Debug.LogWarning(message);
                throw new ResourceException(message);
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

            // 找不到也返回错误贴图
            if (sa == null) sa = Resources.Load<Sprite>(_tileDict["000"].path);

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
            _bgDict.TryGetValue(bgId, out var saPathInfo);

            Sprite sa = null;
            if (saPathInfo == null)
            {
                // 加载错误贴图
                sa = Resources.Load<Sprite>(_bgDict["000"]);
                var message = $"sprite: \"{bgId}\" Can't find! Please check whether the key exists";
                Debug.LogWarning(message);
                throw new ResourceException(message);
            }

            sa = Resources.Load<Sprite>(_bgDict[bgId]);

            // 找不到也返回错误贴图
            if (sa == null) sa = Resources.Load<Sprite>(_bgDict["000"]);

            return sa;
        }
    }
}