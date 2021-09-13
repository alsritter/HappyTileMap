using AlsRitter.GenerateMap.Interface.Do;
using AlsRitter.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace AlsRitter.GenerateMap {
    /**
     * 之所以单独抽离出来这个背景生成器是因为，以后可能要构建多层背景
     *
     * TODO: 构建多层背景~
     */
    public class BuildBackground : MonoBehaviour {
        public Image bg;

        public void StartSetBackground(GameMap mapData) {
            bg.sprite = LoadResourceByIdTool.GetBackgroundSprite(mapData.background.BgId);
            ColorUtility.TryParseHtmlString(mapData.background.Color, out var color);
            bg.color = color;
        }
    }
}