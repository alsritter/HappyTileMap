using System;
using System.Collections.Generic;
using AlsRitter.GenerateMap.CustomTileFrame;
using AlsRitter.GenerateMap.Interface;
using AlsRitter.GenerateMap.Interface.Do;
using AlsRitter.GenerateMap.V1.Do;
using Newtonsoft.Json;
using UnityEngine;

namespace AlsRitter.GenerateMap.V1 {
    public class LoadingMapData : ILoadingMapData {
        /**
         * 取得 Game Map 数据
         */
        public GameMap GetGameMapData(string json) {
            return string.IsNullOrEmpty(json) ? ParseLocalMapJsonData() : ParseWebMapJsonData(json);
        }
        
        
        /**
         * 加载本地的地图数据
         */
        private GameMap ParseLocalMapJsonData() {
            // Assets/Resources/Json/testData.json
            // Debug.LogError("The local map is loaded");
            var text = Resources.Load<TextAsset>("LocalMap/exportData");
            var json = text.text;
            return ParseJson(json);
        }

        /**
         * 加载前端的地图数据
         */
        private GameMap ParseWebMapJsonData(string json) {
            return ParseJson(json);
        }

        private GameMap ParseJson(string json) {
            var data = JsonConvert.DeserializeObject<ConvertMapData>(json);
            var initial = new Initial(data.Initial.X, data.Initial.Y, 
                                      (float)data.Initial.Speed, 
                                      (float)data.Initial.JumpMax, 
                                      (float)data.Initial.JumpMin, 
                                      (float)data.Initial.JumpSpeed);

            var prefabs = new List<PrefabsItem>();
            data.Prefabs.ForEach(x => { prefabs.Add(new PrefabsItem(x.PrefabId, x.X, x.Y)); });

            var tilesData = new List<TileData>();
            data.TileData.ForEach(x => {
                tilesData.Add(new TileData(x.Key,
                                           NumberConvertEnumTool.NumberToLayer(x.Layer),
                                           x.TileSpriteId,
                                           x.Color,
                                           x.EffectKeys.ToArray()));
            });

            var tiles = new List<TilePoint>();
            data.Tiles.ForEach(x => {
                tiles.Add(new TilePoint(
                                        x.Key,
                                        new Vector2(x.X, x.Y),
                                        NumberConvertEnumTool.NumberToLayer(x.Layer)));
            });

            var background = new Background(data.Background.BgId, data.Background.Color);

            // return string.IsNullOrEmpty(json) ? null : null;
            return new GameMap(data.CreateTime, data.Version, data.Author, initial, background, prefabs,
                               tilesData, tiles);
        }
    }
}