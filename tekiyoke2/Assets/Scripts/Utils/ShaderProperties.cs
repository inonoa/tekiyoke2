using UnityEngine;

public abstract class ShaderProperty{
    protected ShaderProperty(Material mat, string name){
        Debug.Assert(mat.HasProperty(name), "そんな名前のプロパティはない");
        (this.mat, this.name) = (mat, name);
    }

    protected readonly Material mat;
    protected readonly string name;
}

///<summary>読み書きを何回もする場合は良いけど現状プロパティ名の文字列打つ回数ぐらいしかうまみの無いラッパ</summary>
public class ShaderPropertyFloat : ShaderProperty{
    
    public ShaderPropertyFloat(Material mat, string name) : base(mat, name){ }

    public float GetVal() => mat.GetFloat(name);
    public void SetVal(float val) => mat.SetFloat(name, val);
}
