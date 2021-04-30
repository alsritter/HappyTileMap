using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CreateMap : MonoBehaviour
{
    public Tilemap tileMap; // 绘制的 TileMap
    public Sprite sprite;
    private CustomTile[] arrTiles; //生成的Tile数组

    // Start is called before the first frame update
    private void Start()
    {
        StartCoroutine(InitData());
    }

    /// <summary>
    /// 地图生成 这里的 IEnumerator 表示这里是协程（不然会导致游戏卡住）
    /// </summary>
    /// <returns></returns>
    private IEnumerator InitData()
    {
        const int levelW = 10;

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
        }
    }

    private void Update()
    {
        if (!Input.GetMouseButtonDown(0)) return;
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
            "tb：" + tb.name);
    }
}