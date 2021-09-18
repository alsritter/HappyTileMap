using System.Collections;
using System.Collections.Generic;
using AlsRitter.Global.Store.Player;
using UnityEngine;


namespace AlsRitter.V3.PlayerController.FSM
{
    public class JumpState : IBaseState
    {
        public string name => "JumpState";
        
        
        public void UpdateHandle(UseStore useStore) {
        }

        public void FixedUpdateHandle(UseStore useStore) {
        }

        public void Enter(UseStore useStore) {
            // throw new System.NotImplementedException();
            //TODO: 跳跃
        }

        public void Exit(UseStore useStore) {
        }
        
    }
}