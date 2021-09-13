using AlsRitter.GenerateMap.Interface;
using AlsRitter.GenerateMap.Interface.Do;
using AlsRitter.Utilities;
using UnityEngine;

namespace AlsRitter.GenerateMap {
    /// <summary>
    /// 用于生成预制件
    /// </summary>
    public class BuildMapProp : MonoBehaviour {
        // 存放所有 prop 的父节点
        public GameObject props;

        public void StartCreateProps(GameMap mapData) {
            ClearProp();
            LoadingProps(mapData);
        }

        private void ClearProp() {
            // 清空 Props 里面的内容
            for (var i = 0; i < props.transform.childCount; i++) {
                Destroy(props.transform.GetChild(i).gameObject);
            }
        }

        private void LoadingProps(GameMap mapData) {
            mapData.prefabs.ForEach(x => {
                // var o = Instantiate(LoadResourceByIdTool.GetProp(x.PrefabId));
                // o.transform.SetParent(Props.transform);
                // // 因为原物品可能有坐标
                // o.transform.position = new Vector3(o.transform.position.x + x.X, o.transform.position.y + x.Y, 0);

                var o = Instantiate(LoadResourceByIdTool.GetProp(x.PrefabId), props.transform, true);
                // 因为原物品可能有坐标
                var position = o.transform.position;
                position = new Vector3(position.x + x.X, position.y + x.Y, 0);
                o.transform.position = position;
            });
        }
    }
}