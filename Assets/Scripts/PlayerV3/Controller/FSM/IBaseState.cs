using System.Collections;
using System.Collections.Generic;
using AlsRitter.Global.Store.Player;
using UnityEngine;


namespace AlsRitter.V3.PlayerController.FSM {
    /// <summary>
    /// 状态接口
    /// </summary>
    public interface IBaseState {
        string name { get; }
        
        void UpdateHandle(UseStore useStore);

        void FixedUpdateHandle(UseStore useStore);

        void Enter(UseStore useStore);

        void Exit(UseStore useStore);
    }
}