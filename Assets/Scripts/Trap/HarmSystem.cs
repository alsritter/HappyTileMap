using System.Collections;
using System.Collections.Generic;
using AlsRitter.EventFrame;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class HarmSystem : MonoBehaviour
{
    private readonly EventData harmEvent;
    private GameObject player;

    public HarmSystem()
    {
        harmEvent = EventData.CreateEvent(EventID.Harm);
    }

    public void Awake()
    {
        player = GameObject.FindWithTag("Player");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        player.transform.position = new Vector2(0, 0);
        harmEvent.Send();
    }
}