using System;
using System.Collections.Generic;
using AlsRitter.GenerateMap.CustomTileFrame;
using AlsRitter.GenerateMap.Interface;
using AlsRitter.GenerateMap.Interface.Do;
using AlsRitter.GenerateMap.V1.Do;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UnityEngine;

namespace AlsRitter.GenerateMap.V1 {
    public class LoadingMapData : ILoadingMapData {
        /**
         * 加载本地的地图数据
         */
        private GameMap ParseLocalMapJsonData() {
            // Assets/Resources/Json/testData.json
            // Debug.LogError("The local map is loaded");
            var text = Resources.Load<TextAsset>("Json/exportData");
            var json = text.text;

            var data = JsonConvert.DeserializeObject<ConvertMapData>(json);
            var initial = new Initial(data.Initial.X, data.Initial.Y, data.Initial.Speed, data.Initial.RunDivisor,
                                      (float) data.Initial.JumpSpeedDivisor, (float) data.Initial.ClimbSpeed,
                                      data.Initial.CrouchSpeedDivisor, data.Initial.JumpForce,
                                      data.Initial.Jump2ForceDivisor, data.Initial.ClimbLateralForce);

            var prefabs = new List<PrefabsItem>();
            data.Prefabs.ForEach(x => { prefabs.Add(new PrefabsItem(x.PrefabId, x.X, x.Y)); });

            var tilesData = new List<TileData>();
            data.TileData.ForEach(x => {
                tilesData.Add(new TileData(x.Key,
                                           NumberConvertEnumTool.NumberToLayer(x.Layer),
                                           x.TileSpriteId,
                                           x.Color,
                                           x.EffectKeys.ToArray(),
                                           NumberConvertEnumTool.NumbersToTags(x.Tags).ToArray()));
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

        public GameMap GetGameMapData(string json) {
            // if (json == null) {
            //     return ParseLocalMapJsonData();
            // }
            //
            // throw new NotImplementedException();
            var map = ParseLocalMapJsonData();
            return map;
        }
    }
}