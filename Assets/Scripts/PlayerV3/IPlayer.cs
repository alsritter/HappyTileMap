namespace AlsRitter.V3.CustomTileFrame.TileEffect {
    
    /**
     * 给效果提供的操作用户接口
     */
    public interface IPlayer {
        /**
         * 向左移动
         */
        void MoveLeft();

        /**
         * 向右移动
         */
        void MoveRight();

        /**
         * 跳跃
         */
        void Jump(float jumpDynamics);
    }
}