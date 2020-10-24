using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Curtain4SceneStartMover : MonoBehaviour
{
    [SerializeField] float gridSize = 46.875f;

    float time = 0;
    [SerializeField] float secondsPerGrid = 0.02f;

    void Update()
    {
        float dt    = TimeManager.CurrentInstance.DeltaTimeExceptHero;
        float scale = TimeManager.CurrentInstance.TimeScaleExceptHero;

        //unscaledDeltaTimeにすると始めのフレームで凄いデカい値が返ってきてそうなのでこうした
        time += dt / scale;

        while(time > secondsPerGrid)
        {
            transform.localPosition += new Vector3(gridSize, 0);
            time -= secondsPerGrid;
        }

        if(gameObject.transform.localPosition.x > 4000) Destroy(gameObject);
    }
}
