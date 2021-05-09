using System.Collections;
using System.Collections.Generic;
using AlsRitter.CustomTileFrame.Tile;
using UnityEditor;
using UnityEngine;


/// <summary>
/// 这个用于刷新修改的 Tile 效果
/// </summary>
[CustomEditor(typeof(TestToolTile))]
public class TileBuilderEditor : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();
        var myTile = (TestToolTile)target;

        if(GUILayout.Button("刷新更改")) {
            myTile.RefreshTileInfo();
        }
    }
}