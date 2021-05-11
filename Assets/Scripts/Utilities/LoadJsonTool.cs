using System;
using System.Collections;
using System.Collections.Generic;
using AlsRitter.GenerateMap.CustomTileFrame.MapDataEntity.V1.Dto;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using AlsRitter.UIFrame;
using UnityEngine;


namespace AlsRitter.Utilities
{
    /// <summary>
    /// 加载 JSON 数据
    /// </summary>
    public class LoadJsonTool
    {
        /// <summary>
        /// 加载地图数据
        /// </summary>
        /// <returns></returns>
        public static MapRootDto ParseMapJsonData()
        {
            // Assets/Resources/Json/testData.json
            var text = Resources.Load<TextAsset>("Json/testData");
            var json = text.text;
            return string.IsNullOrEmpty(json) ? null : JsonConvert.DeserializeObject<MapRootDto>(json);
        }


        /// <summary>
        /// 解析JSON，获取所有面板的路径信息
        /// </summary>
        public static void ParseUiPanelTypeJsonData(ref Dictionary<UIPanelType, string> panelPathDict)
        {
            var ta = Resources.Load<TextAsset>("ResourceReadPath/UIPanelType");
            var array = JArray.Parse(ta.text);

            foreach (var item in array)
            {
                var panelType = (UIPanelType) Enum.Parse(typeof(UIPanelType), item["panelType"].ToString());
                var path = item["path"].ToString();
                panelPathDict.Add(panelType, path);
            }
        }

        /// <summary>
        /// 加载背景图片路径
        /// </summary>
        /// <param name="bgInfo"></param>
        public static void ParseBackgroundPathJsonData(ref Dictionary<string, string> bgInfo)
        {
            var json = Resources.Load<TextAsset>("ResourceReadPath/Background");

            if (json == null)
            {
                Debug.LogError("background.json unfounded");
                return;
            }

            var tempArr = JArray.Parse(json.text);
            foreach (var item in tempArr)
            {
                var path = item["path"].ToString();
                var id = item["bg_id"].ToString();
                bgInfo.Add(id, path);
            }
        }

        public static void ParsePropPathJsonData(ref Dictionary<string, PropResourcePath> propInfo)
        {
            var json = Resources.Load<TextAsset>("ResourceReadPath/Props");

            if (json == null)
            {
                Debug.LogError("props.json unfounded");
                return;
            }

            var tempArr = JArray.Parse(json.text);
            foreach (var item in tempArr)
            {
                var id = item["prefab_id"].ToString();
                var sizeW = item["size_w"].ToObject<int>();
                var sizeH = item["size_h"].ToObject<int>();
                var typeS = item["type"].ToObject<PropResourcePath.PropType>();
                var path = item["path"].ToString();

                propInfo.Add(id, new PropResourcePath(id, sizeW, sizeH, typeS, path));
            }
        }

        /// <summary>
        /// 加载资源信息
        /// mode：0  单独的贴图
        /// mode：1  位于精灵集里面（就是一个贴图多个精灵）如果是这种模式，则它在精灵集的名字就是它的索引
        ///
        /// TODO 这里需要重写一下错误处理
        /// </summary>
        /// <param name="spriteInfoDict"></param>
        public static void ParseTileSpritePathJsonData(ref Dictionary<string, TileResourcePath> spriteInfoDict)
        {
            // 先加载要加载的资源路径
            var spriteCatalog = Resources.Load<TextAsset>("ResourceReadPath/TileSpriteCatalog");
            var tempArr = JArray.Parse(spriteCatalog.text);
            foreach (var filePath in tempArr)
            {
                // path
                var resource = Resources.Load<TextAsset>(filePath["path"].ToString());
                var resourceArr = JArray.Parse(resource.text);
                foreach (var item in resourceArr)
                {
                    var spriteId = item["spriteId"].ToString();
                    var path = item["path"].ToString();
                    var mode = item["mode"].ToString();

                    // 先检查一下是否已经存在这个了，如果有则抛出警告
                    if (spriteInfoDict.ContainsKey(spriteId))
                    {
                        Debug.LogWarning($"already existed {spriteId}!!");
                    }

                    spriteInfoDict.Add(spriteId, new TileResourcePath(spriteId, path,
                        (TileResourcePath.SpriteMode) Enum.Parse(typeof(TileResourcePath.SpriteMode), mode)));
                }
            }
        }
    }
}