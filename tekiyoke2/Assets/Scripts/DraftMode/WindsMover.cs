using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UniRx;
using System;
using System.Runtime.InteropServices;

namespace Draft
{
    class WindsMover : MonoBehaviour
    {
        public bool updates = false;
        [SerializeField][Range(1, 5)] int updatePerFrame = 1;

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

        Func<HeroInfo> heroInfoGetter = () => new HeroInfo
        {
            pos      = Vector2.zero,
            velocity = new Vector2(10, 10)
        };

        Func<float> deltaTimeGetter = () => 0.02f;
        Func<float> timeGetter = () => Time.time;

        public void Init(Func<HeroInfo> heroInfoGetter, Func<float> deltaTimeGetter, Func<float> timeGetter) 
        {
            this.heroInfoGetter  = heroInfoGetter;
            this.deltaTimeGetter = deltaTimeGetter;
            this.timeGetter      = timeGetter;
        }

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
            if(!updates) return;

            foreach(int _ in Enumerable.Range(0, updatePerFrame))
            {
                UpdatePieces();
            }
        }
        void UpdatePieces()
        {
            updateCS.SetFloat("_DeltaTime", deltaTimeGetter.Invoke() / updatePerFrame);
            updateCS.SetFloat("_Time",      GetTime());
            updateCS.SetVector("_CameraPos", Camera.main.transform.position);

            var heroInfo = heroInfoGetter.Invoke();
            Vector4 heroInfoVec = new Vector4
                (heroInfo.pos.x, heroInfo.pos.y, heroInfo.velocity.x, heroInfo.velocity.y);
            updateCS.SetVector("_HeroInfo", heroInfoVec);

            int updateID = updateCS.FindKernel(Consts.UPDATE);
            updateCS.Dispatch(updateID, _NumWinds / 64, 1, 1);
        }

        public float GetTime()
        {
            return timeGetter.Invoke();
        }
    }

    public class HeroInfo
    {
        public Vector2 pos;
        public Vector2 velocity;
    }

    class Consts
    {
        public const string UPDATE = "Update";
        public const string NODES = "_Nodes";
        public const string WINDS = "_Winds";
    }
}
