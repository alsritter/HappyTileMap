using AlsRitter.GenerateMap.Interface.Do;

namespace AlsRitter.GenerateMap.Interface {
    /**
     * 所有的地图解析工具都要实现这个接口
     */
    public interface ILoadingMapData {
        /**
         * 
         */
        GameMap GetGameMapData(string json);
    }
}