using UnityEngine;
using System.Collections;

public class gamelogic : MonoBehaviour {
	
	public GameObject bike1;
	public Material wall1Material;
	
	BikeInputController b1Controller;
	int bike1lastDirectionIndex = -1;
	Vector3 bike1lastPos;
	GameObject bike1lastCreatedWall;
	GameObject bike1wallcontainer;
	
	float wallHeight = 1f;
	float wallWidth = 0.05f;
	float bikeWallOffset = 0.3f;
	
	
	// Use this for initialization
	void Start () {
		Transform b1Trans = bike1.GetComponent<Transform>();
		b1Controller = bike1.GetComponent<BikeInputController>();
		bike1lastPos = b1Trans.position;
		bike1wallcontainer = new GameObject();
	}
	
	// Update is called once per frame
	void Update () {
		Transform b1Trans = bike1.GetComponent<Transform>();
		Vector3 currPos = b1Trans.position;
		//check if bike was rotated
		if(bike1lastDirectionIndex != b1Controller.getDirectionIndex()){
			bike1lastCreatedWall = createWall(bike1lastPos, b1Controller.getDirectionIndex());	
			bike1lastPos = b1Trans.position;
			bike1lastDirectionIndex = b1Controller.getDirectionIndex();
		} else {
			extendWall(bike1lastCreatedWall, bike1lastPos, currPos);
		}
		
	}
	
	GameObject createWall(Vector3 start, int direction){
		GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
		wall.transform.Rotate(new Vector3(0, direction*90, 0));
		//set matrial
		MeshRenderer renderer = wall.GetComponent<MeshRenderer>();
		renderer.material = wall1Material;
		//add wall to bike wall container
		wall.transform.parent = bike1wallcontainer.transform;
		wall.AddComponent<MeshCollider>();
		
		return wall;
	}
	
	GameObject extendWall(GameObject wall, Vector3 start, Vector3 end){
		Vector3 wallDir = end - start;
		float length = Vector3.Distance(start, end);
		Vector3 offset = wallDir.normalized * bikeWallOffset;
		wall.transform.position = ((start+end-offset)/2.0f) + offset;
		Vector3 scale = wall.transform.localScale;
		scale.y = wallHeight;
		scale.z = wallWidth;
		scale.x = length;
		wall.transform.localScale = scale;
	
		return wall;
	}
}
