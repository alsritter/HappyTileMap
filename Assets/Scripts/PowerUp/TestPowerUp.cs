using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PowerUpSystem;
using UnityEngine;

namespace Assets.Scripts.PowerUpSystem
{
    public class TestPowerUp : PowerUp
    {
        [Range(1.0f, 4.0f)]
        public float speedMultiplier = 2.0f;

        protected override void PowerUpPayload() // Checklist item 1
        {
            Debug.Log($"调用了参数： {speedMultiplier}");
            base.PowerUpPayload();
            //playerBrain.SetSpeedBoostOn(speedMultiplier);
        }

        protected override void PowerUpHasExpired() // Checklist item 2
        {
            Debug.Log("执行了过期方法");
            // playerBrain.SetSpeedBoostOff();
            base.PowerUpHasExpired();
        }
    }
}