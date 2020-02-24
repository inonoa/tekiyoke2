using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlurTrans : MonoBehaviour
{
    [SerializeField] int framesToBlur = 10;
    [SerializeField] float distanceMax = 0.1f;

    Material material;

    void Start()
    {
        material = GetComponent<Image>().material;
        StartCoroutine("Blur");
    }

    IEnumerator Blur(){
        for(int i=0; i<framesToBlur; i++){
            float distRate = 1 - ((i+1f) / framesToBlur - 1) * ((i+1f) / framesToBlur - 1);
            material.SetFloat( "_Distance", distanceMax * distRate );
            yield return null;
        }
    }
}
