﻿using UnityEngine;
using System.Collections;

public class GameStateManager : MonoBehaviour {
	
	public enum GamesState {
		MENU,
		WAITING_FOR_PLAYERS,
		GAME_STARTING,
		GAME_RUNNING,
		GAME_ENDED,
	}
	
	private GamesState currentGameState;
	private float lastChange;
	private NetworkManagement networkManagement;
	public int playersNeededForGame = 1;
	public int botsCount = 1;
	public int gameStartsInSeconds = 5;
	
	
	// Use this for initialization
	void Start () {
		setState(GamesState.MENU);
		networkManagement = GameObject.Find("NetworkManager").GetComponent<NetworkManagement>();
	}
	
	// Update is called once per frame
	void Update () {
		if(isState(GamesState.WAITING_FOR_PLAYERS)){
			//check if enough players have joined
			if(networkManagement.currentPlayerCount() >= playersNeededForGame){
				setState(GamesState.GAME_STARTING);
			}
		} else if(isState(GamesState.GAME_STARTING)){
			// once enough there are enough players, start game after some seconds
			if(Time.fixedTime - lastChange > gameStartsInSeconds){
				setState(GamesState.GAME_RUNNING);
			}
		}
	}
	
	public void setState(GamesState state){
		currentGameState = state;
		lastChange = Time.fixedTime;
	}
	
	public GamesState getGameState(){
		return this.currentGameState;
	}
	
	public float getLastChanged(){
		return lastChange;
	}
	
	public bool isState(GamesState state){
		return currentGameState.Equals(state);
	}
}
