
public enum LR
{
    L, R
}

public static class LRExtension
{
    public static LR Reverse(this LR self)
    {
        switch (self)
        {
            case LR.L: return LR.R;
            default:   return LR.L;
        }
    }
}
