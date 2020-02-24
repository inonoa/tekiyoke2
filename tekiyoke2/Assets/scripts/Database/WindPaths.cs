using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(menuName = "ScriptableObject/WindPaths")]
public class WindPaths : ScriptableObject
{
    [SerializeField] [TextArea(5,20)] string[] pathsStr;
    public Vector3[][] paths;
}
