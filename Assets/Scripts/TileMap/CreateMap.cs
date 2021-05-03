using System;
using System.Collections;
using System.Collections.Generic;
using CustomTileFrame.CommonTileEnum;
using CustomTileFrame.MapDataEntity.Dto;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace CustomTileFrame.Tool
{
    public class CreateMap : MonoBehaviour
    {
        public Tilemap BackgroundMap; // 背景 Map
        public Tilemap CrashMap; // 碰撞层 Map
        public Tilemap ForegroundMap; // 前景层 Map

        private CustomTile[] OrderTiles; // 全部 Tile，Map 用于读取

        private MapRootDto mapDto;

        // Start is called before the first frame update
        private void Start()
        {
            mapDto = LoadJsonTool.ParseMapJsonData();
            LoadingTile(mapDto);
            StartCoroutine(InitData(mapDto));
        }

        /// <summary>
        /// 装载 Tile
        /// </summary>
        /// <param name="mapData"></param>
        private void LoadingTile(MapRootDto mapData)
        {
            var tileInfos = mapData.Tiles;

            var tiles = new List<CustomTile>();

            tileInfos.ForEach(tileInfo =>
            {
                var tile = ScriptableObject.CreateInstance<CustomTile>();
                tile.InitializeMyTileInfo(tileInfo.EffectKeys, tileInfo.TileSpriteId, tileInfo.DisplayModel,
                    tileInfo.Tags);
                ColorUtility.TryParseHtmlString(tileInfo.Color, out var nowColor);
                tile.color = nowColor;
                tiles.Add(tile);
            });

            OrderTiles = tiles.ToArray();
        }

        /// <summary>
        /// 地图生成 这里的 IEnumerator 表示这里是协程（不然会导致游戏卡住）
        /// </summary>
        /// <returns></returns>
        private IEnumerator InitData(MapRootDto mapData)
        {
            // 先取出图层数据
            var layerItems = mapData.Layer;
            foreach (var layer in layerItems)
            {
                // 只加载需要显示的
                if (!layer.Show) continue;
                var layerChunks = layer.Chunks;
                foreach (var chunk in layerChunks)
                {
                    // 起始位置
                    var x = chunk.X;
                    var y = chunk.Y;

                    for (var i = 0; i < mapData.ChunkSizeHeight; i++)
                    {
                        for (var j = 0; j < mapData.ChunkSizeWidth; j++)
                        {
                            // 默认 -1 为空
                            if (chunk.Data[i][j] == -1) continue;

                            var tile = OrderTiles[chunk.Data[i][j]];
                            var pos = new Vector3Int(Convert.ToInt32(x + j), Convert.ToInt32(y + i), 0);
                            switch (tile.model)
                            {
                                case DisplayModel.Background:
                                    BackgroundMap.SetTile(pos, tile);
                                    break;
                                case DisplayModel.Crash:
                                    CrashMap.SetTile(pos, tile);
                                    break;
                                case DisplayModel.Foreground:
                                    ForegroundMap.SetTile(pos, tile);
                                    break;
                                default:
                                    throw new ArgumentOutOfRangeException();
                            }
                        }

                        yield return null;
                    }
                }
            }
        }

        // 用于表示它是否初始化
        private static bool _isSpriteInfoDictInit = false;
        // 这里用于装载地图
        private static Dictionary<string, string> _spriteInfoDict = new Dictionary<string, string>();

        public static Sprite GetSprite(string name)
        {
            // 如果没有初始化
            if (!_isSpriteInfoDictInit)
            {
                LoadJsonTool.ParseTileSpritePathJsonData(ref _spriteInfoDict);
                _isSpriteInfoDictInit = true;
            }

            _spriteInfoDict.TryGetValue(name, out var saPath);

            Sprite sa = null;

            // 首次使用加载
            // 加载 Assets/Resources/TileSprite/TileSpriteAtlas.asset
            // 不用加文件后缀
            sa = Resources.Load<Sprite>(saPath ?? _spriteInfoDict["000"]);
            // 找不到也返回错误贴图
            if (sa == null) sa = Resources.Load<Sprite>(_spriteInfoDict["000"]);

            return sa;
        }
    }
}