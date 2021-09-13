using Newtonsoft.Json;

namespace AlsRitter.GenerateMap.Interface.Do
{
    public class Initial
    {
        public Initial(int x, int y, int speed, int runDivisor, float jumpSpeedDivisor, float climbSpeed, int crouchSpeedDivisor, int jumpForce, int jump2ForceDivisor, int climbLateralForce)
        {
            X = x;
            Y = y;
            Speed = speed;
            RunDivisor = runDivisor;
            JumpSpeedDivisor = jumpSpeedDivisor;
            ClimbSpeed = climbSpeed;
            CrouchSpeedDivisor = crouchSpeedDivisor;
            JumpForce = jumpForce;
            Jump2ForceDivisor = jump2ForceDivisor;
            ClimbLateralForce = climbLateralForce;
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
        public int Speed { get;  }

        /// <summary>
        /// 跑步速度
        /// 计算公式：speed + (speed / runDivisor)
        /// </summary>
        public int RunDivisor { get;   }

        /// <summary>
        /// 角色在空中的移动速度
        /// 计算公式：player.xVelocity * (player.speed / player.jumpSpeedDivisor)
        /// </summary>
        public float JumpSpeedDivisor { get;   }

        /// <summary>
        /// 角色攀爬的速度
        /// </summary>
        public float ClimbSpeed { get;   }

        /// <summary>
        /// 角色下蹲移动的速度
        /// </summary>
        public int CrouchSpeedDivisor { get;  }

        /// <summary>
        /// 角色跳跃的力
        /// </summary>
        public int JumpForce { get;  }

        /// <summary>
        /// 角色二段跳的力
        /// </summary>
        public int Jump2ForceDivisor { get; }

        /// <summary>
        /// 角色蹬墙跳给的推力
        /// </summary>
        public int ClimbLateralForce { get;   }
    }
}