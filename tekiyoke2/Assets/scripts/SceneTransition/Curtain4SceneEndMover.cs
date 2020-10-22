using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Curtain4SceneEndMover : MonoBehaviour
{
    public string NextSceneName { get; set; }

    [SerializeField] float gridSize = 50f;

    float time = 0;
    [SerializeField] float secondsPerGrid = 0.025f;

    void Update()
    {
        //unscaledDeltaTimeにすると始めのフレームで凄いデカい値が返ってきてそうなのでこうした
        time += Time.deltaTime / Time.timeScale;

        while(time > secondsPerGrid)
        {
            transform.localPosition += new Vector3(gridSize, 0);
            time -= secondsPerGrid;
        }

        if(gameObject.transform.localPosition.x > 250) SceneManager.LoadScene(NextSceneName);
    }
}
