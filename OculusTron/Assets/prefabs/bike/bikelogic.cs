using UnityEngine;
using System.Collections;

public class bikelogic : MonoBehaviour {
	
	private GameObject bike1;
	public Material wall1Material;
	
	BikeInputController inputController;
	int lastDirectionIndex = -1;
	Vector3 lastCornerPos;
	GameObject lastCreatedWall;
	GameObject wallcontainer;
	
	float wallHeight = 0.6f;
	float wallWidth = 0.05f;
	
	
	// Use this for initialization
	void Start () {
		bike1 = this.gameObject;
		inputController = bike1.GetComponent<BikeInputController>();
		lastCornerPos = currentBikePos();
		wallcontainer = new GameObject();
		wallcontainer.name = "bike_wall_container";
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 currPos = currentBikePos();
		//check if bike was rotated
		if(lastDirectionIndex != inputController.getDirectionIndex()){
			if(lastCreatedWall != null){
				//make sure wall is closed in the corners
				extendWall(lastCreatedWall, lastCornerPos, inputController.getLastCorner());
			}
			lastCreatedWall = createWall(inputController.getLastCorner(), inputController.getDirectionIndex());	
			lastCornerPos = inputController.getLastCorner();
			lastDirectionIndex = inputController.getDirectionIndex();
		} else {
			extendWall(lastCreatedWall, lastCornerPos, currPos);
		}
	}
	
	Vector3 currentBikePos(){
		Transform b1Trans = bike1.GetComponent<Transform>();
		return b1Trans.position;
	}
	
	GameObject createWall(Vector3 start, int direction){
		//Network.Instantiate("wall", start
		GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
		wall.transform.Rotate(new Vector3(0, direction*90, 0));
		//set matrial
		MeshRenderer renderer = wall.GetComponent<MeshRenderer>();
		renderer.material = wall1Material;
		
		//add wall to bike wall container
		wall.transform.parent = wallcontainer.transform;
		MeshCollider collider = wall.AddComponent<MeshCollider>();
		collider.isTrigger = true;
		
		wall.name = "bike1_wall";
		return wall;
	}
	
	GameObject extendWall(GameObject wall, Vector3 start, Vector3 end){
		float length = Vector3.Distance(start, end);
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
