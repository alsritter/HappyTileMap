using System.Collections;
using System.Collections.Generic;
using MapDataEntity.Dto;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

/// <summary>
/// 加载 JSON 数据
/// </summary>
public class LoadJsonToTile
{
    public static MapRootDto LoadJsonData()
    {
        // Assets/Resources/Json/testData.json
        var text = Resources.Load<TextAsset>("Json/testData");
        var json = text.text;
        return string.IsNullOrEmpty(json) ? null : JsonConvert.DeserializeObject<MapRootDto>(json);
    }
}