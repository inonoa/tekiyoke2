using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Number Sprites Holder")]
public class NumberSpritesHolder : ScriptableObject
{
    [SerializeField] Sprite[] _NumberSprites;

    ///<summary>[0]~[9]</summary>
    public IReadOnlyList<Sprite> NumberSprites => _NumberSprites;
}
