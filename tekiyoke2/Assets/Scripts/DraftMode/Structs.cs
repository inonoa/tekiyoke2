using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Draft
{
    public struct Wind
    {
        public int currentIndex;
        public float angle;
        public float velocity;
        public float timeOffset;

        public static Wind Create()
        {
            return new Wind
            {
                currentIndex = 0,
                angle        = Random.Range(Mathf.PI * 0.85f, Mathf.PI * 1.25f),
                velocity     = Random.Range(200f, 600f),
                timeOffset   = Random.Range(0f, 4f)
            };
        }
    }

    public struct Node
    {
        public float   time;
        public Vector2 pos;
        Color color;

        public static Node Create(bool isFirst)
        {
            if(isFirst)
            {
                (float h, float s, float v) hsv =
                (
                    Random.Range(0.4f, 0.75f),
                    1f,
                    1f
                );
                Color col = Color.HSVToRGB(hsv.h, hsv.s, hsv.v);

                return new Node
                {
                    time = Time.time,
                    pos  = new Vector2
                    (
                        Random.Range(-500, 500),
                        Random.Range(-375, 375)
                    ),
                    color = col
                };
            }
            return new Node{ time = -1 };
        }
    }

    public struct Input
    {
        public Vector2 pos;
    }
}
