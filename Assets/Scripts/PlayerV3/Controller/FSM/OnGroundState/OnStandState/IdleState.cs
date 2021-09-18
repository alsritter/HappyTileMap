using System.Collections;
using System.Collections.Generic;
using AlsRitter.Global.Store.Player;
using AlsRitter.Global.Store.Player.Model;
using UnityEngine;

namespace AlsRitter.V3.PlayerController.FSM
{
    /// <summary>
    /// 站立的待机状态
    /// </summary>
    public class IdleState : IBaseState
    {
        public string name => "IdleState";
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