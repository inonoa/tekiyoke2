using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AnyKeyToStart : MonoBehaviour
{

    public GameObject anykts;
    private int count = 0;

    public readonly int blink = 40;

    private bool readyToGame = false;

    public readonly int curtainV = 40;

    public GameObject curtain;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!readyToGame){
            count++;
            if(count==blink){
                anykts.SetActive(false);
            }
            else if(count==2*blink){
                count = 0;
                anykts.SetActive(true);
            }
            if(Input.anyKeyDown){
                readyToGame = true;
                anykts.SetActive(true);
            }
        }
        else{
            curtain.transform.position += new Vector3(curtainV,0);
            if(curtain.transform.position.x>0){
                SceneManager.LoadScene("StageChoiceScene");
            }
        }
        
    }
}