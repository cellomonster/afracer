#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Splines;
#endif

using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;
using Interpolators = UnityEngine.Splines.Interpolators;

namespace Track
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(SplineContainer), typeof(MeshRenderer), typeof(MeshFilter))]
    [RequireComponent(typeof(MeshCollider))]
    public class Mesher : MonoBehaviour
    {
        [Range(0.01f, 1)]
        [SerializeField]
        private float stripsPerMeter = 0.1f;

        [SerializeField] private float2[] stripShape;

        [SerializeField] private Mesh mesh;

        [SerializeField] private float textureScale = 1f;
        [SerializeField] private float uvWidth = 5f;

        [SerializeField] private MeshFilter meshFilter;
        [SerializeField] private MeshCollider meshCollider;
        [SerializeField] private SplineContainer splineContainer;

        private Spline Spline => splineContainer.Spline;

        private Mesh Mesh
        {
            get
            {
                if (mesh != null)
                    return mesh;

                mesh = new Mesh();
                mesh.name = "track mesh";
                return mesh;
            }
        }

        public void OnEnable()
        {
            //Avoid to point to an existing instance when duplicating the GameObject
            if(mesh != null)
                mesh = null;

            stripsPerMeter = Mathf.Min(10, stripsPerMeter);

            splineContainer = GetComponent<SplineContainer>();
            meshFilter = GetComponent<MeshFilter>();
            meshCollider = GetComponent<MeshCollider>();
            //Loft();
#if UNITY_EDITOR
            //EditorSplineUtility.afterSplineWasModified += OnAfterSplineWasModified;
            //EditorSplineUtility.RegisterSplineDataChanged<float>(OnAfterSplineDataWasModified);
            Undo.undoRedoPerformed += Loft;
#endif
        }

        public void OnDisable()
        {
#if UNITY_EDITOR
            //EditorSplineUtility.afterSplineWasModified -= OnAfterSplineWasModified;
            //EditorSplineUtility.UnregisterSplineDataChanged<float>(OnAfterSplineDataWasModified);
            Undo.undoRedoPerformed -= Loft;
#endif

            if (mesh != null)
#if UNITY_EDITOR
                DestroyImmediate(mesh);
#else
                Destroy(mesh);
#endif
        }

        //void OnAfterSplineWasModified(Spline s)
        //{
        //    if (s == Spline)
        //        Loft();
        //}

        //void OnAfterSplineDataWasModified(SplineData<float> splineData)
        //{
        //    if (splineData == widthData.width)
        //        Loft();
        //}

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (Application.isPlaying)
                return;

            Loft();
        }
#endif

        public void Loft()
        {
            Debug.Log("Lofting track!");

            if (Spline == null || Spline.Count < 2)
                return;
            
            Mesh.Clear();

            float length = Spline.GetLength();

            if (length < 1)
                return;

            int segments = (int)(stripsPerMeter * length);
            int vertexCount = segments * stripShape.Length;
            int triCount = segments * stripShape.Length * 6;

            List<Vector3> verts = new(vertexCount);
            List<Vector3> norms = new(vertexCount);
            List<Vector2> uvs = new(vertexCount);
            List<int> tris = new(triCount);

            // verts and normals
            for (int i = 0; i < segments; i++)
            {
                float t = i / (segments - 1f);
                float3 control = SplineUtility.EvaluatePosition(Spline, t);
                float3 dir = SplineUtility.EvaluateTangent(Spline, t);
                float3 norm = SplineUtility.EvaluateUpVector(Spline, t);

                Vector3 scale = transform.lossyScale;
                float3 perpendicular = math.normalize(math.cross(norm, dir)) * new float3(1f / scale.x, 1f / scale.y, 1f / scale.z);

                //float width = widthData.defaultWidth;
                //if(widthData.width != null && widthData.Count > 0)
                //{
                //    width = widthData.width.Evaluate(Spline, t, PathIndexUnit.Normalized, new Interpolators.LerpFloat());
                //    width = math.clamp(width, .001f, 10000f);
                //}

                // vertices
                float uvY = t * length * textureScale;

                for (int j = 0; j < stripShape.Length; j++)
				{
                    float3 vert = stripShape[j].x * perpendicular + stripShape[j].y * norm;

                    verts.Add(control + vert);
                    norms.Add(norm);
                    uvs.Add(new Vector2(stripShape[j].x / uvWidth, uvY));
                }
            }

            //tris
            for (int y = 0; y < segments - 1; y+= 1)
			{
                // 
                int yPlus = y * stripShape.Length;

                for(int x = 0; x < stripShape.Length - 1; x++)
				{
                    int tl = (yPlus + x) % triCount;
                    int tr = (yPlus + x + 1) % triCount;
                    int bl = (yPlus + stripShape.Length + x) % triCount;
                    int br = (yPlus + stripShape.Length + x + 1) % triCount;

                    tris.Add(br);
                    tris.Add(tr);
                    tris.Add(tl);

                    tris.Add(tl);
                    tris.Add(bl);
                    tris.Add(br);
                }
			}

            Mesh.SetVertices(verts);
            Mesh.SetNormals(norms);
            Mesh.SetUVs(0, uvs);
            Mesh.subMeshCount = 1;
            Mesh.SetIndices(tris, MeshTopology.Triangles, 0);
            Mesh.UploadMeshData(false);

            meshFilter.sharedMesh = Mesh;
            meshCollider.sharedMesh = Mesh;
        }
    }
}
