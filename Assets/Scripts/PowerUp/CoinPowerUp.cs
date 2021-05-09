using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlsRitter.EventFrame;
using AlsRitter.PowerUpSystem;
using UnityEngine;

namespace AlsRitter.PowerUpSystem.Impl
{
    public class CoinPowerUp : PowerUp
    {
        private readonly EventData scoresEvent = EventData.CreateEvent(EventID.Scores);


        protected override void PowerUpPayload() // Checklist item 1
        {
            scoresEvent.Send();
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