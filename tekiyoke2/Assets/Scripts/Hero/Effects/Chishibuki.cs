using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chishibuki : MonoBehaviour
{
    [SerializeField] int fadeinFrames = 10;
    [SerializeField] int chishibukiFrames = 30;
    [SerializeField] int fadeoutFrames = 30;
    Image image;

    bool canChishibuki = true;
    void Start() => image = transform.Find("Image").GetComponent<Image>();
    public IEnumerator StartChishibuki(){

        if(canChishibuki){
            canChishibuki = false;
            image.gameObject.SetActive(true);
            image.color = new Color(1,1,1,0);

            for(int i=0;i<fadeinFrames;i++){
                image.color = new Color(1,1,1, image.color.a + 1.0f/fadeoutFrames);
                yield return null;
            }
            image.color = new Color(1,1,1,1);

            for(int i=0;i<chishibukiFrames;i++) yield return null;

            for(int i=0;i<fadeoutFrames;i++){
                image.color = new Color(1,1,1, image.color.a - 1.0f/fadeoutFrames);
                yield return null;
            }

            image.gameObject.SetActive(false);
            canChishibuki = true;
        }
    }
}
