using System.Collections;
using System.Collections.Generic;
using AlsRitter.EventFrame;
using UnityEngine;

namespace AlsRitter.V3.Sundry
{
    public class PlayerVfXController : MonoBehaviour, IEventObserver
    {
        public GameObject injured;

        private void Awake()
        {
            EventManager.Register(this, EventID.Harm);
        }

        public void HandleEvent(EventData resp)
        {
            switch (resp.eid)
            {
                case EventID.Harm:
                    //injured.Play();
                    Instantiate(injured, transform.position, Quaternion.identity);
                    break;
            }
        }

        public void OnDestroy() {
            EventManager.Remove(this);
        }
    }
}