using System;
using System.Collections;
using System.Collections.Generic;
using AlsRitter.GenerateMap.CustomTileFrame;
using AlsRitter.GenerateMap.Interface.Do;
using AlsRitter.V3.GenerateMap.CustomTileFrame.TileScripts;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace AlsRitter.GenerateMap {
    public class BuildTileMap : MonoBehaviour {
        public Tilemap backgroundMap; // 背景 Map
        public Tilemap crashMap; // 碰撞层 Map
        public Tilemap foregroundMap; // 前景层 Map

        private Dictionary<string, CustomTile> tileDictionary; // 全部 Tile，Map 用于读取

        public void StartCreateMap(GameMap mapData) {
            ClearMap();
            LoadingTile(mapData);
            StartCoroutine(InitData(mapData));
        }

        /// <summary>
        /// 清除地图当前地图数据
        /// </summary>
        private void ClearMap() {
            backgroundMap.ClearAllTiles();
            crashMap.ClearAllTiles();
            foregroundMap.ClearAllTiles();
        }

        // ReSharper disable Unity.PerformanceAnalysis
        /**
         * 装载 Tile
         */
        private void LoadingTile(GameMap mapData) {
            var customTiles = mapData.tilesData;
            this.tileDictionary = new Dictionary<string, CustomTile>();

            foreach (var tileData in customTiles) {
                var tile = ScriptableObject.CreateInstance<CustomTile>();
                tile.InitializeCustomTile(tileData.EffectKeys, tileData.TileSpriteId, tileData.layer, tileData.Tags);
                // if (tileData.Color != null) {
                //     ColorUtility.TryParseHtmlString(tileData.Color, out var nowColor);
                //     tile.color = nowColor;
                // }
                tileDictionary.Add(tileData.key, tile);
            }
        }

        /**
        * 地图生成 这里的 IEnumerator 表示这里是协程（不然会导致游戏卡住）
        */
        private IEnumerator InitData(GameMap mapData) {
            // 只加载需要显示的
            foreach (var tile in mapData.tiles) {
                var pos = new Vector3Int(Convert.ToInt32(tile.point.x), Convert.ToInt32(tile.point.y), 0);
                tileDictionary.TryGetValue(tile.key, out var customTile);
                switch (tile.layer) {
                    case DisplayLayer.Background:
                        backgroundMap.SetTile(pos, customTile);
                        break;
                    case DisplayLayer.Crash:
                        crashMap.SetTile(pos, customTile);
                        break;
                    case DisplayLayer.Foreground:
                        foregroundMap.SetTile(pos, customTile);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                yield return null;
            }
        }
    }
}