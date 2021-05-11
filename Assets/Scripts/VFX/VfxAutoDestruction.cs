using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VfxAutoDestruction : MonoBehaviour
{
    public float timeToDestroy = 1;

    private void Start()
    {
        Destroy(gameObject, timeToDestroy);
    }
}