using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlsRitter.ExceptionHandler;
using AlsRitter.GenerateMap.CustomTileFrame.MapDataEntity.V1.Dto;
using AlsRitter.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace AlsRitter.GenerateMap
{
    /// <summary>
    /// 之所以单独抽离出来这个背景生成器是因为，以后可能要构建多层背景
    ///
    /// TODO: 构建多层背景~
    /// </summary>
    public class BuildBackground : MonoBehaviour
    {
        public Image bg;

        public void StartSetBackground(MapRootDto mapData)
        {
            bg.sprite = LoadResourceByIdTool.GetBackgroundSprite(mapData.Background.BgId);
            ColorUtility.TryParseHtmlString(mapData.Background.Color, out var color);
            bg.color = color;
        }
    }
}