using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

namespace Draft
{
    public struct Wind
    {
        public int currentIndex;
        public float angle;
        public float velocity;
        public float timeOffset;

        public static Wind Create(Params params_)
        {
            return new Wind
            {
                currentIndex = 0,
                angle        = Random.Range(params_.angleDegMin * Mathf.Deg2Rad, params_.angleDegMax * Mathf.Deg2Rad),
                velocity     = Random.Range(params_.velocityMin,                 params_.velocityMax),
                timeOffset   = Random.Range(params_.timeOffsetMin,               params_.timeOffsetMax)
            };
        }

        [Serializable]
        public class Params
        {
            public float angleDegMin = 160;
            public float angleDegMax = 220;
            public float velocityMin = 200f;
            public float velocityMax = 600f;
            public float timeOffsetMin = 0;
            public float timeOffsetMax = 4;
        }
    }

    public struct Node
    {
        public float   time;
        public Vector2 pos;
        Color color;

        public static Node Create(bool isFirst, Vector2 area)
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
                        Random.Range(- area.x / 2, area.x / 2),
                        Random.Range(- area.y / 2, area.y / 2)
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
