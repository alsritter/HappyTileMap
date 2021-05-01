using System.Collections;
using System.Collections.Generic;
using CommonTileEnum;
using MapDataEntity.Dto;
using UnityEngine;
using UnityEngine.Tilemaps;


/// <summary>
/// 这个 Tile 专门用于测试效果用的，用于实例化的 Tile 是另一个
/// </summary>
[CreateAssetMenu(fileName = "New Tile", menuName = "My Tile/New Test Tile", order = 1)]
public class TestToolTile : CustomBaseTile
{
    private void OnEnable()
    {
        RefreshTileInfo();
    }
}