using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePositionManager : MonoBehaviour
{
    GameObject resPos;

    [SerializeField]
    int saveCostDP = 10;

    public void Try2Save(){
        if(DPManager.Instance.DP >= saveCostDP){
            print("DPを" + saveCostDP + "消費し情報をセーブします");
            DPManager.Instance.UseDP(saveCostDP);
            MemoryOverDeath.Instance.Save();
            resPos.SetActive(true);
            resPos.transform.position = new Vector3(transform.position.x, transform.position.y, resPos.transform.position.z);
        }else{
            print("セーブに失敗しました。");
        }
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
