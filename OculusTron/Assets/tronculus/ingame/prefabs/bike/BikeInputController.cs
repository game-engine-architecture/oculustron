using UnityEngine;
using System.Collections;

public class BikeInputController : MonoBehaviour {
	
	public CharacterController characterController;
	private ParticleSystem particles;
	Transform modelTransform;
	public float movementSpeed = 1f;
	public float rotationSpeed = 9.4f;
	Vector3 rotation = new Vector3(1,0,0);
	int directionIndex = 0;
	Vector3 lastCorner;
	public bool isAIControlled = false;
	GameStateManager gameState;
	int playerNumber = 0;
	
	public string belongsToPlayer;
	private bikelogic wallLogic;
	public GameObject explosionPrefab;
	
	AudioSource motorSound;
	TurnSoundPlayer turnSoundPlayer;
	
	public float lastSpeedPowerUp = -100000f; //never
	public float powerUpSpeedMultiplicator;
	public float powerUpSpeedDuration;
	public float lastThroughWallPowerUp = -100000f; //never
	public float powerUpThroughWallDuration;
	
	float aiLastTurn = -1f;
	float aiMinimumTurnWaitTime = 1f;
	//float aiRandomTurnPropabilityPerSec = 0.976f;
	float aiRandomTurnPropabilityPerSec = 0.976f;
	public bool aiObjectInFront = false;
	
	enum Direction {FORWARD, LEFT, RIGHT};
	
	// Use this for initialization
	void Start () {
		particles = this.GetComponent<ParticleSystem>();
		wallLogic = this.GetComponent<bikelogic>();
		directionIndex = (int)(Mathf.Round(this.gameObject.transform.rotation.eulerAngles.y/90f))+3 % 4;
		rotation = Quaternion.AngleAxis(90 * (directionIndex), Vector3.up) * rotation;
		lastCorner = this.gameObject.transform.position;

		this.modelTransform = this.GetComponent<Transform>();
		gameState = GameObject.Find("GameState").GetComponent<GameStateManager>();
		this.motorSound = this.gameObject.GetComponent<AudioSource>();
		this.turnSoundPlayer = this.gameObject.GetComponentInChildren<TurnSoundPlayer>();
	}
	
	// Update is called once per frame
	void Update () {
		if(gameState.isState(GameStateManager.GamesState.GAME_RUNNING)) {
			particles.enableEmission = true;
		} else {
			particles.enableEmission = false;
		}
		if (networkView.isMine){
			if(gameState.isState(GameStateManager.GamesState.GAME_RUNNING)) {
				if(!this.motorSound.isPlaying){
					this.motorSound.Play();
				}
				if(aiLastTurn < 0){
					aiLastTurn = Time.time - aiMinimumTurnWaitTime;
					//initialize on game start, so that bots can turn immediately
				}
				float deltaT = Time.deltaTime;
				Direction bikeDirection = Direction.FORWARD;
				if(isAIControlled){
					bikeDirection = calculateAIMovement();
				} else {
					if(Input.GetKeyDown ("left")){
						bikeDirection = Direction.LEFT;
					} else if(Input.GetKeyDown ("right")){
						bikeDirection = Direction.RIGHT;
					}
				}
				if (bikeDirection.Equals(Direction.LEFT)){
					rotation = Quaternion.AngleAxis(-90, Vector3.up) * rotation;
					directionIndex += 1;
					directionIndex %= 4;
					lastCorner = this.gameObject.transform.position;
					this.turnSoundPlayer.playTurnSound();
				} else if(bikeDirection.Equals(Direction.RIGHT)){
					rotation = Quaternion.AngleAxis(90, Vector3.up) * rotation;
					directionIndex += 3;
					directionIndex %= 4;
					lastCorner = this.gameObject.transform.position;
					this.turnSoundPlayer.playTurnSound();
				}
				
				float rotadiff = Vector3.Dot(rotation.normalized, this.modelTransform.right.normalized);
				rotadiff *= deltaT * rotationSpeed;
				this.modelTransform.rotation *= Quaternion.AngleAxis(rotadiff * 180, Vector3.up);
				Vector3 pos = this.modelTransform.position;
				pos.y = 0;
				this.modelTransform.position = pos;
				
				float vert;
				//bikes always move forward
				if(gameState.debugMode){
					vert = Input.GetAxis("Vertical");
				} else {
					vert = 1f;
				}
				Vector3 movement = new Vector3(0,0,0);
				float currentMovementSpeed = movementSpeed;
				
				// increase speed with powerup
				if(Time.time < lastSpeedPowerUp + powerUpSpeedDuration){
					currentMovementSpeed *= powerUpSpeedMultiplicator;
				}
				movement += rotation * vert * currentMovementSpeed;
				characterController.Move(movement);
			} else if(gameState.isState(GameStateManager.GamesState.GAME_ENDED)){
				die ();
			}
		}
	}
	
	public void setPlayerNumber(int num){
		this.playerNumber = num;
	}
	public int getPlayerNumber(){
		return this.playerNumber;
	}
	
	private Direction calculateAIMovement(){
		bool turnNow = false;
		//turn if there is something in front
		turnNow |= aiObjectInFront;
		if(turnNow){
			Debug.Log("Trying to turn to dodge!");
		}
		aiObjectInFront = false;
		
		//if there is nothing turn anyway sometimes
		if(!turnNow){
			if(aiLastTurn + aiMinimumTurnWaitTime < Time.time){
				turnNow |= Random.value * Time.deltaTime > 1f - aiRandomTurnPropabilityPerSec;
			}
		}
		if(turnNow){
			aiLastTurn = Time.time;
			if(Random.value > 0.5f){
				return Direction.LEFT;
			} else {
				return Direction.RIGHT;
			}
		}
		return Direction.FORWARD;
	}
	
	public int getDirectionIndex(){
		return this.directionIndex;
	}
	
	public Vector3 getLastCorner(){
		return this.lastCorner;
	}
	
	
	enum GameEvent {
		POWERUP_WALL = 0,	
		POWERUP_FAST = 1,
		DIE = 2
	}
	
	[RPC]
	public void actionEvent(NetworkViewID playerid, int instrution ){
		GameEvent currEvent = (GameEvent) instrution;
		GameObject bike = NetworkView.Find(playerid).gameObject;
		BikeInputController controller = bike.GetComponent<BikeInputController>();
		if(this.belongsToPlayer.Equals(playerid)){
			if(currEvent.Equals(GameEvent.POWERUP_WALL)){
				controller.lastSpeedPowerUp = Time.time;
			} else if(currEvent.Equals(GameEvent.POWERUP_WALL)){
				controller.lastThroughWallPowerUp = Time.time;
			}
		}
	}
	
	void OnControllerColliderHit(ControllerColliderHit hit) {
		
		string playerid = this.gameObject.GetComponent<BikeInputController>().belongsToPlayer;
		NetworkViewID netviewid = this.gameObject.networkView.viewID;
		if(hit.gameObject.name.Equals("FlashPickup")){
			hit.gameObject.SetActive(false);
			networkView.RPC("actionEvent", RPCMode.All, netviewid, (int) GameEvent.POWERUP_FAST);
		} else if(hit.gameObject.name.Equals("TronDisc")){
			hit.gameObject.SetActive(false);
			networkView.RPC("actionEvent", RPCMode.All, netviewid, (int) GameEvent.POWERUP_WALL);
		}
		bool hasWallPowerUp = (lastThroughWallPowerUp + powerUpThroughWallDuration > Time.time);
		if(!hasWallPowerUp){
			//if collision with own wall or collision with outer wall, we lose.
			if(hit.gameObject.name.StartsWith("bike_wall")
				|| hit.gameObject.name.StartsWith("wall")
				){
				string wallowner = hit.gameObject.GetComponent<WallOwner>().getOwner();
				GameObject.Find("GameState").GetComponent<GameStateManager>().notifyPlayerFragged(wallowner, playerid);
				this.die();
			}
		}
		if("Wall".Equals(hit.gameObject.name)){
			//if we hit the level wall we die as well.
			this.die();
		}
		
	}
	
	public void die(){
		this.gameObject.GetComponent<CharacterController>().enabled = false;
		this.gameObject.GetComponent<BikeInputController>().enabled = false;
		this.gameObject.GetComponent<ParticleSystem>().loop = false;
		
		networkView.RPC("playerLost", RPCMode.AllBuffered, belongsToPlayer);
		
		networkView.RPC("createExplosion", RPCMode.All, this.gameObject.transform.position);
		
		if(!isAIControlled){
			GameObject.Find ("Main Camera").GetComponent<CameraInstructor>().showScoreBoardLater();
		}
		
		wallLogic.hideWallsAndDestroyBike();
	}
	
	[RPC]
	void playerLost(string playerid){
		GameObject.Find ("GameState").GetComponent<GameStateManager>().playerLost(playerid);
		if(!Network.player.ToString().Equals(playerid)){
			//update the score board instantly for other players
			GameObject.Find ("ScoreBoardContainer").GetComponent<ScoreBoardUpater>().renderScoreBoard();
		}
	}
	
	[RPC]
	void createExplosion(Vector3 pos){
		Debug.Log ("creating explosion");
		Network.Instantiate(explosionPrefab, pos, Quaternion.identity, 0);
	}
}
