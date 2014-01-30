using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameStateManager : MonoBehaviour {
		
	public enum GamesState {
		MENU = 0,
		WAITING_FOR_PLAYERS = 1,
		GAME_STARTING = 2,
		GAME_RUNNING = 3,
		GAME_ENDED = 4,
		GAME_WAITING = 5,
	}
	
	private GamesState currentGameState;
	private float lastChange;
	private NetworkManagement networkManagement;
	private MenuState menuState;
	private GameObject menu;
	private GameObject level;
	private Material floorMaterial;
	public int gameStartsInSeconds = 5;
	public int gameEndedWait = 5;
	public bool debugMode;
	
	Dictionary<string, int> score = new Dictionary<string, int>();
	List<string> deadPlayers = new List<string>();
	
	private int _botsCount = 3;
	public int botsCount
	{
    	get { return this._botsCount; }
    	set { if ((value>=0)&&(value<9)) this._botsCount = value; }
	}

	private int _playersNeededForGame = 1;
	public int playersNeededForGame
	{
    	get { return this._playersNeededForGame; }
    	set { if ((value>0)&&(value<9)) this._playersNeededForGame = value; }
	}
	
	private int _arenaSizeMultiplicator=8;
	public int arenaSizeMultiplicator
	{
    	get { return this._arenaSizeMultiplicator; }
    	set { if ((value>0)&&(value<33)) {
				this._arenaSizeMultiplicator = value; 
				level.transform.localScale = new Vector3(value,value/2,value);
				floorMaterial.SetTextureScale("_MainTex", new Vector2 (value*30, value*30));
				floorMaterial.SetTextureScale("_BumpMap", new Vector2 (value*30, value*30));
			}
		}
	}
	
	// Use this for initialization
	void Start () {
		level = GameObject.Find("Level");
		this.menu = GameObject.Find("Menu");
		setState(GamesState.MENU);
		this.menuState = GameObject.Find("MenuState").GetComponent<MenuState>();
		networkManagement = GameObject.Find("NetworkManager").GetComponent<NetworkManagement>();
		floorMaterial = GameObject.Find("Floor").GetComponent<MeshRenderer>().material;
		this.arenaSizeMultiplicator = 16;
	}
	
	// Update is called once per frame
	void Update () {
		if(Network.isServer){
			if(isState(GamesState.WAITING_FOR_PLAYERS)){
				//check if enough players have joined
				if(networkManagement.currentPlayerCount() >= _playersNeededForGame + _botsCount){
					networkView.RPC("dictateState", RPCMode.All, (int) GamesState.GAME_STARTING);
					//setState(GamesState.GAME_STARTING);
				}
			} else if(isState(GamesState.GAME_STARTING)){
				// once enough there are enough players, start game after some seconds
				if(lastChangedLongerThan(gameStartsInSeconds)){
					networkView.RPC("dictateState", RPCMode.All, (int) GamesState.GAME_RUNNING);
					//setState(GamesState.GAME_RUNNING);
				}
			} else if(isState(GamesState.GAME_ENDED)){
				if(lastChangedLongerThan(gameEndedWait)){
					networkView.RPC("dictateState", RPCMode.All, (int) GamesState.GAME_STARTING);
					//setState(GamesState.GAME_STARTING);
				}
			}
		}
	}
	
	public void leaveGame(){
		Debug.Log("Leaving Game!");
		networkManagement.leaveGame();
		setState(GamesState.MENU);
		cleanUpArena();
	}
	
	[RPC]
	public void dictateState(int gameState){
		this.setState((GamesState) gameState);
	}
	
	public void setState(GamesState state){
		currentGameState = state;
		lastChange = Time.fixedTime;
		Debug.Log("GAME STATE: "+currentGameState.ToString());
		
		bool showMenu = isState(GamesState.MENU);
		Renderer[] lvlRenderers = level.GetComponentsInChildren<Renderer>();
		foreach (Renderer lvlRenderer in lvlRenderers){
			lvlRenderer.enabled = !showMenu;
		}
		Renderer[] menuRenderers = menu.GetComponentsInChildren<Renderer>();
		foreach (Renderer menuRenderer in menuRenderers){
			menuRenderer.enabled = showMenu;
		}
		
		if(isState (GamesState.MENU)){
			GameObject cam = GameObject.Find("Main Camera");
			if(cam != null){
				cam.GetComponent<CameraInstructor>().cameraFollow(GameObject.Find("MenuCamPos"));
			}	
		}
		
		if(isState (GamesState.GAME_ENDED)){
			deadPlayers.Clear();
			cleanUpArena();
		} else if(isState (GamesState.GAME_STARTING)){
			AudioSource startSound = this.gameObject.GetComponent<AudioSource>();
			startSound.Play();
			deadPlayers.Clear();
			menuState.currentMenuState = 0;
			networkManagement.respawnPlayers();
		}
	}
	
	public void cleanUpArena(){
		//this is quite a hack, but should work out in the end
		GameObject[] bikes = GameObject.FindGameObjectsWithTag("PlayerBike");
		foreach(GameObject bike in bikes){
			GameObject.Destroy(bike);
		}
		GameObject[] bikewalls = GameObject.FindGameObjectsWithTag("LevelWallContainer");
		foreach(GameObject wall in bikewalls){
			GameObject.Destroy(wall);
		}
	}
	
	public GamesState getGameState(){
		return this.currentGameState;
	}
	
	public bool lastChangedLongerThan(float delaySec){
		return Time.fixedTime - lastChange > delaySec;
	}
	
	public float getLastChanged(){
		return lastChange;
	}
	
	public bool isState(GamesState state){
		return currentGameState.Equals(state);
	}
	
	public void addPlayer(string playerid){
		score[playerid] = 0;
		GameObject.Find ("ScoreBoardContainer").GetComponent<ScoreBoardUpater>().renderScoreBoard();
	}
	
	public void playerLost(string playerid){
		deadPlayers.Add(playerid);
		Debug.Log ("dead players count: "+deadPlayers.Count);
		Debug.Log ("players still alive: "+(score.Count - deadPlayers.Count));
		Dictionary<string, int> currentScore = new Dictionary<string, int>(score);
		foreach (KeyValuePair<string, int> pair in currentScore){
			if(deadPlayers.Contains(pair.Key)){
				//every alive player gets a point
				continue;
			}
			score[pair.Key] += 1;
		}
		if(isState(GamesState.GAME_RUNNING)){
			if(score.Count - deadPlayers.Count <= 1){
				setState(GamesState.GAME_ENDED);
			}
		}
	}
	
	public Dictionary<string, int> getScores(){
		return new Dictionary<string, int>(this.score);
	}
}
