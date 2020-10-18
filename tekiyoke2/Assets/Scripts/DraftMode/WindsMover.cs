using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UniRx;
using System;
using System.Runtime.InteropServices;

namespace Draft
{
    public class WindsMover : MonoBehaviour
    {
        [SerializeField] bool updates = true;

        [SerializeField] int   _NumWinds = 1024;
        public int NumWinds => _NumWinds;
        [SerializeField] int   _NumNodesPerWind = 64;
        public int NumNodesPerWind => _NumNodesPerWind;
        [SerializeField] Vector2 area = new Vector2(1100, 825);
        [SerializeField] Wind.Params windParams = new Wind.Params();

        [Space(10)]
        [SerializeField] ComputeShader updateCS;

        ReactiveProperty<ComputeBuffer> _WindsBuffer = new ReactiveProperty<ComputeBuffer>();
        ReactiveProperty<ComputeBuffer> _NodesBuffer = new ReactiveProperty<ComputeBuffer>();

        public IObservable<ComputeBuffer> WindsBuffer => _WindsBuffer;
        public IObservable<ComputeBuffer> NodesBuffer => _NodesBuffer;

        void Start()
        {
            InitBuffer();
            InitCSParams();
        }

        void InitBuffer()
        {
            int updateID = updateCS.FindKernel(Consts.UPDATE);

            ComputeBuffer windsBuffer = GenerateBuffer<Wind>(_NumWinds, _ => Wind.Create(windParams));
            updateCS.SetBuffer(updateID, Consts.WINDS, windsBuffer);
            windsBuffer.AddTo(this);
            _WindsBuffer.Value = windsBuffer;

            int numNodesTotal = _NumNodesPerWind * _NumWinds;
            ComputeBuffer nodesBuffer = GenerateBuffer<Node>(numNodesTotal, i => Node.Create(i % _NumNodesPerWind == 0, area));
            updateCS.SetBuffer(updateID, Consts.NODES, nodesBuffer);
            nodesBuffer.AddTo(this);
            _NodesBuffer.Value = nodesBuffer;
        }

        ComputeBuffer GenerateBuffer<T>(int length, Func<int, T> generator)
        {
            ComputeBuffer buffer = new ComputeBuffer(length, Marshal.SizeOf<T>());
            buffer.SetData
            (
                Enumerable.Range(0, length)
                    .Select(i => generator.Invoke(i))
                    .ToArray()
            );
            return buffer;
        }

        void InitCSParams()
        {
            updateCS.SetInt("_NumNodesPerWind", _NumNodesPerWind);
            updateCS.SetVector("_Area", area);
        }
    
        void Update()
        {
            if(updates) UpdatePieces();
        }
        void UpdatePieces()
        {
            updateCS.SetFloat("_DeltaTime", Time.deltaTime);
            updateCS.SetFloat("_Time",      GetTime());
            updateCS.SetVector("_CameraPos", Camera.main.transform.position);

            var heroInfo = GetHeroInfo();
            Vector4 heroInfoVec = new Vector4
                (heroInfo.pos.x, heroInfo.pos.y, heroInfo.vel.x, heroInfo.vel.y);
            updateCS.SetVector("_HeroInfo", heroInfoVec);

            int updateID = updateCS.FindKernel(Consts.UPDATE);
            updateCS.Dispatch(updateID, _NumWinds / 64, 1, 1);
        }

        (Vector2 pos, Vector2 vel) GetHeroInfo()
        {
            return (new Vector2(0, 0), new Vector2(10, 10));
        }

        public float GetTime()
        {
            return Time.time; //GameTimeCounterかなんか通したい
        }
    }

    public class Consts
    {
        public const string UPDATE = "Update";
        public const string NODES = "_Nodes";
        public const string WINDS = "_Winds";
    }
}
