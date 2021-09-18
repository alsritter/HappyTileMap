using AlsRitter.Global.Store.Player;
using AlsRitter.Global.Store.Player.Model;
using AlsRitter.V3.GenerateMap.CustomTileFrame.TileScripts;
using AlsRitter.V3.Player;
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
        private IPlayer         player;
        
        //
        // private PlayerFSMSystem pm;

        private class WingTiles {
            public CustomTile leftTile  { get; set; }
            public CustomTile rightTile { get; set; }
        }

        private void Awake() {
            player = GlobalPlayer.GetPlayer();
            view = UseStore.GetStore().viewModel;
        }

        private void Update() {
            var wingTiles = CastUnderFoot();
            if (wingTiles.leftTile != null) {
                wingTiles.leftTile.SetPlayer(player);
            }

            if (wingTiles.rightTile != null) {
                wingTiles.rightTile.SetPlayer(player);
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