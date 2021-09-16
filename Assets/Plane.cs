using System;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Plane : MonoBehaviour {
	public GameObject sensor1;
	public GameObject sensor2;
	public GameObject sensor3;
	public GameObject sensor4;
	public GameObject source;
	public GameObject dynamic_Mesh;

	public InputField source_location_InputField;
	public Text sourcePredicted_Xmax_Ymax;
	public Text sourcePredicted_Xmax_Ymin;
	public Text sourcePredicted_Xmin_Ymax;
	public Text sourcePredicted_Xmin_ymin;

	public InputField samplingRate_InputField;
	public Scrollbar samplingRate_Scrollbar;

	public InputField sourcex_InputField;
	public Scrollbar sourcex_Scrollbar;
	public InputField sourcey_InputField;
	public Scrollbar sourcey_Scrollbar;

	public float speed;
	private Vector3 final_Xmax_Ymax;
	private Vector3 final_Xmax_Ymin;
	private Vector3 final_Xmin_Ymax;
	private Vector3 final_Xmin_Ymin;

	private float samplingRate;
	// Use this for initialization
	void Start () {
		final_Xmax_Ymax = new Vector3(0f,0f,0f);
		final_Xmax_Ymin = new Vector3(0f,0f,0f);
		final_Xmin_Ymax = new Vector3(0f,0f,0f);
		final_Xmin_Ymin = new Vector3(0f,0f,0f);
		samplingRate = 1000f;
		source_location_InputField.text = source.transform.position.x*100 + "cm" + "," + source.transform.position.z*100 + "cm";
		samplingRate = (samplingRate_Scrollbar.value) * 44100;
		samplingRate_InputField.text = samplingRate.ToString() + "Hz";
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void Calculate(){
		float distancetosensor1 = (sensor1.transform.position - source.transform.position).magnitude;
		float distancetosensor2 = (sensor2.transform.position - source.transform.position).magnitude;
		float distancetosensor3 = (sensor3.transform.position - source.transform.position).magnitude;
		float distancetosensor4 = (sensor4.transform.position - source.transform.position).magnitude;
		float t1 = distancetosensor1 / speed;
		float t2 = distancetosensor2 / speed;
		float t3 = distancetosensor3 / speed;
		float t4 = distancetosensor4 / speed;
		float dtx = t2 - t1;
		float dty = t4 - t3;

		float dtx_max = dtx + 2 * 1/samplingRate;
		float dtx_min = dtx - 2 * 1/samplingRate;

		float dty_max = dty + 2 * 1/samplingRate;
		float dty_min = dty - 2 * 1/samplingRate;

		final_Xmax_Ymax = Calculate_Final (dtx_max, dty_max);
		final_Xmax_Ymin = Calculate_Final (dtx_max, dty_min);
		final_Xmin_Ymax = Calculate_Final (dtx_min, dty_max);
		final_Xmin_Ymin = Calculate_Final (dtx_min, dty_min);
	}

	Vector3 Calculate_Final(float dtx ,float dty){
		float aX = speed * dtx / 2;
		float aY = speed * dty / 2;
		float cX = sensor1.transform.position.x;
		float cY = sensor3.transform.position.z;

		float bX = Mathf.Sqrt (cX * cX - aX * aX);
		float bY = Mathf.Sqrt (cY * cY - aY * aY);
		float den = Mathf.Sqrt (bX * bX * bY * bY - aX * aX * aY * aY);
		float finalX = aX * bY * Mathf.Sqrt (aY * aY + bX * bX)/den;
		float finalY = aY * bX * Mathf.Sqrt (aX * aX + bY * bY)/den;

		return new Vector3(finalX,0,finalY);
	}

	void Update_UI(){
		source_location_InputField.text = source.transform.position.x*100 + "cm" + "," + source.transform.position.z*100 + "cm";
		sourcePredicted_Xmax_Ymax.text = final_Xmax_Ymax.x*100 + "cm" + "," + final_Xmax_Ymax.z*100 + "cm";
		sourcePredicted_Xmax_Ymin.text = final_Xmax_Ymin.x*100 + "cm" + "," + final_Xmax_Ymin.z*100 + "cm";
		sourcePredicted_Xmin_Ymax.text = final_Xmin_Ymax.x*100 + "cm" + "," + final_Xmin_Ymax.z*100 + "cm";
		sourcePredicted_Xmin_ymin.text = final_Xmin_Ymin.x*100 + "cm" + "," + final_Xmin_Ymin.z*100 + "cm";
	}

	public void Change_SourceLocation(){
		string[] words = source_location_InputField.text.Split (',');

		string[] x = words [0].Split ('c');

		string[] y = words [1].Split ('c');

		try{
			source.transform.position = new Vector3 (float.Parse (x [0])/100, 0, float.Parse (y [0])/100);
		}
		catch(Exception e){
			Debug.Log (e);
			Debug.Log("change_sourcelocation");
		}

	}
	public void Change_SamplingRate(){
		samplingRate = (samplingRate_Scrollbar.value) * 44100;
		samplingRate_InputField.text = samplingRate.ToString() + "Hz";
	}

	public void Change_SamplingRate_2(){
		//string[] words = samplingRate_InputField.text.Split ('H');
		string[] words = Regex.Split(samplingRate_InputField.text, @"\D+");

		try{
			samplingRate = float.Parse (words[0]);
			samplingRate_Scrollbar.value = (samplingRate) / 44100;
		}
		catch(Exception e){
			Debug.Log (e);
			Debug.Log ("change_samplingrate_2");
		}

	}

	public void Change_SensorXPosition(){
		sensor1.transform.position = new Vector3 (sourcex_Scrollbar.value, 0, 0);
		sensor2.transform.position = new Vector3 (-sourcex_Scrollbar.value, 0, 0);
		sourcex_InputField.text = sensor1.transform.position.x*100 +"cm";
	}

	public void Change_SensorXPosition_2(){
		string[] words = sourcex_InputField.text.Split ('c');

		try{
			sourcex_Scrollbar.value = float.Parse(words[0])/100;
		}
		catch(Exception e){
			Debug.Log (e);
			Debug.Log ("change_samplingrate_2");
		}
	}

	public void Change_SensorYPostion(){
		sensor3.transform.position = new Vector3 (0, 0, sourcey_Scrollbar.value);
		sensor4.transform.position = new Vector3 (0, 0, -sourcey_Scrollbar.value);
		sourcey_InputField.text = sensor3.transform.position.z*100 +"cm";
	}

	public void Change_SensorYPosition_2(){
		string[] words = sourcey_InputField.text.Split ('c');

		try{
			sourcey_Scrollbar.value = float.Parse(words[0])/100;
		}
		catch(Exception e){
			Debug.Log (e);
			Debug.Log ("change_samplingrate_2");
		}
	}

	void Change_Dynamic_Mesh(){
		if(float.IsNaN(final_Xmax_Ymax.x)||float.IsNaN(final_Xmax_Ymax.z)||
			float.IsNaN(final_Xmax_Ymin.x)||float.IsNaN(final_Xmax_Ymin.z)||
			float.IsNaN(final_Xmin_Ymax.x)||float.IsNaN(final_Xmin_Ymax.z)||
			float.IsNaN(final_Xmin_Ymin.x)||float.IsNaN(final_Xmin_Ymin.z)){
			dynamic_Mesh.GetComponent<Dynamic_Mesh> ().Fill_Space ();
			return;
		}
		try{
			Vector3[] vertices = new Vector3[] {final_Xmax_Ymax,final_Xmax_Ymin,final_Xmin_Ymax,final_Xmin_Ymin};
			dynamic_Mesh.GetComponent<Dynamic_Mesh> ().Create_Mesh (vertices);
		}
		catch(Exception e){
			Debug.Log (e);
			Debug.Log ("change_dynamic_mesh");
		}
	}
	public void run(){
		Calculate ();
		Update_UI ();
		Change_Dynamic_Mesh ();
	}
}
