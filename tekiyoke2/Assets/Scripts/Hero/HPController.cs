using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Sirenix.OdinInspector;
using System.Linq;
using DG.Tweening;
using UniRx;

public class HPController : MonoBehaviour
{
    const string DEP = "依存";
    [FoldoutGroup(DEP), SerializeField] Sprite img3;
    [FoldoutGroup(DEP), SerializeField] Sprite img2;
    [FoldoutGroup(DEP), SerializeField] Sprite img1;
    [FoldoutGroup(DEP), SerializeField] Sprite img0;

    [Space(10)]
    [FoldoutGroup(DEP), SerializeField] Sprite img3_2;
    [FoldoutGroup(DEP), SerializeField] Sprite img2_1;
    [FoldoutGroup(DEP), SerializeField] Sprite img1_0;

    [Space(10)]
    [FoldoutGroup(DEP), SerializeField] Sprite img2_3;
    [FoldoutGroup(DEP), SerializeField] Sprite img1_2;

    [Space(10)]
    [FoldoutGroup(DEP), SerializeField] Image image;

    private static int max_hp = 3; //正直3で決め打ってるし必要ある？
    private bool isDamaging = false;
    private float secondsAfterDamage = 0;

    private float secondsAfterRecover = 0;
    [SerializeField] float mutekiSecondsAfterDamage = 1.6f;


    [SerializeField, ReadOnly, InlineProperty] string[] mutekiFilters = new string[0];
    public bool CanBeDamaged => mutekiFilters.Length == 0;
    public void AddMutekiFilter(string key)
    {
        if(mutekiFilters.Contains(key)) return;
        mutekiFilters = mutekiFilters.Concat(new string[1]{ key }).ToArray();
    }
    public void RemoveMutekiFilter(string key)
    {
        mutekiFilters = mutekiFilters.Where(k => k != key).ToArray();
    }
    

    ///<summary>ここを直接書き換えない</summary>
    int hp = max_hp;

    public event EventHandler die;
    public event EventHandler damaged;
    public event EventHandler hpChanged;

    new CameraController camera;
    [SerializeField] Vector3 cameraShakeWidth   = new Vector3(30, 30, 0);
    [SerializeField] float cameraShakeSeconds = 0.2f;
    [SerializeField] int   cameraShakeVibrato = 10;
    [SerializeField] float houtaiRedSeconds  = 0.3f;
    [SerializeField] float houtaiBlueSeconds = 0.7f;
    [SerializeField] float houtaiAlpha1Seconds = 1f;

    [SerializeField] HeroMover hero;

    ///<summary>HPの増減はすべてここから。</summary>
    public void ChangeHP(int value)
    {
        if(value <= 0 && HP <= 0) return;
        if(value >= 3 && HP >= 3) return;

        if(HP > value)
        {
            OnDamaged();
            damaged?.Invoke(this, EventArgs.Empty);

            if(value <= 0)
            {
                die?.Invoke(this, EventArgs.Empty);
                hp=0;

                image.sprite = img1_0;
                DOVirtual.DelayedCall(houtaiRedSeconds, () => image.sprite = img0)
                    .GetPausable().AddTo(this);
                DOVirtual.DelayedCall(houtaiAlpha1Seconds, () => image.color = Color.white)
                    .GetPausable().AddTo(this);
            }
            else
            {
                hp = Math.Min(3, value);

                if(value == 1)
                {
                    image.sprite = img2_1;
                    DOVirtual.DelayedCall(houtaiRedSeconds, () => image.sprite = img1)
                        .GetPausable().AddTo(this);
                    DOVirtual.DelayedCall(houtaiAlpha1Seconds, () => image.color = Color.white)
                        .GetPausable().AddTo(this);
                }
                else if(value == 2)
                {
                    image.sprite = img3_2;
                    DOVirtual.DelayedCall(houtaiRedSeconds, () => image.sprite = img2)
                        .GetPausable().AddTo(this);
                    DOVirtual.DelayedCall(houtaiAlpha1Seconds, () => image.color = Color.white)
                        .GetPausable().AddTo(this);
                }
            }
            hpChanged?.Invoke(this, EventArgs.Empty);
        }
        else if(HP < value)
        {
            if(value >= 3)
            {
                hp = 3;
                image.sprite = img2_3;
                DOVirtual.DelayedCall(houtaiBlueSeconds, () => image.sprite = img3)
                    .GetPausable().AddTo(this);
                DOVirtual.DelayedCall(houtaiAlpha1Seconds, () => image.color = Color.white)
                    .GetPausable().AddTo(this);
                secondsAfterRecover = 0;
                image.color = new Color(1,1,1,1);
            }
            else if(value == 2)
            {
                hp = 2;
                image.sprite = img1_2;
                DOVirtual.DelayedCall(houtaiBlueSeconds, () => image.sprite = img3)
                    .GetPausable().AddTo(this);
                DOVirtual.DelayedCall(houtaiAlpha1Seconds, () => image.color = Color.white)
                    .GetPausable().AddTo(this);
                secondsAfterRecover = 0;
                image.color = new Color(1,1,1,1);
            }
            hpChanged?.Invoke(this, EventArgs.Empty);
        }

        void OnDamaged()
        {
            secondsAfterDamage = 0;
            isDamaging = true;
            image.color = new Color(1,1,1,1);
            AddMutekiFilter("Damage");

            camera.transform.DOShakePosition(cameraShakeSeconds, cameraShakeWidth, cameraShakeVibrato);
        }
    }

    public int HP{ get => hp; }

    ///<summary>全回復</summary>
    public void FullRecover() => ChangeHP(max_hp);

    void Start()
    {
        camera = CameraController.CurrentCamera;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.H)) ChangeHP(3);

        if(this.isDamaging)
        {
            // if(secondsAfterDamage < damagemove.GetLength(0))
            // {
            //     camera.transform.localPosition += new Vector3(damagemove[(int)(secondsAfterDamage * 50), 0], damagemove[(int)(secondsAfterDamage * 50), 1]);
            // }
            // else if(secondsAfterDamage == damagemove.GetLength(0))
            // {
            //     camera.transform.localPosition = HeroDefiner.CurrentHeroPastPos[0] + new Vector3(0,50,-200); 
            //     //短時間に複数回被弾したときに画面揺れの途中で揺れの状態が初めに戻って二重にずれてる、応急処置
            // }

            // if(secondsAfterDamage < secondsAfterRecover)
            // {
            //     if(secondsAfterDamage == 19 || secondsAfterDamage == 20) //これはなぜ
            //     {
            //         if(HP==0)      image.sprite = img0;
            //         else if(HP==1) image.sprite = img1;
            //         else if(HP==2) image.sprite = img2;
            //     }
            //     else if(secondsAfterDamage == 60)
            //     {
            //         image.color = new Color(1, 1, 1, 180f/255f);
            //         isDamaging = false;
            //     }
            // }
        }

        float dt = TimeManager.Current.DeltaTimeAroundHero;

        if(secondsAfterDamage >= mutekiSecondsAfterDamage) return;

        secondsAfterDamage += dt;

        if(secondsAfterDamage >= mutekiSecondsAfterDamage) RemoveMutekiFilter("Damage");

        // if(secondsAfterRecover < secondsAfterDamage)
        // {
        //     if (secondsAfterRecover == 40)
        //     {
        //         if(HP==2)      image.sprite = img2;
        //         else if(HP==3) image.sprite = img3;
        //     }
        //     else if(secondsAfterRecover == 60)
        //     {
        //         image.color = new Color(1,1,1,180f/255f);
        //     }
        // }

        secondsAfterRecover += dt;
    }
}
