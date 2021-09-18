using Newtonsoft.Json;

namespace AlsRitter.GenerateMap.Interface.Do
{
    public class Initial
    {
        public Initial(int x, int y, float speed, float jumpMax, float jumpMin, float jumpSpeed)
        {
            X = x;
            Y = y;
            Speed = speed;
            JumpMax = jumpMax;
            JumpMin = jumpMin;
            JumpSpeed = jumpSpeed;
        }

        /// <summary>
        /// 出生坐标 x
        /// </summary>
        public int X { get;  }

        /// <summary>
        /// 出生坐标 y
        /// </summary>
        public int Y { get;  }

        /// <summary>
        /// 设置角色速度
        /// </summary>
        public float Speed { get;  }

        /// <summary>
        /// 角色跳跃的最大高度
        /// </summary>
        public float JumpMax { get;   }

        /// <summary>
        /// 角色跳跃的最小高度
        /// </summary>
        public float JumpMin { get;   }

        /// <summary>
        /// 角色跳跃的速度
        /// </summary>
        public float JumpSpeed { get;  }
    }
}