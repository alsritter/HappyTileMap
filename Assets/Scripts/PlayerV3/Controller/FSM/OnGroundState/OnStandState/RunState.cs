using System.Collections;
using System.Collections.Generic;
using AlsRitter.EventFrame;
using AlsRitter.EventFrame.CustomEvent;
using AlsRitter.Global.Store.Player;
using AlsRitter.Global.Store.Player.Model;
using UnityEngine;

namespace AlsRitter.V3.PlayerController.FSM {
    public class RunState : IBaseState {
        public string name => "RunState";

        public void UpdateHandle(UseStore useStore) {
        }

        public void FixedUpdateHandle(UseStore useStore) {
            useStore.basicModel.fixHorizon = false;
        }

        public void Enter(UseStore useStore) {
            useStore.basicModel.currentSpeed = useStore.basicModel.runSpeed;
        }

        public void Exit(UseStore useStore) {
        }
    }
}