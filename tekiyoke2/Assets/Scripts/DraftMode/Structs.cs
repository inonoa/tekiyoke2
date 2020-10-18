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

        public enum State : int{ Rotating, GoingStraight }
        public State state;
        public float timeOffsetForState;
        public float goStraightSec;
        public float rotateSec;

        public static Wind Create(Params params_)
        {
            return new Wind
            {
                currentIndex       = 0,
                angle              = params_.angle.GenerateRandom(),
                velocity           = params_.velocity.GenerateRandom(),
                state              = State.GoingStraight,
                timeOffsetForState = params_.timeOffset.GenerateRandom(),
                goStraightSec      = params_.goStraightSec.GenerateRandom(),
                rotateSec          = params_.rotateSec.GenerateRandom()
            };
        }

        [Serializable]
        public class Params
        {
            public MinMaxFloat angle         = new MinMaxFloat{ min = 0, max = 360 };
            public MinMaxFloat velocity      = new MinMaxFloat{ min = 200, max = 600 };
            public MinMaxFloat timeOffset    = new MinMaxFloat{ min = 0, max = 3 };
            public MinMaxFloat goStraightSec = new MinMaxFloat{ min = 2, max = 4 };
            public MinMaxFloat rotateSec     = new MinMaxFloat{ min = 0.5f, max = 1.3f };
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

    [Serializable]
    public class MinMaxFloat
    {
        public float min;
        public float max;

        public float GenerateRandom()
        {
            return Random.Range(min, max);
        }
    }
}
