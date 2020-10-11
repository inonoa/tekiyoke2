using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace Draft
{
    public class DebugView : MonoBehaviour
    {
        [SerializeField] GPUInstantiater instantiater;
        void Start()
        {
            MeshFilter meshFilter = GetComponent<MeshFilter>();
            instantiater.Mesh
                .Subscribe(mesh => meshFilter.mesh = mesh);
        }
    }
}
