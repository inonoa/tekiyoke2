using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Draft
{
    public class MeshMaker
    {
        public static Mesh CreateMesh(float size)
        {
            Mesh windMesh = new Mesh();
            windMesh.name = "Wind";
            windMesh.vertices = new Vector3[]
            {
                new Vector3( 1,  1,  1),
                new Vector3( 1, -1, -1),
                new Vector3(-1,  1, -1),
                new Vector3(-1, -1,  1)
            }
            .Select(vert => vert * size)
            .ToArray();

            windMesh.normals = new Vector3[]
            {
                new Vector3( 1,  1,  1),
                new Vector3( 1, -1, -1),
                new Vector3(-1,  1, -1),
                new Vector3(-1, -1,  1)
            }
            .Select(vert => vert.normalized)
            .ToArray();

            windMesh.uv = new Vector2[]
            {
                new Vector2(0.5f, 0),
                new Vector2(0, 1),
                new Vector2(1, 1),
                new Vector2(0.5f, 1)
            };

            windMesh.triangles = new int[]
            {
                0, 1, 2,
                0, 2, 3,
                0, 3, 1,
                1, 3, 2
            };

            return windMesh;
        }
    }
}
