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
        [SerializeField] int   numNodesPerWind = 64;
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
        ComputeBuffer argsBuffer;

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

            int numNodesTotal = numNodesPerWind * numWinds;
            ComputeBuffer nodesBuffer = GenerateBuffer<Node>(numNodesTotal, i => Node.Create(i % numNodesPerWind == 0));
            updateCS.SetBuffer(updateID, "_Nodes", nodesBuffer);
            material.SetBuffer("_Nodes", nodesBuffer);

            ComputeBuffer inputsBuffer = GenerateBuffer<Input>(numWinds, _ => new Input());
            updateCS.SetBuffer(updateID, "_Inputs", inputsBuffer);

            argsBuffer = new ComputeBuffer(5, Marshal.SizeOf<uint>(), ComputeBufferType.IndirectArguments);
            argsBuffer.SetData(new uint[]
            {
                (uint) _Mesh.Value.GetIndexCount(0),
                (uint) numWinds,
                (uint) _Mesh.Value.GetIndexStart(0),
                (uint) _Mesh.Value.GetBaseVertex(0),
                0
            });
        }

        void InitCSParams()
        {
            updateCS.SetInt("_NumNodesPerWind", numNodesPerWind);
            material.SetInt("_NumNodesPerWind", numNodesPerWind);
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
            if(renders) RenderPieces();
        }

        void UpdatePieces()
        {
            updateCS.SetFloat("_DeltaTime", Time.deltaTime * timeScale);
            updateCS.SetFloat("_Time",      Time.time); //GameTimeCounterかなんか通したい

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
            Graphics.DrawMeshInstancedIndirect
            (
                _Mesh.Value,
                0,
                material,
                new Bounds(Vector3.zero, new Vector3(1, 1, 1) * 100),
                argsBuffer
            );
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
