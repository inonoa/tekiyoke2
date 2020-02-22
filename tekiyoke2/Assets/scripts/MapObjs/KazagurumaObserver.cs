using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class KazagurumaObserver : MonoBehaviour
{
    [SerializeField] Kazaguruma[] kazagurumas;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(kazagurumas.All(kg => kg.IsRotating)){
            Debug.Log("回った！！");
        }
    }
}
