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
		wallcontainer.tag = "LevelWallContainer";
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
				// remove Walls if the player is dead
				if(gameState.isState(GameStateManager.GamesState.GAME_ENDED)){
					// game ended, remove all containers and game objects from game
					GameObject.Destroy(this.wallcontainer);
					if(networkView.isMine){
						Debug.Log("Removing GameObject: "+gameObject.name);
						Network.Destroy(gameObject);
					}
				} else {
					//player is dead, hide walls
					float wallHidePerc = (Time.time - _hideWallsTimeSince) / _hideWallsDuration;
					if(wallHidePerc < 1.0f){
						float MAGIC_CLIPPING_VAL = 1.2f;
						foreach(Transform childwall in this.wallcontainer.GetComponentsInChildren<Transform>()){
							childwall.position = childwall.position - Vector3.up * wallHeight * (Time.deltaTime/_hideWallsDuration) * MAGIC_CLIPPING_VAL;
						}
					}
				}
			} else {
				if(networkView.isMine){
					//player is alive, update walls
					Vector3 currPos = currentBikePos();
					//check if bike was rotated
					if(lastDirectionIndex != inputController.getDirectionIndex()){
						if(lastCreatedWall != null){
							//make sure wall is closed in the corners
							extendWall(lastCreatedWall, lastCornerPos, inputController.getLastCorner());
						} else {
						
						}
						Vector3 lastCornerPosition = inputController.getLastCorner();
						int directionIndexInt = inputController.getDirectionIndex();
						lastCreatedWall = createWall(lastCornerPosition, directionIndexInt, inputController.getPlayerNumber(), inputController.belongsToPlayer);	
						lastCornerPos = inputController.getLastCorner();
						lastDirectionIndex = inputController.getDirectionIndex();
					} else {
						if(lastCreatedWall != null){
							extendWall(lastCreatedWall, lastCornerPos, currPos);
						} else {
							lastCreatedWall = createWall(currPos, lastDirectionIndex, inputController.getPlayerNumber(), inputController.belongsToPlayer);	
						}
					}
				}
			}
		}
	}
	
	Vector3 currentBikePos(){
		Transform b1Trans = bike1.GetComponent<Transform>();
		return b1Trans.position;
	}
	
	GameObject createWall(Vector3 start, int direction, int material, string playerid){
		GameObject wall = Network.Instantiate(wallPrefab, start, Quaternion.Euler(new Vector3(0, direction*90, 0)), 0) as GameObject;
		NetworkViewID viewId = wall.GetComponent<NetworkView>().networkView.viewID;
		networkView.RPC("setWallProperties", RPCMode.AllBuffered, viewId, int.Parse(Network.player.ToString()), playerid);
		return wall;
	}
	
	[RPC]
	void setWallProperties(NetworkViewID networkViewId, int material, string belongsToPlayer){
		GameObject wall = NetworkView.Find(networkViewId).gameObject;
		MeshRenderer renderer = wall.GetComponent<MeshRenderer>();
		Material wallmaterial = wall.GetComponent<WallMaterials>().wallMaterials[material];
		renderer.material = wallmaterial;
		wall.GetComponent<WallOwner>().setOwner(belongsToPlayer, material);
		
		//add wall to bike wall container
		wall.transform.parent = wallcontainer.transform;
		wall.name = "bike_wall";

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
