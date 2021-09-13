using Newtonsoft.Json;

namespace AlsRitter.GenerateMap.Interface.Do
{
    public class PrefabsItem
    {
        public PrefabsItem(string prefabId, int x, int y)
        {
            PrefabId = prefabId;
            X = x;
            Y = y;
        }

        /// <summary>
        /// 预制件的编号
        /// </summary>
        public string PrefabId { get;set; }

        /// <summary>
        /// 位置（这个只是在 Grid 中的位置，需要转成世界坐标）
        /// </summary>
        public int X { get;set; }

        /// <summary>
        /// 位置
        /// </summary>
        public int Y { get;set; }
    }
}
