using System.Collections;
using System.Collections.Generic;
using AlsRitter.PlayerController.FSM;
using UnityEngine;

namespace AlsRitter.TileEffect
{
    public abstract class BaseObjectEffect
    {
        public abstract void ApplyTo(PlayerFSMSystem player);

        public abstract int versionUID { get; } // 用于做版本控制，如果子类更新了构造函数，需要更新这个版本号

        public abstract string name { get; }
    }
}

