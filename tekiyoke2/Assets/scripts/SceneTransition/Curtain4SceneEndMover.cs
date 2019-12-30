using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Curtain4SceneEndMover : MonoBehaviour
{
    public string NextSceneName { get; set; }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition += new Vector3(50,0,0);
        if(transform.localPosition.x>250) SceneManager.LoadScene(NextSceneName);
    }
}
