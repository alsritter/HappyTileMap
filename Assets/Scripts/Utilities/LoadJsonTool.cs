using System;
using System.Collections;
using System.Collections.Generic;
using CustomTileFrame.MapDataEntity.Dto;
using DTO;
using ExceptionHandler;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UIFrame;
using UnityEngine;

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
    public static void ParseUIPanelTypeJsonData(ref Dictionary<UIPanelType, string> panelPathDict)
    {
        var ta = Resources.Load<TextAsset>("Prefabs/UI/UIPanelType");
        var array = JArray.Parse(ta.text);

        foreach (var item in array)
        {
            var panelType = (UIPanelType) Enum.Parse(typeof(UIPanelType), item["panelType"].ToString());
            var path = item["path"].ToString();
            panelPathDict.Add(panelType, path);
        }
    }


    /// <summary>
    /// 加载资源信息
    /// mode：0  单独的贴图
    /// mode：1  位于精灵集里面（就是一个贴图多个精灵）如果是这种模式，则它在精灵集的名字就是它的索引
    /// </summary>
    /// <param name="spriteInfoDict"></param>
    public static void ParseTileSpritePathJsonData(ref Dictionary<string, TileResourcePath> spriteInfoDict)
    {
        try
        {
            // 先加载要加载的资源路径
            var spriteCatalog = Resources.Load<TextAsset>("TileSprite/TileSpriteCatalog");
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
                    spriteInfoDict.Add(spriteId, new TileResourcePath(spriteId, path,
                        (TileResourcePath.SpriteMode) Enum.Parse(typeof(TileResourcePath.SpriteMode), mode)));
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogWarning("Please check the resource path");
            throw new ResourceException(e.Message);
        }
    }
}