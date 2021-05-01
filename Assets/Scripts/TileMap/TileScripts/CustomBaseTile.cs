using System.Collections;
using System.Collections.Generic;
using CustomTileFrame.CommonTileEnum;
using PlayerController.FSM;
using TileEffect;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace CustomTileFrame.Tile
{
    public abstract class CustomBaseTile : UnityEngine.Tilemaps.Tile
    {
        public string[] effectKeys;
        public string tileImageId;

        public DisplayModel model;

        // 用来记录这个砖块的属性
        public TileTag[] tags;

        // 里面保存这个砖块所拥有的效果
        protected readonly List<BaseObjectEffect> effects = new List<BaseObjectEffect>();

        public void RefreshTileInfo()
        {
            // 只有碰撞层的需要加载效果
            if (model == DisplayModel.Crash)
            {
                // 动态加载
                foreach (var effectKey in effectKeys)
                {
                    effects.Add(GlobalEffectRegistry.GetEffect(effectKey));
                }
            }

            sprite = GlobalTileSpriteManage.GetSprite(tileImageId);
        }

        public void SetPlayer(PlayerFSMSystem player)
        {
            // Debug.Log("这是：" + tileId);
            foreach (var effect in effects)
            {
                effect.ApplyTo(player);
            }
        }
    }
}

