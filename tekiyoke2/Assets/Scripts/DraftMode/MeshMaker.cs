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

            int numSegments = 20;
            
            windMesh.vertices = GenerateUzumaki
            (
                rMax:        1,
                thetaMax:    Mathf.PI * 3,
                width:       0.2f,
                numSegments: numSegments
            )
            .Select(vert => vert * size)
            .ToArray();

            windMesh.uv = new Vector2[]{ Vector2.zero }
                .Concat
                (
                    Enumerable.Range(1, numSegments)
                    .Select(i => Mathf.Lerp(0, 1, (float)i / numSegments))
                    .SelectMany(u => new Vector2[]
                    {
                        new Vector2(u, 0),
                        new Vector2(u, 1)
                    })
                )
                .ToArray();

            windMesh.triangles = GenerateUzumakisTriangles(numSegments);

            return windMesh;
        }

        static Vector3[] GenerateUzumaki(float rMax, float thetaMax, float width, int numSegments)
        {
            Vector3[] verts = new Vector3[numSegments * 2 + 1];

            verts[0] = Vector3.zero;

            foreach(int i in Enumerable.Range(1, numSegments))
            {
                float i_norm = (float)i / numSegments;

                float rad_in  = Mathf.Lerp(0, rMax,     i_norm);
                float rad_out = rad_in + width;
                float theta   = Mathf.Lerp(0, thetaMax, i_norm);

                verts[i * 2 - 1] = RadThetaToXY0(rad_in,  theta);
                verts[i * 2    ] = RadThetaToXY0(rad_out, theta);
            }

            return verts;
        }

        static int[] GenerateUzumakisTriangles(int numSegments)
        {
            int[] tris = new int[(numSegments * 2 - 1) * 3];

            (tris[0], tris[1], tris[2]) = (0, 1, 2);

            foreach(int i in Enumerable.Range(1, numSegments - 1))
            {
                int curr_in  = i * 2 - 1;
                int curr_out = curr_in + 1;
                int next_in  = curr_in + 2;
                int next_out = curr_in + 3;

                int tris_curr = i * 6 - 3;

                tris[tris_curr    ] = curr_in;
                tris[tris_curr + 1] = next_in;
                tris[tris_curr + 2] = curr_out;

                tris[tris_curr + 3] = curr_out;
                tris[tris_curr + 4] = next_in;
                tris[tris_curr + 5] = next_out;
            }

            return tris;
        }

        static Vector3 RadThetaToXY0(float rad, float theta)
        {
            return new Vector3(Mathf.Cos(theta), Mathf.Sin(theta)) * rad;
        }
    }
}
