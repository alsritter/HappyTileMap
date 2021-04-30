using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.U2D.Animation;
using UnityEngine.U2D;

public class GlobalTileSpriteManage
{
    private static SpriteLibraryAsset sa;

    public static Sprite GetSprite(string name)
    {
        // 首次使用加载
        if (sa == null)
        {
            // 加载 Assets/Resources/TileSprite/TileSpriteAtlas.asset
            // 不用加文件后缀
            sa = Resources.Load<SpriteLibraryAsset>("TileSprite/TileSpriteAtlas");
        }

        return sa.GetSprite("Tiles", name);
    }
}
