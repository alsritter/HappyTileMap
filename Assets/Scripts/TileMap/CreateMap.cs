using System;
using System.Collections;
using System.Collections.Generic;
using MapDataEntity.Dto;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class CreateMap : MonoBehaviour
{
    public Tilemap BackgroundMap; // 背景 Map
    public Tilemap CrashMap; // 碰撞层 Map
    public Tilemap ForegroundMap; // 前景层 Map

    private CustomTile[] OrderTiles; // 全部 Tile，Map 用于读取

    private MapRootDto mapDto;

    // Start is called before the first frame update
    private void Start()
    {
        mapDto = LoadJsonToTile.LoadJsonData();
        LoadingTile(mapDto);
        StartCoroutine(InitData(mapDto));
    }

    /// <summary>
    /// 装载 Tile
    /// </summary>
    /// <param name="mapData"></param>
    private void LoadingTile(MapRootDto mapData)
    {
        var tileInfos = mapData.Tiles;

        var tiles = new List<CustomTile>();

        tileInfos.ForEach(tileInfo =>
        {
            var tile = ScriptableObject.CreateInstance<CustomTile>();
            tile.InitializeMyTileInfo(tileInfo.EffectKeys, tileInfo.TileImageId, tileInfo.DisplayModel);
            ColorUtility.TryParseHtmlString(tileInfo.Color, out var nowColor);
            tile.color = nowColor;
            tiles.Add(tile);
        });

        OrderTiles = tiles.ToArray();
    }

    /// <summary>
    /// 地图生成 这里的 IEnumerator 表示这里是协程（不然会导致游戏卡住）
    /// </summary>
    /// <returns></returns>
    private IEnumerator InitData(MapRootDto mapData)
    {
        // 先取出图层数据
        var layerItems = mapData.Layer;
        foreach (var layer in layerItems)
        {
            // 只加载需要显示的
            if (!layer.Show) continue;
            var layerChunks = layer.Chunks;
            foreach (var chunk in layerChunks)
            {
                // 起始位置
                var x = chunk.X;
                var y = chunk.Y;
                
                for (var i = 0; i < mapData.ChunkSizeHeight; i++)
                {
                    for (var j = 0; j < mapData.ChunkSizeWidth; j++)
                    {
                        // 默认 -1 为空
                        if (chunk.Data[i][j] == -1) continue;

                        var tile = OrderTiles[chunk.Data[i][j]];
                        var pos = new Vector3Int(Convert.ToInt32(x + j), Convert.ToInt32(y + i), 0);
                        switch (tile.model)
                        {
                            case DisplayModel.Background:
                                BackgroundMap.SetTile(pos, tile);
                                break;
                            case DisplayModel.Crash:
                                CrashMap.SetTile(pos, tile);
                                break;
                            case DisplayModel.Foreground:
                                ForegroundMap.SetTile(pos, tile);
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }
                    yield return null;
                }
            }
        }


        /*        const int levelW = 10;

        arrTiles = new CustomTile[2];

        for (var i = 0; i < 2; i++)
        {
            arrTiles[i] = ScriptableObject.CreateInstance<CustomTile>(); //创建Tile，注意，要使用这种方式
            arrTiles[i].sprite = sprite;
            arrTiles[i].color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1);
            arrTiles[i].name = "编号为：" + i.ToString();
        }

        //这里就是设置每个Tile的信息了
        for (var j = 0; j < levelW; j++)
        {
            tileMap.SetTile(new Vector3Int(j, -4, 0), arrTiles[Random.Range(0, arrTiles.Length)]);
            yield return null;
        }*/
        yield return null;
    }

    private void Update()
    {
/*        if (!Input.GetMouseButtonDown(0)) return;
        var mousePosition = Input.mousePosition;
        var wordPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        var cellPosition = tileMap.WorldToCell(wordPosition);
        var tb = tileMap.GetTile(cellPosition);
        if (tb == null)
        {
            return;
        }

        //tb.hideFlags = HideFlags.None;
        Debug.Log(
            "鼠标坐标：" + mousePosition +
            "世界：" + wordPosition +
            "cell：" + cellPosition +
            "tb：" + tb.name);*/
    }
}