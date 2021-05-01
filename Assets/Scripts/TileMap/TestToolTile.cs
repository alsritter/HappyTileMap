using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


/// <summary>
/// 这个 Tile 专门用于测试效果用的，用于实例化的 Tile 是另一个
/// </summary>
[CreateAssetMenu(fileName = "New Tile", menuName = "My Tile/New Test Tile", order = 1)]
public class TestToolTile : Tile
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
    /// 注意，内部也有个 RefreshTile 方法，不用无意中重写了，VS2019 没有提示的...
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
            Debug.Log(effect.name);
            effect.ApplyTo(player);
        }
    }
}