using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>「ジャンプ中」などの主人公の状態はこのインターフェースを実装したクラスの形で記述します
///それはそうとメソッド作りすぎたかもな… / OnLand() みたいなのあってもいいかもな…
/// / Stateの配列をScriptableObjectにしてそこから拾ってくるみたいな実装にしたらStateにPrefabとか持てそう？</summary>
public abstract class HeroState
{
    public abstract void Start();
    public abstract void Resume();
    public abstract void Update();
    public abstract void Try2StartJet();
    ///<summary>EndJetなのにここでジェットが始まるようになっていて命名が良くなさすぎる</summary>
    public abstract void Try2EndJet();
    public abstract void Try2Jump();
    public abstract void Try2StartMove(bool toRight);
    public abstract void Try2EndMove();
    public abstract void Exit();
}
