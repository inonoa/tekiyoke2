using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroObjsHolder4States : MonoBehaviour
{
    [SerializeField] JetStream _JetstreamPrefab;
    public JetStream JetstreamPrefab => _JetstreamPrefab;

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
    [SerializeField] HeroAfterimage afterimagePrefab;
    public ObjectPool<HeroAfterimage> AfterimagePool{ get; private set; }

    void Start()
    {
        Transform gmTF = DraftManager.CurrentInstance.GameMasterTF;

        TsuchihokoriPool    = new ObjectPool<Tsuchihokori>(tsuchihokoriForRun,  8,  gmTF);
        JumpEffectPool      = new ObjectPool<JumpEffect>(jumpEffectPrefab,      8,  gmTF);
        JumpEffectInAirPool = new ObjectPool<JumpEffect>(jumpEffectInAirPrefab, 8,  gmTF);
        KabezuriPool        = new ObjectPool<Kabezuri>(kabezuriPrefab,          8,  gmTF);
        AfterimagePool      = new ObjectPool<HeroAfterimage>(afterimagePrefab,  16, gmTF);

        PhantomRenderer = Instantiate(phantomRendererPrefab, DraftManager.CurrentInstance.GameMasterTF);
    }
}
