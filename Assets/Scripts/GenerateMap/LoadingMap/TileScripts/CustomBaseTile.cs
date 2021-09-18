using System.Collections.Generic;
using AlsRitter.GenerateMap.CustomTileFrame;
using AlsRitter.Utilities;
using AlsRitter.V3.CustomTileFrame.TileEffect;
using UnityEngine.Serialization;

namespace AlsRitter.V3.GenerateMap.CustomTileFrame.TileScripts {
    public abstract class CustomBaseTile : UnityEngine.Tilemaps.Tile {
        public string[] effectKeys;
        public string   tileSpriteId;

        [FormerlySerializedAs("model")]
        public DisplayLayer layer;

        // 用来记录这个砖块的属性
        public TileTag[] tags;

        // 里面保存这个砖块所拥有的效果
        protected readonly List<IBaseEffect> effects = new List<IBaseEffect>();

        /**
         * 刷新当前砖块的信息
         */
        public void RefreshTileInfo() {
            // 只有碰撞层的需要加载效果
            if (layer == DisplayLayer.Crash) {
                // 动态加载
                foreach (var effectKey in effectKeys) {
                    effects.Add(GlobalEffectRegistry.instance.GetEffect(effectKey));
                }
            }

            sprite = LoadResourceByIdTool.GetTileSprite(tileSpriteId);
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public void SetPlayer(IPlayer player) {
            // Debug.Log("这是：" + tileId);
            foreach (var effect in effects) {
                effect.ApplyTo(player);
            }
        }
    }
}