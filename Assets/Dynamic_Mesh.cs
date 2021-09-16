using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dynamic_Mesh : MonoBehaviour {
	
	void Start()
	{
		
		Mesh mesh = GetComponent<MeshFilter>().mesh;
		mesh.Clear();
		mesh.vertices = new Vector3[] {new Vector3(0, 0, 0), new Vector3(0, 0, 5), new Vector3(5, 0, 0) ,new Vector3(5,0,5)};
		mesh.triangles =  new int[] {0, 1, 2, 1, 3, 2};
	}

	public void Create_Mesh(Vector3[] vertices){

		Mesh mesh = GetComponent<MeshFilter>().mesh;
		mesh.Clear();
		mesh.vertices = vertices;
		mesh.triangles =  new int[] {0, 1, 2, 0, 2, 1, 1, 0, 2, 1, 2, 0, 2, 0, 1, 2, 1, 0, 1, 2, 3, 1, 3, 2, 2, 1, 3, 2, 3, 1, 3, 1, 2, 3, 2, 1};
	}

	public void Fill_Space(){
		Mesh mesh = GetComponent<MeshFilter>().mesh;
		mesh.Clear();
		mesh.vertices = new Vector3[] {new Vector3(-10, 0, -10), new Vector3(-10, 0, 10), new Vector3(10, 0, -10) ,new Vector3(10,0,10)};
		mesh.triangles =  new int[] {0, 1, 2, 1, 3, 2};
	}
}
