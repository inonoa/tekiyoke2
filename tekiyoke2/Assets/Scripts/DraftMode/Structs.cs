using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Draft
{
    public struct Wind
    {
        public int currentIndex;

        public static Wind Create()
        {
            return new Wind
            {
                currentIndex = 0
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

        static int i = 0;
    }

    public struct Input
    {
        public Vector2 pos;
    }
}
