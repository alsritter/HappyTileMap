using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// 用于触发脚下的 Tile
/// </summary>
public class PlayerTriggerTile : MonoBehaviour
{
    public Tilemap tileMap; // 绘制的 TileMap

    private PlayerFSMSystem pm;

    [HideInInspector]
    public GameObject rightFoot;
    [HideInInspector]
    public GameObject leftFoot;
    [HideInInspector]
    public GameObject hand;

    private class WingTiles
    {
        public CustomTile leftTile { get; set; }
        public CustomTile rightTile { get; set; }
    }

    private void Start()
    {
        rightFoot = GameObject.FindGameObjectWithTag("rightFoot");
        leftFoot = GameObject.FindGameObjectWithTag("leftFoot");
        pm = GetComponent<PlayerFSMSystem>();
    }

    // Update is called once per frame
    private void Update()
    {
        var wingTiles = CastUnderFoot();
        if (wingTiles.leftTile != null)
        {
            wingTiles.leftTile.SetPlayer(pm);
        }

        if (wingTiles.rightTile != null)
        {
            wingTiles.rightTile.SetPlayer(pm);
        }
    }

    private WingTiles CastUnderFoot()
    {
        // 左右两边的 CustomTile
        var wingTiles = new WingTiles();
        var leftCell = tileMap.WorldToCell(leftFoot.transform.position);
        var rightCell = tileMap.WorldToCell(rightFoot.transform.position);

        wingTiles.leftTile = tileMap.GetTile<CustomTile>(leftCell);
        wingTiles.rightTile = tileMap.GetTile<CustomTile>(rightCell);

        return wingTiles;
    }
}