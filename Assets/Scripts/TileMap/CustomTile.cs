using System;
using System.Collections;
using System.Collections.Generic;
using PhysicsEffects;
using UnityEngine;
using UnityEngine.Tilemaps;


[CreateAssetMenu(fileName = "New Tile", menuName = "My Tile/New Tile", order = 1)]
public class CustomTile : Tile
{
    [Header("当前拥有的效果")]
    public string[] effectKeys = { };
    [Header("Tile 贴图名字")]
    public string tileImageId;

    // 里面保存这个砖块所拥有的效果
    private readonly List<BaseObjectEffect> effects = new List<BaseObjectEffect>();

    private void OnEnable()
    {
        RefreshTileInfo();
    }

    /// <summary>
    /// 注意，内部也有个 RefreshTile 方法
    /// </summary>
    public void RefreshTileInfo()
    {
        // 动态加载
        foreach (var effectKey in effectKeys)
        {
            effects.Add(GlobalEffectRegistry.GetEffect(effectKey));
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