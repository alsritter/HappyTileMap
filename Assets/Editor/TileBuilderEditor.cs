using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


/// <summary>
/// 这个用于刷新修改的 Tile 效果
/// </summary>
[CustomEditor(typeof(CustomTile))]
public class TileBuilderEditor : Editor
{
    public override void OnInspectorGUI() {
        DrawDefaultInspector();
        var myTile = (CustomTile)target;

        if(GUILayout.Button("刷新效果")) {
            myTile.RefreshTileInfo();
        }
    }
}