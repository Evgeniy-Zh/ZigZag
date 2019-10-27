using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour, IPooledObject
{
    private bool falling = false;
    public bool FirstInSector = false;

    private void OnEnable()
    {
        falling = false;
    }
    void Update()
    {
        if (falling)
        {
            var dt = Time.deltaTime;
            transform.Translate(0, -dt * 5, 0);
        }
    }

    public void Drop()
    {
        falling = true;
    }

    public void OnObjectSpawned()
    {
        falling = false;
        FirstInSector = false;
    }
}
