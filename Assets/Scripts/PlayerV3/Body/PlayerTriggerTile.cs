using System.Collections;
using System.Collections.Generic;
using AlsRitter.GenerateMap.CustomTileFrame.TileScripts;
using AlsRitter.Global.Store.Player;
using AlsRitter.Global.Store.Player.Model;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace AlsRitter.V3.PlayerController {
    /// <summary>
    /// 用于触发脚下的 Tile
    /// </summary>
    [DisallowMultipleComponent]
    public class PlayerTriggerTile : MonoBehaviour {
        public  Tilemap         tileMap; // 绘制的 TileMap
        private PlayerViewModel view;
        //
        // private PlayerFSMSystem pm;

        private class WingTiles {
            public CustomTile leftTile  { get; set; }
            public CustomTile rightTile { get; set; }
        }

        private void Awake() {
            view = UseStore.GetStore().viewModel;
        }

        private void Update() {
            var wingTiles = CastUnderFoot();
            if (wingTiles.leftTile != null) {
                // wingTiles.leftTile.SetPlayer(pm);
            }

            if (wingTiles.rightTile != null) {
                // wingTiles.rightTile.SetPlayer(pm);
            }
        }

        private WingTiles CastUnderFoot() {
            // 左右两边的 CustomTile
            var wingTiles = new WingTiles();

            var leftCell = tileMap.WorldToCell(view.leftFoot.transform.position);
            var rightCell = tileMap.WorldToCell(view.rightFoot.transform.position);

            wingTiles.leftTile = tileMap.GetTile<CustomTile>(leftCell);
            wingTiles.rightTile = tileMap.GetTile<CustomTile>(rightCell);

            return wingTiles;
        }
    }
}