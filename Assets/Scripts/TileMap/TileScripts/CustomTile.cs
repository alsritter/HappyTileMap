using System;
using System.Collections;
using System.Collections.Generic;
using CommonTileEnum;
using MapDataEntity.Dto;
using PhysicsEffects;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// 用于读取 JSON 实例化的 Tile
/// </summary>
public class CustomTile : CustomBaseTile
{
    /// <summary>
    /// 用于传入初始参数的工具
    /// </summary>
    /// <param name="effectKeys0"></param>
    /// <param name="tileImageId0"></param>
    /// <param name="model0"></param>
    public void InitializeMyTileInfo(string[] effectKeys0, string tileImageId0, DisplayModel model0, TileTag[] tags0)
    {
        this.effectKeys = effectKeys0;
        this.tileImageId = tileImageId0;
        this.model = model0;
        this.tags = tags0;
        RefreshTileInfo();
    }
}