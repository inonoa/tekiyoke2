using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using Random = UnityEngine.Random;
using UnityEngine.Rendering;

namespace Draft
{
    public class GPUInstantiater : MonoBehaviour
    {
        [SerializeField] bool updates = true;
        [SerializeField] bool renders = true;
        [SerializeField] [Range(0.5f, 10)] float timeScale = 1;

        [SerializeField] float meshSize = 1;
        [SerializeField] int   numWinds = 1024;
        [SerializeField] int   numNodesPerWind = 128;
        [SerializeField] float nodeLife = 1;
        [SerializeField] float windWidth = 10;
        [SerializeField] float firstSpeedXMin = -100;
        [SerializeField] float firstSpeedXMax = 100;
        [SerializeField] float firstSpeedYMin = -100;
        [SerializeField] float firstSpeedYMax = 100;

        [Space(10)]
        [SerializeField] Material material;
        [SerializeField] ComputeShader updateCS;
        const string UPDATE = "Update";

        ReactiveProperty<Mesh> _Mesh = new ReactiveProperty<Mesh>();
        public IObservable<Mesh> Mesh => _Mesh;

        void Start()
        {
            _Mesh.Value = MeshMaker.CreateMesh(meshSize);

            InitBuffer();
            InitCSParams();
        }

        void InitBuffer()
        {
            int updateID = updateCS.FindKernel(UPDATE);

            ComputeBuffer windsBuffer = GenerateBuffer<Wind>(numWinds, _ => Wind.Create());
            updateCS.SetBuffer(updateID, "_Winds", windsBuffer);
            material.SetBuffer("_Winds", windsBuffer);
            windsBuffer.AddTo(this);

            int numNodesTotal = numNodesPerWind * numWinds;
            ComputeBuffer nodesBuffer = GenerateBuffer<Node>(numNodesTotal, i => Node.Create(i % numNodesPerWind == 0));
            updateCS.SetBuffer(updateID, "_Nodes", nodesBuffer);
            material.SetBuffer("_Nodes", nodesBuffer);
            nodesBuffer.AddTo(this);

            ComputeBuffer inputsBuffer = GenerateBuffer<Input>(numWinds, _ => new Input());
            updateCS.SetBuffer(updateID, "_Inputs", inputsBuffer);
            inputsBuffer.AddTo(this);

            //Debug(nodesBuffer);
        }

        void Debug(ComputeBuffer buf) => StartCoroutine(DebugCor(buf));
        IEnumerator DebugCor(ComputeBuffer buf)
        {
            yield return new WaitForSeconds(2);

            var dt = new Node[numNodesPerWind * numWinds];
            buf.GetData(dt);
            string.Join("\n", dt.Select((node, i) => i%64==63 ? $"{node.pos}, {node.time}\n" : $"{node.pos}, {node.time}"))
                .p();
        }

        void InitCSParams()
        {
            updateCS.SetInt("_NumNodesPerWind", numNodesPerWind);
            material.SetInt("_NumNodesPerWind", numNodesPerWind);
            material.SetFloat("_NodeLife", nodeLife);
            material.SetFloat("_Width",    windWidth);
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
    
        void Update()
        {
            if(updates) UpdatePieces();
        }

        void OnRenderObject()
        {
            if(renders) RenderPieces();
        }

        void UpdatePieces()
        {
            updateCS.SetFloat("_DeltaTime", Time.deltaTime * timeScale);
            updateCS.SetFloat("_Time",      GetTime());

            var heroInfo = GetHeroInfo();
            Vector4 heroInfoVec = new Vector4
                (heroInfo.pos.x, heroInfo.pos.y, heroInfo.vel.x, heroInfo.vel.y);
            updateCS.SetVector("_HeroInfo", heroInfoVec);

            int updateID = updateCS.FindKernel(UPDATE);
            updateCS.Dispatch(updateID, numWinds / 64, 1, 1);
        }

        (Vector2 pos, Vector2 vel) GetHeroInfo()
        {
            return (new Vector2(0, 0), new Vector2(10, 10));
        }

        void RenderPieces()
        {
            material.SetFloat("_Time_", GetTime());
            if(material.SetPass(0))
            {
                Graphics.DrawProcedural
                (
                    MeshTopology.Points,
                    numNodesPerWind,
                    numWinds
                );
            }
        }

        float GetTime()
        {
            return Time.time; //GameTimeCounterかなんか通したい
        }
    }

}

public static class PrintExt
{
    public static T p<T>(this T obj)
    {
        Debug.Log(obj);
        return obj;
    }
}
