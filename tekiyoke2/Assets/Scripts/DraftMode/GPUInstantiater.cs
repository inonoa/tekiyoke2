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

        [SerializeField] float meshSize = 1;
        [SerializeField] int numPieces = 1024;

        [Space(10)]
        [SerializeField] Material material;
        [SerializeField] ComputeShader updateCS;

        ReactiveProperty<Mesh> _Mesh = new ReactiveProperty<Mesh>();
        public IObservable<Mesh> Mesh => _Mesh;
        ComputeBuffer piecesBuffer;
        ComputeBuffer argsBuffer;

        void Start()
        {
            _Mesh.Value = MeshMaker.CreateMesh(meshSize);

            piecesBuffer = new ComputeBuffer(numPieces, Marshal.SizeOf<Piece>());
            piecesBuffer.SetData
            (
                Enumerable.Range(0, numPieces)
                    .Select(_ => GenerateRandomPiece())
                    .ToArray()
            );

            argsBuffer = new ComputeBuffer(5, Marshal.SizeOf<uint>(), ComputeBufferType.IndirectArguments);
            argsBuffer.SetData(new uint[]
            {
                (uint) _Mesh.Value.GetIndexCount(0),
                (uint) numPieces,
                (uint) _Mesh.Value.GetIndexStart(0),
                (uint) _Mesh.Value.GetBaseVertex(0),
                0
            });
        }

        Piece GenerateRandomPiece()
        {
            Vector3 pos = new Vector3
            (
                Random.Range(-5f, 5f),
                Random.Range(-5f, 5f),
                Random.Range(-5f, 5f)
            );
            Vector3 vel = new Vector3
            (
                Random.Range(-1f, 1f),
                Random.Range(-1f, 1f),
                Random.Range(-1f, 1f)
            );

            return new Piece{ pos = pos, vel = vel };
        }
    
        void Update()
        {
            if(updates) UpdatePieces();
            if(renders) RenderPieces();
        }

        void UpdatePieces()
        {
            int updateID = updateCS.FindKernel("Update");

            updateCS.SetBuffer(updateID, "_Pieces", piecesBuffer);

            updateCS.Dispatch(updateID, numPieces / 64, 1, 1);
        }

        void RenderPieces()
        {
            material.SetBuffer("_Pieces", piecesBuffer);

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
