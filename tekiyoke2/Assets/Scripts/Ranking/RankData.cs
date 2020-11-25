using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Ranking
{
    [Serializable]
    public class RankData
    {
        [field: SerializeField, LabelText(nameof(Kind))]
        public RankKind Kind { get; private set; }
        
        [SerializeField] RankDatum[] _Top100;
        [SerializeField] RankDatum[] _AroundPlayer100;
        
        public IReadOnlyList<RankDatum> Top100 => _Top100;
        public IReadOnlyList<RankDatum> AroundPlayer100 => _AroundPlayer100;

        public RankData(RankKind kind, RankDatum[] top100, RankDatum[] aroundPlayer100)
        {
            this.Kind = kind;
            this._Top100 = top100;
            this._AroundPlayer100 = aroundPlayer100;
        }
    }
    
    [Serializable]
    public class RankDatum
    {
        [field: SerializeField, LabelText(nameof(Name))]
        public string Name { get; private set; }
        
        [field: SerializeField, LabelText(nameof(Rank))]
        public int    Rank { get; private set; }
        
        [field: SerializeField, LabelText(nameof(Time))]
        public float  Time { get; private set; }

        public RankDatum(string name, int rank, float time)
        {
            this.Name = name;
            this.Rank = rank;
            this.Time = time;
        }
    }
    
    public enum RankKind
    {
        Draft1,
        Draft2,
        Draft3,
        AllDrafts
    }

    public static class RankKindUtil
    {
        static readonly RankKind[] kinds = {RankKind.Draft1, RankKind.Draft2, RankKind.Draft3};
        public static RankKind ToKind(int stageIndex) => kinds[stageIndex];
    }
}