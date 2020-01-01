using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePositionManager : MonoBehaviour
{
    GameObject resPos;

    public void Save(){
        MemoryOverDeath.Instance.SavePosition();
        resPos.SetActive(true);
        resPos.transform.position = new Vector3(transform.position.x, transform.position.y, resPos.transform.position.z);
    }

    // Start is called before the first frame update
    void Start()
    {
        resPos = transform.parent.Find("RespawnPosition").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
