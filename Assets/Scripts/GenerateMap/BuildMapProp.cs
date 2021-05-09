using AlsRitter.GenerateMap.CustomTileFrame.MapDataEntity.V1.Dto;
using AlsRitter.Utilities;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace AlsRitter.GenerateMap
{
    /// <summary>
    /// 用于生成预制件
    /// </summary>
    public class BuildMapProp: MonoBehaviour
    {
        // 存放所有 prop 的父节点
        public GameObject Props;

        public void StartCreateProps(MapRootDto mapData)
        {
            ClearProp();
            LoadingProps(mapData);
        }


        private void ClearProp()
        {
            // 清空 Props 里面的内容
            for (var i = 0; i < Props.transform.childCount; i++) {  
                Destroy(Props.transform.GetChild (i).gameObject);  
            } 
        }

        private void LoadingProps(MapRootDto mapData)
        {
            mapData.Prefabs.ForEach(x =>
            {
                var o = Instantiate(LoadResourceByIdTool.GetProp(x.PrefabId));
                o.transform.parent = Props.transform;
                // 因为原物品可能有坐标
                o.transform.position = new Vector3(o.transform.position.x + x.X, o.transform.position.y + x.Y, 0);
            });
        }
        
    }
}