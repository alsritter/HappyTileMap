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
            bg.sprite = GetSprite(mapData.Background.BgId);
            ColorUtility.TryParseHtmlString(mapData.Background.Color, out var color);
            bg.color = color;
        }

        // 用于表示它是否初始化
        private bool isSpriteInfoDictInit = false;
        // 这里用于装载背景图片
        private Dictionary<string, string> spriteInfoDict = new Dictionary<string, string>();

        public Sprite GetSprite(string bgId)
        {
            // 如果没有初始化
            if (!isSpriteInfoDictInit)
            {
                LoadJsonTool.ParseBackgroundPathJsonData(ref spriteInfoDict);
                isSpriteInfoDictInit = true;
            }

            // 先判断当前传入的 key 是否为空
            spriteInfoDict.TryGetValue(bgId, out var saPathInfo);

            Sprite sa = null;
            if (saPathInfo == null)
            {
                // 加载错误贴图
                sa = Resources.Load<Sprite>(spriteInfoDict["000"]);
                var message = $"sprite: \"{bgId}\" Can't find! Please check whether the key exists";
                Debug.LogWarning(message);
                throw new ResourceException(message);
            }

            sa = Resources.Load<Sprite>(spriteInfoDict[bgId]);

            // 找不到也返回错误贴图
            if (sa == null) sa = Resources.Load<Sprite>(spriteInfoDict["000"]);

            return sa;
        }
    }
}