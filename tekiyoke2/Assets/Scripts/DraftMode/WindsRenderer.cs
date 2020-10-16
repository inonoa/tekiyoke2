using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace Draft
{
    public class WindsRenderer : MonoBehaviour
    {
        [SerializeField] float nodeLife = 1;
        [SerializeField] float windWidth = 64;
        [SerializeField] Material material;
        

        WindsMover mover;

        void Start()
        {
            mover = GetComponent<WindsMover>();

            mover.NodesBuffer.Subscribe(nodesBuffer =>
            {
                material.SetBuffer(Consts.NODES, nodesBuffer);
            });
            mover.WindsBuffer.Subscribe(windsBuffer =>
            {
                material.SetBuffer(Consts.WINDS, windsBuffer);
            });
            
            material.SetInt(  "_NumNodesPerWind", mover.NumNodesPerWind);
            material.SetFloat("_NodeLife",        nodeLife);
            material.SetFloat("_WindWidth",       windWidth);
        }

        void OnRenderObject() => RenderPieces();
        void RenderPieces()
        {
            material.SetFloat("_Time_", mover.GetTime());
            if(material.SetPass(0))
            {
                Graphics.DrawProcedural
                (
                    MeshTopology.Points,
                    mover.NumNodesPerWind,
                    mover.NumWinds
                );
            }
        }
    }

}
