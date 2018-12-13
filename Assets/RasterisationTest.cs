using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

	public const int RESX=160;
	public const int RESY=90;
	Conversion conversion;
	Matrix4x4 RotationMatrix;
	Matrix4x4 ProjectionMatrix;
	Vector3[] Cube;
	
	Color COLOR = Color.black;

	private Vector3[] TransformVertices(Vector3[] vertex, Matrix4x4 transformMatrix)
    {
        Vector3[] output = new Vector3[vertex.Length];

        for (int i = 0; i<vertex.Length;i++)
        {
            Vector4 homogVert = new Vector4(vertex[i].x, vertex[i].y, vertex[i].z, 1);
            Vector4 homogImage = transformMatrix * homogVert;
            output[i] = new Vector3(homogImage.x, homogImage.y, homogImage.z);

        }

        return output;
    }

	public void InitProjectionMatrix()
	{
		ProjectionMatrix = Matrix4x4.Perspective(110, RESX/RESY, 1, 1000);
	}

	public void InitCube()
	{
		// Create initial cube
		Cube = new Vector3[8];
        Cube[0] = new Vector3(1, 1, 1);
        Cube[1] = new Vector3(-1, 1, 1);
        Cube[2] = new Vector3(-1, -1, 1);
        Cube[3] = new Vector3(1, -1, 1);
        Cube[4] = new Vector3(1, 1, -1);
        Cube[5] = new Vector3(-1, 1, -1);
        Cube[6] = new Vector3(-1, -1, -1);
        Cube[7] = new Vector3(1, -1, -1);

		// Translation
        Vector3 translation = new Vector3(-0.75f, 0, 0);
        Matrix4x4 translationMatrix = Matrix4x4.TRS(translation,
                                                    Quaternion.identity,
                                                    Vector3.one);
        Cube =  TransformVertices(Cube, translationMatrix);		
	}

	private Vector2[] ProjectCube(Vector3[] cube, Matrix4x4 projectionMatrix)
    {
        Vector3[] projectionImage = TransformVertices(cube, projectionMatrix);

		// To Vector2[]
		Vector2[] projectedCube = new Vector2[projectionImage.Length];
		for(int i=0; i<projectionImage.Length; i++)
		{
			projectedCube[i].x = projectionImage[i].x / -(projectionImage[i].z);
			projectedCube[i].y = projectionImage[i].y / -(projectionImage[i].z);
		}

		return projectedCube;
    }

	private Vector2[][] CreateCubeLines(Vector2[] projectedCube)
    {
        Vector2[][] cubeLines = new Vector2[12][];
		cubeLines[0] = new Vector2[]{projectedCube[0], projectedCube[1]};
		cubeLines[1] = new Vector2[]{projectedCube[1], projectedCube[2]};
		cubeLines[2] = new Vector2[]{projectedCube[2], projectedCube[3]};
		cubeLines[3] = new Vector2[]{projectedCube[3], projectedCube[0]};
		cubeLines[4] = new Vector2[]{projectedCube[0], projectedCube[4]};
		cubeLines[5] = new Vector2[]{projectedCube[4], projectedCube[7]};
		cubeLines[6] = new Vector2[]{projectedCube[7], projectedCube[3]};
		cubeLines[7] = new Vector2[]{projectedCube[1], projectedCube[5]};
		cubeLines[8] = new Vector2[]{projectedCube[5], projectedCube[6]};
		cubeLines[9] = new Vector2[]{projectedCube[6], projectedCube[2]};
		cubeLines[10] = new Vector2[]{projectedCube[4], projectedCube[5]};
		cubeLines[11] = new Vector2[]{projectedCube[6], projectedCube[7]};
		return cubeLines;
    }

	void InitRotationMatrix()
	{
		Vector3 startingAxis = Vector3.forward;
        startingAxis.Normalize();
        Quaternion rotation = Quaternion.AngleAxis(1, startingAxis);
    	RotationMatrix = Matrix4x4.TRS(new Vector3(0, 0, 0),
                            rotation,
                            Vector3.one);
	}

	void DrawOnPlan()
	{
		Vector2[] projectedCube = ProjectCube(Cube, ProjectionMatrix);
		Vector2[][] cubeLines = CreateCubeLines(projectedCube);
	
		List<Vector2[]> clippedCube = new List<Vector2[]>(cubeLines.Length);
		for(int i=0; i<cubeLines.Length; i++)
		{
			if(LineClipping.LineClip(ref cubeLines[i][0], ref cubeLines[i][1]))
			{
				Vector2[] line = new Vector2[2];
				line[0] = cubeLines[i][0];
				line[1] = cubeLines[i][1];
				clippedCube.Add(line);
			}
		}
		
		Vector2Int[][] convertedCube = new Vector2Int[clippedCube.Count][];
		for (int i=0; i<convertedCube.Length; i++)
		{
			convertedCube[i] = new Vector2Int[2];
			convertedCube[i][0] = conversion.Convert(clippedCube[i][0]);
			convertedCube[i][1] = conversion.Convert(clippedCube[i][1]);
		}

		Vector2Int[][] drawnCube = new Vector2Int[convertedCube.Length][];
		for(int i=0; i<convertedCube.Length; i++)
		{
			drawnCube[i] = Rasterisation.RasteriseLine(convertedCube[i][0], convertedCube[i][1]);
		}

		Texture2D texture = new Texture2D(conversion.ResX, conversion.ResY);
        GetComponent<Renderer>().material.mainTexture = texture;

		foreach(Vector2Int[] line in drawnCube)
			foreach(Vector2Int point in line)
				texture.SetPixel(point.x, point.y, COLOR);

		texture.Apply(false);
	}

	// Use this for initialization
	void Start () {
		InitCube();
		InitRotationMatrix();
		InitProjectionMatrix();
		conversion = new Conversion(RESX, RESY);

		DrawOnPlan();
	}


    void Update () {
		Cube = TransformVertices(Cube, RotationMatrix);
		DrawOnPlan();
	}
}
