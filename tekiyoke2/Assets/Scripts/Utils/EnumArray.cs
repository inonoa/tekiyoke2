
using System;
using UnityEngine;

public class EnumArray<TElem, TEnum> where TEnum : Enum
{
    TElem[] array;
    
    public EnumArray(TElem[] array)
    {
        Debug.Assert(array.Length == Enum.GetValues(typeof(TEnum)).Length);
        array = this.array;
    }

    public EnumArray()
    {
        array = new TElem[Enum.GetValues(typeof(TEnum)).Length];
    }

    public TElem this[TEnum e]
    {
        get
        {
            int i = Convert.ToInt32(e);
            return array[i];
        }
        set
        {
            int i = Convert.ToInt32(e);
            array[i] = value;
        }
    }
}
