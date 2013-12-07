using UnityEngine;
using System.Collections;

public class GameStateManager : MonoBehaviour {
	
	enum GamesState {
		MENU,
		WAITING_FOR_PLAYERS,
		GAME_RUNNING,
		GAME_ENDED,
	}
	
	private GamesState currentGameState;
	
	
	// Use this for initialization
	void Start () {
		currentGameState = GamesState.MENU;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
