using System.Collections;
using System.Collections.Generic;
using AlsRitter.Global.Store.Player;
using AlsRitter.Global.Store.Player.Model;
using AlsRitter.PlayerController.FSM;
using UnityEngine;

namespace AlsRitter.V3.PlayerController.FSM {
    public class WalkState : IBaseState {
        public string name => "WalkState";

        public void UpdateHandle(UseStore useStore) {
        }

        public void FixedUpdateHandle(UseStore useStore) {
        }

        public void Enter(UseStore useStore) {
            useStore.basicModel.currentSpeed = useStore.basicModel.speed;
        }

        public void Exit(UseStore useStore) {
        }

    }
}