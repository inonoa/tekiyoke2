using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableObjectsHolder : MonoBehaviour
{
    //参照があるだけでInstantiateされるらしいよ
    [SerializeField] ScriptableObject[] scriptableObjects;
}
