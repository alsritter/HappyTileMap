using System.Collections;
using System.Collections.Generic;
using AlsRitter.Global.Store.Player;
using UnityEngine;

namespace AlsRitter.V3.PlayerController.FSM
{
    /// <summary>
    /// 下蹲时的走路状态
    /// </summary>
    public class CrouchIdleState : IBaseState
    {
        public string name => "CrouchIdleState";
        public void UpdateHandle(UseStore useStore) {
        }

        public void FixedUpdateHandle(UseStore useStore) {
        }

        public void Enter(UseStore useStore) {
        }

        public void Exit(UseStore useStore) {
        }

    }
}