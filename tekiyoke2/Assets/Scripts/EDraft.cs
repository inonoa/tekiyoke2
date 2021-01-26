
using UnityEngine;

public enum EDraft
{
    Draft1,
    Draft2,
    Draft3
}

public static class EDraftExtension
{
    public static int ToInt(this EDraft eDraft) => (int) eDraft;

    public static EDraft Plus1(this EDraft eDraft)
    {
        switch (eDraft)
        {
            case EDraft.Draft1: return EDraft.Draft2;
            case EDraft.Draft2: return EDraft.Draft3;
            default:
            {
                Debug.LogError("Draft3 + 1...?");
                return EDraft.Draft3;
            }
        }
    }
    
    public static EDraft Minus1(this EDraft eDraft)
    {
        switch (eDraft)
        {
            case EDraft.Draft3: return EDraft.Draft2;
            case EDraft.Draft2: return EDraft.Draft1;
            default:
            {
                Debug.LogError("Draft1 - 1...?");
                return EDraft.Draft1;
            }
        }
    }
}

public class EDraftUtil
{
    public static EDraft ToEDraft(int index)
    {
        Debug.Assert(index == 0 || index == 1 || index == 2);
        
        if (index == 0) return EDraft.Draft1;
        if (index == 1) return EDraft.Draft2;
        return EDraft.Draft3;
    }
}
