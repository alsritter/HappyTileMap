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
        // 加载 Assets/Resources/TileSprite/TileSpriteAtlas.asset
        // 不用加文件后缀
        if (sa == null) sa = Resources.Load<SpriteLibraryAsset>("TileSprite/TileSpriteAtlas");


        var s = sa.GetSprite("Tiles", name);
        if (s == null)
        {
            s = sa.GetSprite("Tiles", "000");
        }

        return s;
    }
}
