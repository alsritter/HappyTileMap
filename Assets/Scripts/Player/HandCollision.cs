using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlsRitter.PlayerController
{
    public class HandCollision : MonoBehaviour
    {
        public bool handIsTrigger = false;

        // 开始接触
        private void OnTriggerEnter2D(Collider2D  collider)
        {
            handIsTrigger = true;
        }

        // 接触持续中
        private void OnTriggerStay2D(Collider2D collider)
        {
            handIsTrigger = true;
        }

        // 接触结束
        private void OnTriggerExit2D(Collider2D collider)
        {
            handIsTrigger = false;
        }
    }
}

