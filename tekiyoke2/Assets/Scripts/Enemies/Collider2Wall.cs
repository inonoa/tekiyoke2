using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Collider2Wall : MonoBehaviour
{
    public event EventHandler touched2Wall;

    public bool IsTouched2Wall => touchCount > 0;
    int touchCount = 0;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag(Tags.Terrain))
        {
            touched2Wall?.Invoke(this,EventArgs.Empty);
            touchCount ++;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag(Tags.Terrain))
        {
            touchCount --;
        }
    }
}
