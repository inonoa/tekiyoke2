using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>暫定的に、State内でプレハブをインスタンス化したいときにここを参照する？/多分、Stateを1個かn個しか作らないようにしてScriptableObjectにしてしまうのがいい</summary>
public class HeroObjsHolder4States : MonoBehaviour
{
    public GameObject jetstreamPrefab;
    public TrailRenderer jetTrail;

    //このへんなんとかならんの……
    public ObjectPool<Tsuchihokori> TsuchihokoriPool{ get; private set; }
    [SerializeField] Tsuchihokori tsuchihokoriForRun;
    public ObjectPool<JumpEffect> JumpEffectPool{ get; private set; }
    [SerializeField] JumpEffect jumpEffectPrefab;
    public ObjectPool<JumpEffect> JumpEffectInAirPool{ get; private set; }
    [SerializeField] JumpEffect jumpEffectInAirPrefab;
    public ObjectPool<Kabezuri> KabezuriPool{ get; private set; }
    [SerializeField] Kabezuri kabezuriPrefab;
    [SerializeField] SpriteRenderer phantomRendererPrefab;
    public SpriteRenderer PhantomRenderer{ get; private set; }

    void Start(){
        Transform gmTF = DraftManager.CurrentInstance.GameMasterTF;
        TsuchihokoriPool = new ObjectPool<Tsuchihokori>(tsuchihokoriForRun, 8, gmTF);
        JumpEffectPool = new ObjectPool<JumpEffect>(jumpEffectPrefab, 8, gmTF);
        JumpEffectInAirPool = new ObjectPool<JumpEffect>(jumpEffectInAirPrefab, 8, gmTF);
        KabezuriPool = new ObjectPool<Kabezuri>(kabezuriPrefab, 8, gmTF);
        PhantomRenderer = Instantiate(phantomRendererPrefab, DraftManager.CurrentInstance.GameMasterTF);
    }
}
