using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMgr : MonoBehaviour {

	[SerializeField]
	NiwatoriAsset niwatori;

	// Use this for initialization
	void Start () {
		NiwatoriAsset.CreateObject (niwatori);
	}

	// Update is called once per frame
	void Update () {

	}
}
