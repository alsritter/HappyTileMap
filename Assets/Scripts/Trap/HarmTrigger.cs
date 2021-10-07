using System.Collections;
using System.Collections.Generic;
using AlsRitter.EventFrame;
using UnityEngine;


namespace AlsRitter.Sundry {
    [RequireComponent(typeof(Collider2D))]
    public class HarmTrigger : MonoBehaviour {
        public float waitTile = 3; // 首次碰到触发伤害，下次再被触发的间隔

        private readonly EventData harmEvent;
        private          float     tempTime = 0;

        public HarmTrigger() {
            harmEvent = EventData.CreateEvent(EventID.Harm);
        }

        private void OnTriggerEnter2D(Collider2D other) {
            if (other.gameObject.CompareTag("Player")) {
                if (Time.time > tempTime) {
                    tempTime = Time.time + waitTile;
                    harmEvent.Send();
                }
            }
        }
    }
}