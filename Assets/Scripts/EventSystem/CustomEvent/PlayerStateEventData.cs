using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace EventFrame.CustomEvent
{
    public class PlayerStateEventData : EventData
    {
        /// <summary>
        /// 存储状态的值
        /// </summary>
        public bool trigger { get; private set; }

        public PlayerStateEventData(EventID eid) : base(eid)
        {
        }

        public void UpdateState(bool state)
        {
            //Debug.Log($"当前 {eid} 的状态为：{trigger}");
            trigger = state;
            Send();
        }
    }
}