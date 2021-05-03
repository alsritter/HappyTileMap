using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PowerUpSystem;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PowerUpSystem
{
    public interface IPowerUpEvents : IEventSystemHandler
    {
        void OnPowerUpCollected (PowerUp powerUp, GameObject player);

        void OnPowerUpExpired (PowerUp powerUp, GameObject player);
    }
}
