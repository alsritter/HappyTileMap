using Newtonsoft.Json;

namespace AlsRitter.GenerateMap.CustomTileFrame.MapDataEntity.V1.Dto
{
    public class Initial
    {
        /// <summary>
        /// 出生坐标 x
        /// </summary>
        [JsonProperty("x")]
        public int X { get; set; }

        /// <summary>
        /// 出生坐标 y
        /// </summary>
        [JsonProperty("y")]
        public int Y { get; set; }

        /// <summary>
        /// 设置角色速度
        /// </summary>
        [JsonProperty("speed")]
        public int Speed { get; set; }

        /// <summary>
        /// 跑步速度
        /// 计算公式：speed + (speed / runDivisor)
        /// </summary>
        [JsonProperty("runDivisor")]
        public int RunDivisor { get; set; }

        /// <summary>
        /// 角色在空中的移动速度
        /// 计算公式：player.xVelocity * (player.speed / player.jumpSpeedDivisor)
        /// </summary>
        [JsonProperty("jumpSpeedDivisor")]
        public float JumpSpeedDivisor { get; set; }

        /// <summary>
        /// 角色攀爬的速度
        /// </summary>
        [JsonProperty("climbSpeed")]
        public float ClimbSpeed { get; set; }

        /// <summary>
        /// 角色下蹲移动的速度
        /// </summary>
        [JsonProperty("crouchSpeedDivisor")]
        public int CrouchSpeedDivisor { get; set; }

        /// <summary>
        /// 角色跳跃的力
        /// </summary>
        [JsonProperty("jumpForce")]
        public int JumpForce { get; set; }

        /// <summary>
        /// 角色二段跳的力
        /// </summary>
        [JsonProperty("jump2ForceDivisor")]
        public int Jump2ForceDivisor { get; set; }

        /// <summary>
        /// 角色蹬墙跳给的推力
        /// </summary>
        [JsonProperty("climbLateralForce")]
        public int ClimbLateralForce { get; set; }
    }
}