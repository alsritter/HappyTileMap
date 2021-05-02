using System;
using System.Collections;
using System.Collections.Generic;
using CustomTileFrame.MapDataEntity.Dto;
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
    public static MapRootDto ParseJsonMapData()
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
            var panelType = (UIPanelType)Enum.Parse(typeof(UIPanelType), item["panelType"].ToString());
            var path = item["path"].ToString();
            panelPathDict.Add(panelType, path);
        }
    }
}