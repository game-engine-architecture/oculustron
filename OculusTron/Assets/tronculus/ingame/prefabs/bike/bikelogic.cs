using UnityEngine;
using System.Collections;

public class bikelogic : MonoBehaviour {
	
	private GameObject bike1;
	public Material wall1Material;
	public GameObject wallPrefab;
	
	BikeInputController inputController;
	int lastDirectionIndex = -1;
	Vector3 lastCornerPos;
	GameObject lastCreatedWall;
	GameObject wallcontainer;
	float wallHeight = 0.6f;
	float wallWidth = 0.05f;
	int wallnumber = 0;
	
	private bool _hideWalls = false;
	private float _hideWallsTimeSince;
	private float _hideWallsDuration = 1.0f;
	
	GameStateManager gameState;
	
	// Use this for initialization
	void Start () {
		bike1 = this.gameObject;
		inputController = bike1.GetComponent<BikeInputController>();
		lastCornerPos = currentBikePos();
		wallcontainer = new GameObject();
		wallcontainer.transform.position = Vector3.zero;
		wallcontainer.name = "bike_wall_container";
		lastDirectionIndex = inputController.getDirectionIndex();
		gameState = GameObject.Find("GameState").GetComponent<GameStateManager>();
	}
	
	public void hideWallsAndDestroyBike(){
		this._hideWalls = true;
		this._hideWallsTimeSince = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		if(gameState.isState(GameStateManager.GamesState.GAME_RUNNING) || gameState.isState(GameStateManager.GamesState.GAME_ENDED)){
			if(this._hideWalls){
				//player is dead, hide walls
				float wallHidePerc = (Time.time - _hideWallsTimeSince) / _hideWallsDuration;
				if(wallHidePerc < 1.0f && !gameState.isState(GameStateManager.GamesState.GAME_ENDED)){
					float MAGIC_CLIPPING_VAL = 1.2f;
					this.wallcontainer.transform.position = - Vector3.up * wallHidePerc * wallHeight * MAGIC_CLIPPING_VAL;
				} else {
					if(networkView.isMine){
						Debug.Log("Removing GameObject: "+gameObject.name);
						Network.Destroy(gameObject);
					}
					GameObject.Destroy(this.wallcontainer);
				}
			} else {
				//player is alive, update walls
				Vector3 currPos = currentBikePos();
				//check if bike was rotated
				if(lastDirectionIndex != inputController.getDirectionIndex()){
					if(lastCreatedWall != null){
						//make sure wall is closed in the corners
						extendWall(lastCreatedWall, lastCornerPos, inputController.getLastCorner());
					} else {
					
					}
					lastCreatedWall = createWall(inputController.getLastCorner(), inputController.getDirectionIndex());	
					lastCornerPos = inputController.getLastCorner();
					lastDirectionIndex = inputController.getDirectionIndex();
				} else {
					if(lastCreatedWall != null){
						extendWall(lastCreatedWall, lastCornerPos, currPos);
					} else {
						lastCreatedWall = createWall(currPos, lastDirectionIndex);	
					}
				}
			}
		}
	}
	
	Vector3 currentBikePos(){
		Transform b1Trans = bike1.GetComponent<Transform>();
		return b1Trans.position;
	}
	
	[RPC]
	GameObject createWall(Vector3 start, int direction){
		GameObject wall = Network.Instantiate(wallPrefab, start, Quaternion.Euler(new Vector3(0, direction*90, 0)), 0) as GameObject;
		//set matrial
		MeshRenderer renderer = wall.GetComponent<MeshRenderer>();
		Material wallmaterial = wall.GetComponent<WallMaterials>().wallMaterials[inputController.getPlayerNumber()];
		renderer.material = wallmaterial;
		
		//add wall to bike wall container
		wall.transform.parent = wallcontainer.transform;
		//MeshCollider collider = wall.AddComponent<MeshCollider>();
		//collider.isTrigger = true;
		
		wall.name = "bike_wall"+wallnumber;
		wallnumber++;
		return wall;
	}
	
	void OnNetworkInstantiate(NetworkMessageInfo info) {
        Debug.Log("New object instantiated by " + info.sender);
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
