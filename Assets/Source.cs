using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Source : MonoBehaviour {
	// Use this for initialization
	private Plane p;
	void Start () {
		p = GameObject.FindObjectOfType<Plane> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void OnMouseDrag()
	{
		float distance_to_screen = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
		Vector3 pos_move = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance_to_screen ));
		transform.position = new Vector3( pos_move.x, transform.position.y, pos_move.z );
		p.BroadcastMessage ("Calculate");
		p.BroadcastMessage ("Update_UI");
		p.BroadcastMessage ("Change_Dynamic_Mesh");
	}
}
