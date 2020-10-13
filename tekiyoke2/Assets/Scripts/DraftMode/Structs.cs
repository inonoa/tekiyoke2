using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Draft
{
    public struct Wind
    {
        public int currentIndex;
        public float angle;

        public static Wind Create()
        {
            return new Wind
            {
                currentIndex = 0,
                angle = Random.Range(0f, Mathf.PI * 2)
            };
        }
    }

    public struct Node
    {
        public float   time;
        public Vector2 pos;

        public static Node Create(bool isFirst)
        {
            if(isFirst)
            {
                return new Node
                {
                    time = Time.time,
                    pos  = new Vector2
                    (
                        Random.Range(-500, 500),
                        Random.Range(-375, 375)
                    )
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
