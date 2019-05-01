using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage3 : MonoBehaviour
{
    private static readonly int animtime = 10;
    private float orig_x;
    private float orig_y;
    private static readonly float dx = 16;//16
    private static readonly float dy = 22;//42
    private float[] vxs = new float[animtime];
    private float[] vys = new float[animtime];
    private int upcount = animtime;
    private int downcount = animtime;
    private bool isSelected = false;
    public bool IsSelected{
        get{
            return isSelected;
        }
        set{
            if(isSelected==false&&value==true){
                sr.sprite = onSprite;
                // アニメ開始
                transform.position = new Vector3(orig_x+dx,orig_y+dy,-1);
                upcount = 0;
            }
            else if(isSelected==true&&value==false){
                sr.sprite = offSprite;
                // アニメ開始
                transform.position = new Vector3(orig_x-dx,orig_y-dy,-1);
                downcount = 0;
            }
            isSelected = value;
        }
    }

    public Sprite offSprite;
    public Sprite onSprite;
    public Sprite selectedSprite;
    public SpriteRenderer sr;
    
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        for(int i=0;i<animtime;i++){
            vxs[i] = (2*i - 2*animtime +1) * dx /animtime /animtime;
            vys[i] = (2*i - 2*animtime +1) * dy /animtime /animtime;
        }
        orig_x = transform.position.x;
        orig_y = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        if(isSelected && (Input.GetKeyDown(KeyCode.Return)||Input.GetKeyDown(KeyCode.Z))){
            sr.sprite = selectedSprite;
        }
        if(upcount<animtime){
            transform.position += new Vector3(vxs[upcount],vys[upcount]);
            upcount ++;
        }
        if(downcount<animtime){
            transform.position -= new Vector3(vxs[downcount],vys[downcount]);
            downcount ++;
        }
    }
}
