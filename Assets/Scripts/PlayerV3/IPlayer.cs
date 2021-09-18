using UnityEngine;

namespace AlsRitter.V3.Player {
    /**
     * 给效果提供的操作用户接口，这个效果不应该直接依托于具体的实现类
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

        /**
         * 返回 Player 对象自己
         */
        GameObject PlayerSelf();

        /**
         * 设置速度
         */
        void SetSpeed(float speed);

        /**
         * 设置角色位置
         */
        void SetPos(Vector3 pos);

        /**
         * 取得当前角色的位置
         */
        Vector3 GetPos();

        /**
         * 取得精灵渲染器
         */
        SpriteRenderer GetSpriteRenderer();

        /**
         * 取得角色刚体
         */
        Rigidbody2D GetRigidbody2D();
    }
}