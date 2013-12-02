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
	
	float wallHeight = 0.6f;
	float wallWidth = 0.05f;
	float bikeWallOffset = 0.3f;
	
	
	// Use this for initialization
	void Start () {
		Transform b1Trans = bike1.GetComponent<Transform>();
		b1Controller = bike1.GetComponent<BikeInputController>();
		bike1lastPos = currentBikePos();
		bike1wallcontainer = new GameObject();
		bike1wallcontainer.name = "bike1wallcontainer";
	}
	
	// Update is called once per frame
	void Update () {
		Transform b1Trans = bike1.GetComponent<Transform>();
		Vector3 currPos = currentBikePos();
		//check if bike was rotated
		if(bike1lastDirectionIndex != b1Controller.getDirectionIndex()){
			if(bike1lastCreatedWall != null){
				//make sure wall is closed in the corners
				extendWall(bike1lastCreatedWall, bike1lastPos, currPos);
			}
			bike1lastCreatedWall = createWall(currPos, b1Controller.getDirectionIndex());	
			bike1lastPos = currPos;
			bike1lastDirectionIndex = b1Controller.getDirectionIndex();
		} else {
			extendWall(bike1lastCreatedWall, bike1lastPos, currPos);
		}
	}
	
	Vector3 currentBikePos(){
		Transform b1Trans = bike1.GetComponent<Transform>();
		return b1Trans.position;
	}
	
	GameObject createWall(Vector3 start, int direction){
		GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
		wall.transform.Rotate(new Vector3(0, direction*90, 0));
		//set matrial
		MeshRenderer renderer = wall.GetComponent<MeshRenderer>();
		renderer.material = wall1Material;
		
		//add wall to bike wall container
		wall.transform.parent = bike1wallcontainer.transform;
		MeshCollider collider = wall.AddComponent<MeshCollider>();
		collider.isTrigger = true;
		
		wall.name = "bike1_wall";
		b1Controller.currentWall = wall;
		return wall;
	}
	
	GameObject extendWall(GameObject wall, Vector3 start, Vector3 end){
		Vector3 wallDir = end - start;
		float length = Vector3.Distance(start, end);
		Vector3 offset = wallDir.normalized * bikeWallOffset;
		Vector3 wallpos = ((start+end)/2.0f);
		Vector3 scale = wall.transform.localScale;
		scale.y = wallHeight;
		scale.z = wallWidth;
		scale.x = length;
		wall.transform.localScale = scale;
		wall.transform.position = new Vector3(wallpos.x, wallHeight/2.0f, wallpos.z);
	
		return wall;
	}
}
