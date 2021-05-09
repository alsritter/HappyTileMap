using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AlsRitter.ExceptionHandler;
using AlsRitter.GenerateMap.CustomTileFrame.MapDataEntity.V1.Dto;
using AlsRitter.GenerateMap.CustomTileFrame.TileScripts;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace AlsRitter.GenerateMap.CustomTileFrame.Tool
{
    public class BuildTileMap : MonoBehaviour
    {
        public Tilemap backgroundMap; // 背景 Map
        public Tilemap crashMap; // 碰撞层 Map
        public Tilemap foregroundMap; // 前景层 Map

        private CustomTile[] orderTiles; // 全部 Tile，Map 用于读取
        
        private void StartCreateMap(MapRootDto mapData)
        {
            ClearMap();
            LoadingTile(mapData);
            StartCoroutine(InitData(mapData));
        }

        /// <summary>
        /// 清除地图当前地图数据
        /// </summary>
        private void ClearMap()
        {
            backgroundMap.ClearAllTiles();
            crashMap.ClearAllTiles();
            foregroundMap.ClearAllTiles();
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

            orderTiles = tiles.ToArray();
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

                            var tile = orderTiles[chunk.Data[i][j]];
                            var pos = new Vector3Int(Convert.ToInt32(x + j), Convert.ToInt32(y + i), 0);
                            switch (tile.model)
                            {
                                case DisplayModel.Background:
                                    backgroundMap.SetTile(pos, tile);
                                    break;
                                case DisplayModel.Crash:
                                    crashMap.SetTile(pos, tile);
                                    break;
                                case DisplayModel.Foreground:
                                    foregroundMap.SetTile(pos, tile);
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
        private static Dictionary<string, TileResourcePath> _spriteInfoDict = new Dictionary<string, TileResourcePath>();

        public static Sprite GetSprite(string name)
        {
            // 如果没有初始化
            if (!_isSpriteInfoDictInit)
            {
                LoadJsonTool.ParseTileSpritePathJsonData(ref _spriteInfoDict);
                _isSpriteInfoDictInit = true;
            }

            _spriteInfoDict.TryGetValue(name, out var saPathInfo);
            Sprite sa = null;

            if (saPathInfo == null)
            {
                sa = Resources.Load<Sprite>(_spriteInfoDict["000"].path);
                var message = $"sprite: \"{name}\" Can't find! Please check the path";
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
            if (sa == null) sa = Resources.Load<Sprite>(_spriteInfoDict["000"].path);

            return sa;
        }

        private void Update()
        {
            if (!Application.isEditor) return;

            if (!Input.GetMouseButtonDown(0)) return;
            var mousePosition = Input.mousePosition;
            var wordPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            var cellPosition = crashMap.WorldToCell(wordPosition);
            //tilemap.SetTile(cellPosition, gameUI.GetSelectColor().colorData.mTile);
            var tb = crashMap.GetTile<CustomBaseTile>(cellPosition);
            if (tb == null)
            {
                return;
            }

            //tb.hideFlags = HideFlags.None;
            Debug.Log($"鼠标坐标：{mousePosition}  世界坐标：{wordPosition}  cell：{cellPosition}");
            tb.effectKeys.ToList().ForEach(Debug.Log);
            tb.tags.ToList().ForEach(x=>Debug.Log(x.ToString()));
        }
    }
}