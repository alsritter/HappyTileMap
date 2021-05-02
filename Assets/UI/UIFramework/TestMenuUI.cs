using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UIFrame
{
    public class TestMenuUI : BasePanel
    {
        /// <summary>
        /// 这里能收到这个面板下的所有 Button 的消息
        /// 所以根据名字来判断它的职责
        /// </summary>
        /// <param name="sender"></param>
        public override void DidOnClick(GameObject sender)
        {
            switch (sender.name) {
                case "Start Game Button":
                    Debug.Log("游戏开始了~");
                    break;
                default:
                    break;
            }
        }
    }
}

