using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GPUParticleSystemMeshUtility
{
	#region LocalBuffer
#pragma warning disable 414
	private static Vector3[] Vertices = new Vector3[0];
    private static Vector3[] Normals = new Vector3[0];
	private static Vector4[] Tangents = new Vector4[0];
	private static int[] Triangles = new int[0];
	private static Vector2[] UVs = new Vector2[0];
	private static Vector2[] PosUV = new Vector2[0];
	private static Vector2[] AnimUV = new Vector2[0];

	public static float ignoreSize = 0f;//0.0002f;
    private static int numTriangles;
    private static Vector3[] vertexBuffer = new Vector3[0];
	private static int[] triangleBuffer = new int[0];
	private static Vector2[] uV1Buffer = new Vector2[0];
	private static Vector2[] uV2Buffer = new Vector2[0];
	private static float[] weightsBuffer = new float[0];
	private static float Buffer = 0;
    private static Vector3 vectorBuffer;
    private static float sizeBuffer;
	private static Color[] ColorArray = new Color[0];
	private static Texture2D MeshEmitterPositions;
#pragma warning restore 414
	#endregion

	public static GameObject[] CreateParticlesPoint(int _Width, int _Height, Material _Mat)
    {
        int NumParticles = _Width * _Height;
        int NumMeshes = Mathf.CeilToInt(((float)NumParticles) / 65535);

        GameObject[] MeshHolders = new GameObject[NumMeshes];

        int CurrentVertNum = 65535;
        Vector2 HalfTexelOffset = new Vector2((1f / (float)_Width) / 2f, (1f / (float)_Height) / 2f);
        int index = 0;

        for (int n = 0; n < NumMeshes; n++)
        {
            if (n == NumMeshes - 1)
                CurrentVertNum = NumParticles % 65535;

            Vertices = new Vector3[CurrentVertNum];
            Triangles = new int[CurrentVertNum];
            PosUV = new Vector2[CurrentVertNum];

            //Create Vertices
            for (int i = 0; i < Vertices.Length; i++)
            {
                Vertices[i] = new Vector3(0f, 0f, 0f);
                Triangles[i] = i;
            }

            //Create UVs
            for (int i = 0; i < PosUV.Length; i++, index++)
            {
				float uCoord = ((float)index % (float)_Width) / _Width;
				float vCoord = ((Mathf.Floor((float)index / (float)_Width)) % _Height) / _Height;

				PosUV[i] = new Vector2(uCoord, vCoord) + HalfTexelOffset;
            }

			//Debug.Log("Index "+index);

            GameObject g = new GameObject("ParticleHolder_" + n.ToString());
            Mesh mesh = new Mesh();
            mesh.vertices = Vertices;
            mesh.SetIndices(Triangles, MeshTopology.Points, 0);
            mesh.uv = PosUV;
            g.AddComponent<MeshFilter>().mesh = mesh;
            g.AddComponent<MeshRenderer>().sharedMaterial = _Mat;
            Bounds b = new Bounds();
            b.center = g.transform.position;
            b.extents = new Vector3(15f, 15f, 15f);
            mesh.bounds = b;
			g.hideFlags = HideFlags.HideAndDontSave;
			MeshHolders[n] = g;
        }

		//Debug.Log(index);

        return MeshHolders;
    }

	public static GameObject[] CreateParticlesTriangle(int _Width, int _Height, Material _Mat)
	{
		int NumParticles = _Width * _Height;
		int NumMeshes = Mathf.CeilToInt((NumParticles * 3f) / 65535);	//3verts + 1 tri per particle devided by max vert count

		//Debug.Log(NumParticles + " " + _Width + " " + _Height + " " + NumMeshes);

		GameObject[] MeshHolders = new GameObject[NumMeshes];

		int CurrentVertCount = 21845;									//max vert count devided by 3
		Vector2 HalfTexelOffset = new Vector2((1f / (float)_Width) / 2f, (1f / (float)_Height) / 2f);
		int index = 0;

		for (int n = 0; n < NumMeshes; n++)
		{
			if (n == NumMeshes - 1)
				CurrentVertCount = NumParticles % 21845;

			Vertices = new Vector3[CurrentVertCount * 3];
			Normals = new Vector3[CurrentVertCount * 3];
			Triangles = new int[CurrentVertCount * 3];
			PosUV = new Vector2[CurrentVertCount * 3];
			UVs = new Vector2[CurrentVertCount * 3];

			//Create Vertices
			for (int i = 0; i < Vertices.Length; i += 3)
			{
				Vertices[i] = new Vector3(0f, 0f, 0f);
				Vertices[i + 1] = new Vector3(0f, 0f, 0f);
				Vertices[i + 2] = new Vector3(0f, 0f, 0f);

				Normals[i] = new Vector3(0f, 0f, -1f);
				Normals[i + 1] = new Vector3(0f, 0f, -1f);
				Normals[i + 2] = new Vector3(0f, 0f, -1f);
			}

			int triIndex = 0;

			for (int i = 0; i < Vertices.Length; i += 3)
			{
				Triangles[triIndex] = i;
				triIndex++;
				Triangles[triIndex] = i + 2;
				triIndex++;
				Triangles[triIndex] = i + 1;
				triIndex++;
			}

			for (int i = 0; i < CurrentVertCount; i++, index++)
			{
				UVs[i * 3] = new Vector2(0f, 0f);
				UVs[i * 3 + 1] = new Vector2(1f, 0f);
				UVs[i * 3 + 2] = new Vector2(.5f, 1f);

				float uCoord = ((float)index%(float)_Width) / _Width;
				float vCoord = ((Mathf.Floor((float)index / (float)_Width)) % _Height ) / _Height;

				PosUV[i * 3] = new Vector2(uCoord, vCoord) + HalfTexelOffset;
				PosUV[i * 3 + 1] = new Vector2(uCoord, vCoord) + HalfTexelOffset;
				PosUV[i * 3 + 2] = new Vector2(uCoord, vCoord) + HalfTexelOffset;
			}

			//Debug.Log("Index: "+index);

			GameObject g = new GameObject("ParticleHolder_" + n.ToString());
			Mesh mesh = new Mesh();
			mesh.name = "Particle Mesh";
			mesh.vertices = Vertices;
			mesh.triangles = Triangles;
			mesh.uv = PosUV;
			mesh.uv2 = UVs;
			mesh.normals = Normals;
			g.AddComponent<MeshFilter>().mesh = mesh;
			g.AddComponent<MeshRenderer>().sharedMaterial = _Mat;
			Bounds b = new Bounds();
			b.center = g.transform.position;
			b.extents = new Vector3(15f, 15f, 15f);
			mesh.bounds = b;
			g.hideFlags = HideFlags.HideAndDontSave;
			MeshHolders[n] = g;
		}

		return MeshHolders;
	}

	public static GameObject[] CreateParticlesQuad(int _Width, int _Height, Material _Mat)
    {
        int NumParticles = _Width * _Height;
        int NumMeshes = Mathf.CeilToInt(((float)NumParticles) / 16250);

        GameObject[] MeshHolders = new GameObject[NumMeshes];

        int CurrentVertNum = 16250;
        Vector2 HalfTexelOffset = new Vector2((1f / (float)_Width) / 2f, (1f / (float)_Height) / 2f);
        int index = 0;

        for (int n = 0; n < NumMeshes; n++)
        {
            if (n == NumMeshes - 1)
                CurrentVertNum = NumParticles % 16250;

            Vertices = new Vector3[CurrentVertNum * 4];
            Normals = new Vector3[CurrentVertNum * 4];
            Tangents = new Vector4[CurrentVertNum * 4];
            Triangles = new int[CurrentVertNum * 6];
            PosUV = new Vector2[CurrentVertNum * 4];
            UVs = new Vector2[CurrentVertNum * 4];

            //Create Vertices
            for (int i = 0; i < Vertices.Length; i += 4)
            {
                Vertices[i] = new Vector3(0f, 0f, 0f);
                Vertices[i + 1] = new Vector3(0f, 0f, 0f);
                Vertices[i + 2] = new Vector3(0f, 0f, 0f);
                Vertices[i + 3] = new Vector3(0f, 0f, 0f);

                Normals[i] = new Vector3(0f, 0f, -1f);
                Normals[i + 1] = new Vector3(0f, 0f, -1f);
                Normals[i + 2] = new Vector3(0f, 0f, -1f);
                Normals[i + 3] = new Vector3(0f, 0f, -1f);

                Tangents[i] = new Vector3(1f, 0f, 0f);
                Tangents[i + 1] = new Vector3(1f, 0f, 0f);
                Tangents[i + 2] = new Vector3(1f, 0f, 0f);
                Tangents[i + 3] = new Vector3(1f, 0f, 0f);
            }

            int triIndex = 0;

            for (int i = 0; i < Vertices.Length; i += 4)
            {
                Triangles[triIndex] = i;
                triIndex++;
                Triangles[triIndex] = i + 3;
                triIndex++;
                Triangles[triIndex] = i + 1;
                triIndex++;

                Triangles[triIndex] = i + 1;
                triIndex++;
                Triangles[triIndex] = i + 3;
                triIndex++;
                Triangles[triIndex] = i + 2;
                triIndex++;
            }

            //Create UVs
            for (int i = 0; i < CurrentVertNum; i++, index++)
            {
                UVs[i * 4] = new Vector2(0f, 0f);
                UVs[i * 4 + 1] = new Vector2(1f, 0f);
                UVs[i * 4 + 2] = new Vector2(1f, 1f);
                UVs[i * 4 + 3] = new Vector2(0f, 1f);

				float uCoord = ((float)index % (float)_Width) / _Width;
				float vCoord = ((Mathf.Floor((float)index / (float)_Width)) % _Height) / _Height;

				PosUV[i * 4] = new Vector2(uCoord, vCoord) + HalfTexelOffset;
                PosUV[i * 4 + 1] = new Vector2(uCoord, vCoord) + HalfTexelOffset;
                PosUV[i * 4 + 2] = new Vector2(uCoord, vCoord) + HalfTexelOffset;
                PosUV[i * 4 + 3] = new Vector2(uCoord, vCoord) + HalfTexelOffset;
            }

            GameObject g = new GameObject("ParticleHolder_" + n.ToString());
            Mesh mesh = new Mesh();
            mesh.vertices = Vertices;
            mesh.triangles = Triangles;
            mesh.uv = PosUV;
            mesh.uv2 = UVs;
            mesh.normals = Normals;
            mesh.tangents = Tangents;
            g.AddComponent<MeshFilter>().mesh = mesh;
            g.AddComponent<MeshRenderer>().sharedMaterial = _Mat;
            Bounds b = new Bounds();
            b.center = g.transform.position;
            b.extents = new Vector3(15f, 15f, 15f);
            mesh.bounds = b;
			g.hideFlags = HideFlags.HideAndDontSave;
			MeshHolders[n] = g;
        }
		
        return MeshHolders;
    }

    public static GameObject[] CreateParticlesDoubleQuad(int _Width, int _Height, Material _Mat)
    {
        int NumParticles = _Width * _Height;
        int NumMeshes = Mathf.CeilToInt(((float)NumParticles) / 13100);

        GameObject[] MeshHolders = new GameObject[NumMeshes];

        int CurrentVertNum = 13100;
        Vector2 HalfTexelOffset = new Vector2((1f / (float)_Width) / 2f, (1f / (float)_Height) / 2f);
        int index = 0;

        for (int n = 0; n < NumMeshes; n++)
        {
            if (n == NumMeshes - 1)
                CurrentVertNum = NumParticles % 13100;

            Vertices = new Vector3[CurrentVertNum * 5];
            Normals = new Vector3[CurrentVertNum * 5];
            Triangles = new int[CurrentVertNum * 9];
            PosUV = new Vector2[CurrentVertNum * 5];
            UVs = new Vector2[CurrentVertNum * 5];

            //Create Vertices
            for (int i = 0; i < Vertices.Length; i += 5)
            {
                Vertices[i] = new Vector3(1f, 0f, 0f);
                Vertices[i + 1] = new Vector3(0f, 0f, 0f);
                Vertices[i + 2] = new Vector3(0f, 0f, 0f);
                Vertices[i + 3] = new Vector3(0f, 0f, 0f);
                Vertices[i + 4] = new Vector3(0f, 0f, 0f);

                Normals[i] = new Vector3(0f, 0f, -1f);
                Normals[i + 1] = new Vector3(0f, 0f, -1f);
                Normals[i + 2] = new Vector3(0f, 0f, -1f);
                Normals[i + 3] = new Vector3(0f, 0f, -1f);
                Normals[i + 4] = new Vector3(0f, 0f, -1f);
            }

            int triIndex = 0;

            for (int i = 0; i < Vertices.Length; i += 5)
            {
                Triangles[triIndex] = i;
                triIndex++;
                Triangles[triIndex] = i + 1;
                triIndex++;
                Triangles[triIndex] = i + 2;
                triIndex++;

                Triangles[triIndex] = i + 1;
                triIndex++;
                Triangles[triIndex] = i + 3;
                triIndex++;
                Triangles[triIndex] = i + 2;
                triIndex++;

                Triangles[triIndex] = i + 2;
                triIndex++;
                Triangles[triIndex] = i + 3;
                triIndex++;
                Triangles[triIndex] = i + 4;
                triIndex++;
            }

            //Create UVs
            for (int i = 0; i < CurrentVertNum; i++, index++)
            {
                UVs[i * 5] = new Vector2(0f, 0.5f);
                UVs[i * 5 + 1] = new Vector2(0.5f, 1f);
                UVs[i * 5 + 2] = new Vector2(.5f, 0f);
                UVs[i * 5 + 3] = new Vector2(1f, 1f);
                UVs[i * 5 + 4] = new Vector2(1f, 0f);

				float uCoord = ((float)index % (float)_Width) / _Width;
				float vCoord = ((Mathf.Floor((float)index / (float)_Width)) % _Height) / _Height;

				PosUV[i * 5] = new Vector2(uCoord, vCoord) + HalfTexelOffset;
                PosUV[i * 5 + 1] = new Vector2(uCoord, vCoord) + HalfTexelOffset;
                PosUV[i * 5 + 2] = new Vector2(uCoord, vCoord) + HalfTexelOffset;
                PosUV[i * 5 + 3] = new Vector2(uCoord, vCoord) + HalfTexelOffset;
                PosUV[i * 5 + 4] = new Vector2(uCoord, vCoord) + HalfTexelOffset;
            }

            GameObject g = new GameObject("ParticleHolder_" + n.ToString());
            Mesh mesh = new Mesh();
            mesh.vertices = Vertices;
            mesh.triangles = Triangles;
            mesh.uv = PosUV;
            mesh.uv2 = UVs;
            mesh.normals = Normals;
            g.AddComponent<MeshFilter>().mesh = mesh;
            g.AddComponent<MeshRenderer>().sharedMaterial = _Mat;
            Bounds b = new Bounds();
            b.center = g.transform.position;
            b.extents = new Vector3(15f, 15f, 15f);
            mesh.bounds = b;
            MeshHolders[n] = g;
        }
		
        foreach (GameObject g in MeshHolders)
        {
            g.hideFlags = HideFlags.HideAndDontSave;
        }
		
        return MeshHolders;
    }

    public static GameObject[] CreateMeshParticles(Mesh _Mesh, int _Width, int _Height, Material _Mat, bool _2ndUV)
    {
        int oMeshVertNum = _Mesh.vertexCount;
        int oMeshTriNum = _Mesh.triangles.Length;
        int oMeshNormNum = _Mesh.normals.Length;
        int oMeshTanNum = _Mesh.tangents.Length;
        int ParticlesPerMesh = Mathf.FloorToInt(65000f / (float)oMeshVertNum);
        int NumParticles = _Width * _Height;
        int NumMeshes = Mathf.CeilToInt((float)NumParticles / (float)ParticlesPerMesh);

        GameObject[] MeshHolders = new GameObject[NumMeshes];
        Vector2 HalfTexelOffset = new Vector2((1f / (float)_Width) / 2f, (1f / (float)_Height) / 2f);
        float AnimHalfTexelOffset = (1f / (float)oMeshVertNum) / 2f;

        //Original Mesh
        Vector3[] oVertices = _Mesh.vertices;
        Vector4[] oTangents = _Mesh.tangents;
        Vector3[] oNormals = _Mesh.normals;
        int[] oTriangles = _Mesh.triangles;
        Vector2[] oUV1 = _Mesh.uv;

        int index = 0;

        for (int n = 0; n < NumMeshes; n++)
        {
            GameObject g = new GameObject("ParticleHolder_" + n.ToString());
            Mesh m = new Mesh();

            int nVertNum = oMeshVertNum * ParticlesPerMesh;
            int nTriNum = oMeshTriNum * ParticlesPerMesh;
            int nNormNum = oMeshNormNum * ParticlesPerMesh;
            int nTangentsNum = oMeshTanNum * ParticlesPerMesh;
            Vertices = new Vector3[nVertNum];
            Normals = new Vector3[nNormNum];
            Tangents = new Vector4[nTangentsNum];
            int[] nTriangles = new int[nTriNum];
            UVs = new Vector2[nVertNum];
            PosUV = new Vector2[nVertNum];
            AnimUV = new Vector2[nVertNum];

            int vIndex = 0;

            for (int v = 0; v < nVertNum; v++)
            {
                Vertices[v] = oVertices[vIndex];

                if (vIndex >= oMeshVertNum - 1)
                {
                    vIndex = 0;
                }
                else
                {
                    vIndex++;
                }
            }

            int nIndex = 0;

            for (int v = 0; v < nNormNum; v++)
            {
                Normals[v] = oNormals[nIndex];

                if (nIndex >= oMeshNormNum - 1)
                {
                    nIndex = 0;
                }
                else
                {
                    nIndex++;
                }
            }

            int tanIndex = 0;

            for (int v = 0; v < nTangentsNum; v++)
            {
                Tangents[v] = oTangents[tanIndex];

                if (tanIndex >= oMeshTanNum - 1)
                {
                    tanIndex = 0;
                }
                else
                {
                    tanIndex++;
                }
            }

            int tIndex = 0;

            for (int t = 0; t < nTriNum; t++)
            {
                nTriangles[t] = oTriangles[tIndex] + (oMeshVertNum * Mathf.FloorToInt((float)t / (float)oMeshTriNum));

                if (tIndex >= oMeshTriNum - 1)
                {
                    tIndex = 0;
                }
                else
                {
                    tIndex++;
                }
            }

            int uvIndex = 0;

            //Create UVs
            for (int u = 0; u < nVertNum; u++)
            {
                UVs[u] = oUV1[uvIndex];

                int uIndex = Mathf.FloorToInt((float)index / (float)oMeshVertNum);
                index++;
                //Debug.Log(index);
                float uCoord = ((float)uIndex % (float)_Height) / _Height;
                float vCoord = (Mathf.Floor((float)uIndex / (float)_Width)) / _Width;

                PosUV[u] = new Vector2(uCoord, vCoord) + HalfTexelOffset;
                AnimUV[u] = new Vector2(0f, (float)u / (float)oMeshVertNum) + new Vector2(0f, AnimHalfTexelOffset);

                if (uvIndex >= oMeshVertNum - 1)
                {
                    uvIndex = 0;
                }
                else
                {
                    uvIndex++;
                }
            }

            m.vertices = Vertices;
            m.normals = Normals;
            m.tangents = Tangents;
            m.triangles = nTriangles;
            m.uv = PosUV;
            m.uv2 = UVs;
            m.uv3 = AnimUV;
            g.AddComponent<MeshFilter>().mesh = m;
            g.AddComponent<MeshRenderer>().material = _Mat;
            MeshHolders[n] = g;
        }
		
        foreach (GameObject g in MeshHolders)
        {
            g.hideFlags = HideFlags.HideAndDontSave;
        }
		
        return MeshHolders;
    }

    public static Vector4[] AnimationCurveToBezier(AnimationCurve Curve)
    {
        Curve = CheckAnimationCurve(Curve);

        List<Vector4> BezierPoints = new List<Vector4>();

        int numKeys = Curve.keys.Length;

        if (numKeys > 0)
        {
            Keyframe[] keys = Curve.keys;

            for (int i = 0; i < numKeys - 1; i++)
            {
                Vector4 P1C1 = Vector4.zero;
                Vector4 P2C2 = Vector4.zero;

                CurveToBezier(keys[i], keys[i + 1], out P1C1, out P2C2);

                BezierPoints.Add(P1C1);
                BezierPoints.Add(P2C2);
            }
        }

        while (BezierPoints.Count < 10)
        {
            BezierPoints.Add(Vector4.zero);
        }

        return BezierPoints.ToArray();
    }

    public static AnimationCurve CheckAnimationCurve(AnimationCurve Curve)
    {
        int numKeys = Curve.keys.Length;

        if (numKeys > 6)
        {
            Debug.LogWarning("[UPS] Due to Shader optimizations, no more than 6 keyframes are allowed!");
            Keyframe[] newKewFrames = new Keyframe[6];

            for (int i = 0; i < 6; i++)
            {
                newKewFrames[i] = Curve.keys[i];
            }
            Curve.keys = newKewFrames;
        }
        return Curve;
    }

    public static void CurveToBezier(Keyframe k0, Keyframe k1, out Vector4 P1C1, out Vector4 P2C2)
    {

        float tgIn = k1.inTangent;          //inTangent
        float tgOut = k0.outTangent;        //outTangent

        P1C1.x = k0.time;
        P1C1.y = k0.value;
        P2C2.x = k1.time;
        P2C2.y = k1.value;

        float tangLengthX = Mathf.Abs(P1C1.x - P2C2.x) * 0.333333f;
        float tangLengthY = tangLengthX;
        Vector2 c1 = new Vector2(P1C1.x, P1C1.y);
        Vector2 c2 = new Vector2(P2C2.x, P2C2.y);

        c1.x += tangLengthX;
        c1.y += tangLengthY * tgOut;
        c2.x -= tangLengthX;
        c2.y -= tangLengthY * tgIn;

        P1C1.z = c1.x;
        P1C1.w = c1.y;
        P2C2.z = c2.x;
        P2C2.w = c2.y;
    }

	public static Texture2D MeshToTexture(Mesh mesh, GPUParticleSystem.MeshBakeType bakeType, int size)
	{
		int ArraySize = size * size;

		MeshEmitterPositions = new Texture2D(size, size, TextureFormat.RGBAHalf, false);
		MeshEmitterPositions.filterMode = FilterMode.Point;

		if (ArraySize != ColorArray.Length)
		{
			ColorArray = new Color[ArraySize];
			vertexBuffer = mesh.vertices;
			triangleBuffer = mesh.triangles;
			weightsBuffer = new float[mesh.triangles.Length / 3];
		}

		CreateTextureColours(bakeType, ref Vertices, ref triangleBuffer, ref weightsBuffer, ArraySize);

		MeshEmitterPositions.SetPixels(ColorArray);
		MeshEmitterPositions.Apply(false);
		return MeshEmitterPositions;
	}

	private static void CreateTextureColours(GPUParticleSystem.MeshBakeType geometryType, ref Vector3[] vertices, ref int[] tris, ref float[] weights, int arraySize)
	{
		switch (geometryType)
		{
			case GPUParticleSystem.MeshBakeType.Vertex:
				for (int i = 0; i < arraySize; i++)
				{
					int RandomIndex = Random.Range(0, vertices.Length);
					Vector3 Pos = vertices[RandomIndex];
					ColorArray[i] = new Color(Pos.x, Pos.y, Pos.z, 1f);
				}
				break;

			case GPUParticleSystem.MeshBakeType.Edge:
				for (int i = 0; i < arraySize; i++)
				{
					int StartIndex = Random.Range(0, tris.Length / 3) * 3;
					int StartTriangle = Random.Range(0, 3);
					int EndTriangle = 0;

					if (StartTriangle != 2)
					{
						EndTriangle = Random.Range(1, 3);
					}

					Vector3 Pos1 = vertices[tris[StartIndex + StartTriangle]];
					Vector3 Pos2 = vertices[tris[StartIndex + EndTriangle]];

					//Vector3 Pos = Vector3.Lerp(Pos1, Pos2, Rnd.Next());
					Vector3 Pos = Vector3.Lerp(Pos1, Pos2, Random.Range(0f, 1f));
					ColorArray[i] = new Color(Pos.x, Pos.y, Pos.z, 1f);
				}
				break;

			case GPUParticleSystem.MeshBakeType.Triangle:

				float WholeMeshArea = 0.0f;
				int Tri = 0;

				//Calculate Triangle and Whole Triangle Size
				CalculateTriangles(ref vertices, ref tris, ref weights, ref WholeMeshArea, ref Tri);

				//Normalize
				Normalize(ref weights, ref WholeMeshArea);

				//Generate Random Point
				GenerateRandomPoint(ref vertices, ref tris, ref weights, arraySize);
				break;
		}
	}

	private static void CalculateTriangles(ref Vector3[] vertices, ref int[] tris, ref float[] weights, ref float wholeMeshArea, ref int tri)
	{
		int triangleLength = tris.Length;
		for (int j = 0; j < triangleLength; j += 3)
		{
			Vector3 A = vertices[tris[j]];
			Vector3 B = vertices[tris[j + 1]];
			Vector3 C = vertices[tris[j + 2]];
			Vector3 V = Vector3.Cross(A - B, A - C);
			float area = V.magnitude * 0.5f;
			weights[tri] = area;
			++tri;
			wholeMeshArea += area;
		}
	}

	private static void Normalize(ref float[] Weights, ref float WholeMeshArea)
	{
		int weightsLength = Weights.Length;
		for (int j = 0; j < weightsLength; ++j)
		{
			Weights[j] /= WholeMeshArea;
		}
	}

	private static void GenerateRandomPoint(ref Vector3[] Vertices, ref int[] Tris, ref float[] Weights, int ArraySize)
	{
		int weightLength = Weights.Length;
		for (int i = 0; i < ArraySize; ++i)
		{
			float RandomTriangle = Random.Range(0f, 1f);
			float acc = 0.0f;
			int TriangleIndex = 0;
			GetWeights(ref Weights, ref weightLength, ref RandomTriangle, ref acc, ref TriangleIndex);
			Vector3 Pos = GenerateRandomPos(ref Vertices, ref Tris, TriangleIndex);
			AssignColour(i, Pos);
		}
	}

	private static Vector3 GenerateRandomPos(ref Vector3[] Vertices, ref int[] Tris, int TriangleIndex)
	{
		Vector3 Pos1 = Vertices[Tris[TriangleIndex]];
		Vector3 Pos2 = Vertices[Tris[TriangleIndex + 1]];
		Vector3 Pos3 = Vertices[Tris[TriangleIndex + 2]];

		Vector3 Pos = GetRandomPointOnTriangle(Pos1, Pos2, Pos3);
		return Pos;
	}

	private static void AssignColour(int i, Vector3 Pos)
	{
		Color col = ColorArray[i];
		col.r = Pos.x;
		col.g = Pos.y;
		col.b = Pos.z;
		ColorArray[i] = col;
	}

	private static void GetWeights(ref float[] Weights, ref int weightLength, ref float RandomTriangle, ref float acc, ref int TriangleIndex)
	{
		for (int j = 0; j < weightLength; ++j)
		{
			acc += Weights[j];

			if (acc >= RandomTriangle)
			{
				TriangleIndex = j * 3;
				break;
			}
		}
	}

	private static Vector3 GetRandomPointOnTriangle(Vector3 P1, Vector3 P2, Vector3 P3)
	{
		float r1 = Random.Range(0f, 1f);
		float r2 = Random.Range(0f, 1f);

		float Sqrtr1 = Mathf.Sqrt(r1);
		float negSqrtr1 = 1f - Mathf.Sqrt(r1);

		return new Vector3((negSqrtr1) * P1.x + (Sqrtr1 * (1f - r2)) * P2.x + (Sqrtr1 * r2) * P3.x,
			(negSqrtr1) * P1.y + (Sqrtr1 * (1f - r2)) * P2.y + (Sqrtr1 * r2) * P3.y,
			(negSqrtr1) * P1.z + (Sqrtr1 * (1f - r2)) * P2.z + (Sqrtr1 * r2) * P3.z
			);
	}

	//Skinned Mesh Emitter
	//Splits all triangles (unique vertices, normals etc.) and adds 2nd UV set.
	public static Mesh ProcessSkinnedMesh(Mesh mesh)
    {
        mesh = SplitTriangles(mesh);
        vertexBuffer = mesh.vertices;
        triangleBuffer = mesh.triangles;
        uV1Buffer = mesh.uv;
        uV2Buffer = mesh.uv;
        numTriangles = triangleBuffer.Length / 3;
        weightsBuffer = new float[numTriangles];

        WeightTriangles(vertexBuffer, triangleBuffer, ref weightsBuffer);

        float left = 0f;
        float right = 0f;

        for (int i = 0; i < numTriangles; i += 2)
        {
            float a = weightsBuffer[i] * 2f;
            float b = weightsBuffer[i + 1] * 2f;

            if (left + a > 1f)
            {
                uV2Buffer[triangleBuffer[i * 3]] = new Vector2(1f, right + b);
                uV2Buffer[triangleBuffer[i * 3 + 1]] = new Vector2(0f, left);
                uV2Buffer[triangleBuffer[i * 3 + 2]] = new Vector2(1f, right);
                right += a;
            }
            else
            {
                uV2Buffer[triangleBuffer[i * 3]] = new Vector2(0f, left);
                uV2Buffer[triangleBuffer[i * 3 + 1]] = new Vector2(1f, right);
                uV2Buffer[triangleBuffer[i * 3 + 2]] = new Vector2(0f, left + a);
                left += a;
            }

            if (right + b > 1f)
            {
                uV2Buffer[triangleBuffer[i * 3 + 3]] = new Vector2(0f, left);
                uV2Buffer[triangleBuffer[i * 3 + 4]] = new Vector2(1f, right);
                uV2Buffer[triangleBuffer[i * 3 + 5]] = new Vector2(0f, left + a);
                left += b;
            }
            else
            {
                uV2Buffer[triangleBuffer[i * 3 + 3]] = new Vector2(1f, right + b);
                uV2Buffer[triangleBuffer[i * 3 + 4]] = new Vector2(0f, left);
                uV2Buffer[triangleBuffer[i * 3 + 5]] = new Vector2(1f, right);
                right += b;
            }
        }

        mesh.uv2 = uV2Buffer;
        Debug.Log("[GPU P] Processed skinned mesh. New mesh has: "+ vertexBuffer.Length + " Vertices and "+ numTriangles + " Triangles ");
        return mesh;
    }

    private static Mesh SplitTriangles(Mesh mesh)
    {
        Mesh mesh2 = new Mesh();
        mesh2.name = mesh.name + " converted";
        Vector3[] vert = mesh.vertices;
        int[] tris = mesh.triangles;
        Vector2[] uv1 = mesh.uv;
        Vector3[] norm = mesh.normals;
        Vector4[] tan = mesh.tangents;
        BoneWeight[] boneWeights = mesh.boneWeights;

        List<Vector3> newVerts = new List<Vector3>();
        List<int> newTris = new List<int>();
        List<Vector2> newUvs1 = new List<Vector2>();
        List<Vector3> newNorm = new List<Vector3>();
        List<Vector4> newTan = new List<Vector4>();
        List<BoneWeight> newBoneWeights = new List<BoneWeight>();

        int tri = 0;

        for (int i = 0; i < tris.Length; i += 3)
        {
            int tri1 = tris[i];
            int tri2 = tris[i + 1];
            int tri3 = tris[i + 2];

            float tSize = TriangleSize(vert[tri1], vert[tri2], vert[tri3]);

            if (tSize < ignoreSize)
            {
                continue;
            }

            newVerts.Add(vert[tri1]);
            newVerts.Add(vert[tri2]);
            newVerts.Add(vert[tri3]);

            newTris.Add(tri);
            tri++;
            newTris.Add(tri);
            tri++;
            newTris.Add(tri);
            tri++;

            newUvs1.Add(uv1[tri1]);
            newUvs1.Add(uv1[tri2]);
            newUvs1.Add(uv1[tri3]);

            newNorm.Add(norm[tri1]);
            newNorm.Add(norm[tri2]);
            newNorm.Add(norm[tri3]);

            newTan.Add(tan[tri1]);
            newTan.Add(tan[tri2]);
            newTan.Add(tan[tri3]);

            newBoneWeights.Add(boneWeights[tri1]);
            newBoneWeights.Add(boneWeights[tri2]);
            newBoneWeights.Add(boneWeights[tri3]);

        }

        mesh2.vertices = newVerts.ToArray();
        mesh2.triangles = newTris.ToArray();
        mesh2.uv = newUvs1.ToArray();
        mesh2.normals = newNorm.ToArray();
        mesh2.tangents = newTan.ToArray();
        mesh2.boneWeights = newBoneWeights.ToArray();
        mesh2.RecalculateBounds();
        mesh2.bindposes = mesh.bindposes;
        return mesh2;
    }

    private static void WeightTriangles(Vector3[] verts, int[] tris, ref float[] weights)
    {
        for (int i = 0; i < numTriangles; i++)
        {
            weights[i] = TriangleSize(verts[tris[i * 3]], verts[tris[i * 3 + 1]], verts[tris[i * 3 + 2]]);
            Buffer += weights[i];
        }

        for (int i = 0; i < numTriangles; i++)
        {
            weights[i] /= Buffer;
        }
    }

    private static float TriangleSize(Vector3 V1, Vector3 V2, Vector3 V3)
    {
        vectorBuffer = Vector3.Cross(V1 - V2, V1 - V3);
        sizeBuffer = vectorBuffer.magnitude * 0.5f;
        return sizeBuffer;
    }
}
