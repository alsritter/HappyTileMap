using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventFrame;
using PowerUpSystem;
using UnityEngine;

namespace Assets.Scripts.PowerUpSystem
{
    public class TestPowerUp : PowerUp
    {
        private EventData eventData = EventData.CreateEvent(EventID.Scores);
        

        protected override void PowerUpPayload() // Checklist item 1
        {
            eventData.Send();
            base.PowerUpPayload();
            //playerBrain.SetSpeedBoostOn(speedMultiplier);
        }

        protected override void PowerUpHasExpired() // Checklist item 2
        {
            // playerBrain.SetSpeedBoostOff();
            base.PowerUpHasExpired();
        }
    }
}