using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameStateManager : MonoBehaviour {
		
	public enum GamesState {
		MENU,
		WAITING_FOR_PLAYERS,
		GAME_STARTING,
		GAME_RUNNING,
		GAME_ENDED,
		GAME_WAITING,
	}
	
	
	private GamesState currentGameState;
	private float lastChange;
	private NetworkManagement networkManagement;
	private MenuState menuState;
	public int gameStartsInSeconds = 5;
	public int gameEndedWait = 5;
	
	Dictionary<string, int> score = new Dictionary<string, int>();
	List<string> deadPlayers = new List<string>();
	
	private int _botsCount = 3;
	public int botsCount
	{
    	get { return this._botsCount; }
    	set { if ((value>0)&&(value<9)) this._botsCount = value; }
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
    	set { if ((value>0)&&(value<33)) this._arenaSizeMultiplicator = value; }
	}
	
	// Use this for initialization
	void Start () {
		setState(GamesState.MENU);
		this.menuState = GameObject.Find("MenuState").GetComponent<MenuState>();
		networkManagement = GameObject.Find("NetworkManager").GetComponent<NetworkManagement>();
	}
	
	// Update is called once per frame
	void Update () {
		if(isState(GamesState.WAITING_FOR_PLAYERS)){
			//check if enough players have joined
			if(networkManagement.currentPlayerCount() >= _playersNeededForGame + _botsCount){
				setState(GamesState.GAME_STARTING);
			}
		} else if(isState(GamesState.GAME_STARTING)){
			// once enough there are enough players, start game after some seconds
			if(lastChangedLongerThan(gameStartsInSeconds)){
				setState(GamesState.GAME_RUNNING);
			}
		} else if(isState(GamesState.GAME_ENDED)){
			if(lastChangedLongerThan(gameEndedWait)){
				setState(GamesState.GAME_STARTING);
			}
		}
	}
	
	public void setState(GamesState state){
		currentGameState = state;
		lastChange = Time.fixedTime;
		Debug.Log("GAME STATE: "+currentGameState.ToString());
		if(isState (GamesState.GAME_ENDED)){
			deadPlayers.Clear();
		} else if(isState (GamesState.GAME_STARTING)){
			deadPlayers.Clear();
			menuState.currentMenuState = 0;
			networkManagement.respawnPlayers();
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
			if(score.Count - deadPlayers.Count == 1){
				setState(GamesState.GAME_ENDED);
			}
		}
	}
	
	public Dictionary<string, int> getScores(){
		return new Dictionary<string, int>(this.score);
	}
}
