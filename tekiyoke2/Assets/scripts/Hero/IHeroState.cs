using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<summary>「ジャンプ中」などの主人公の状態はこのインターフェースを実装したクラスの形で記述します。それはそうとメソッド作りすぎたかもな…</summary>
public interface IHeroState
{
    void Start();
    void Update();
    void Try2StartJet();
    ///<summary>EndJetなのにここでジェットが始まるようになっていて命名が良くなさすぎる</summary>
    void Try2EndJet();
    void Try2Jump();
    void Try2StartMove(bool toRight);
    void Try2EndMove();
    void Exit();
}
