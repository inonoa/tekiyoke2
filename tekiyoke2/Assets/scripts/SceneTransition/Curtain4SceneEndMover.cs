using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Sirenix.OdinInspector;
using UniRx;

public class Curtain4SceneEndMover : MonoBehaviour
{
    public string NextSceneName { get; set; }

    public IObservable<Unit> OnMoveEnd => _OnMoveEnd;
    Subject<Unit> _OnMoveEnd = new Subject<Unit>();

    [SerializeField] float gridSize = 50f;

    [SerializeField] [ReadOnly] float time = 0;
    [SerializeField] float secondsPerGrid = 0.025f;

    void Update()
    {
        float dt    = Time.deltaTime;
        float scale = Time.timeScale;

        //unscaledDeltaTimeにすると始めのフレームで凄いデカい値が返ってきてそうなのでこうした
        time += dt / scale;

        while(time > secondsPerGrid)
        {
            transform.localPosition += new Vector3(gridSize, 0);
            time -= secondsPerGrid;
        }

        if(gameObject.transform.localPosition.x >= 250) _OnMoveEnd.OnNext(Unit.Default);
    }
}
