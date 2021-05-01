﻿using System;
using System.Collections;
using System.Collections.Generic;
using MapDataEntity.Dto;
using PhysicsEffects;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// 用于读取 JSON 实例化的 Tile
/// </summary>
public class CustomTile : Tile
{
    public string[] effectKeys;
    public string tileImageId;

    public DisplayModel model;

    // 里面保存这个砖块所拥有的效果
    private readonly List<BaseObjectEffect> effects = new List<BaseObjectEffect>();

    public void InitializeMyTileInfo(string[] effectKeys, string tileImageId, DisplayModel model)
    {
        this.effectKeys = effectKeys;
        this.tileImageId = tileImageId;
        this.model = model;
        RefreshTileInfo();
    }

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