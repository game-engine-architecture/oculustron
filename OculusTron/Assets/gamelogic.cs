using UnityEngine;
using System.Collections;

public class gamelogic : MonoBehaviour {
	
	public GameObject bike1;
	Vector3 bike1lastPos;
	
	// Use this for initialization
	void Start () {
		Transform b1Trans = bike1.GetComponent<Transform>();
		bike1lastPos = b1Trans.position;
	}
	
	// Update is called once per frame
	void Update () {
		Transform b1Trans = bike1.GetComponent<Transform>();
		Vector3 currPos = b1Trans.position;
		createWall(bike1lastPos, currPos);
		bike1lastPos = b1Trans.position;
	}
	
	void createWall(Vector3 start, Vector3 end, Vector3 rotation){
		GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Plane);
		wall.AddComponent<BoxCollider>();
		//wall.AddComponent<Rigidbody>();
		Vector3 wallDir = end - start;
		float length = Vector3.Distance(start, end);
		wall.transform.position = ((start+end)/2.0f) + new Vector3(0, 1, 0);
		wall.transform.Rotate(new Vector3(90, 0, 0));
		Vector3 scale = wall.transform.localScale;
		scale.z = 0.1f;
		scale.x = length;
		wall.transform.localScale = scale;
	}	
}
