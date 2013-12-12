﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class NetworkManagement : MonoBehaviour {
 
	private MenuState menuState;
	private GameStateManager gameState;
	private const string typeName = "Tronculus";
	private const string gameName = "DeathMatch";
	public GameObject playerPrefab;
	private Dictionary<string, GameObject> players;
	
	public int currentPlayerCount(){
		return players.Count;
	}
	
	// Use this for initialization
	void Start () {
		this.menuState = GameObject.Find("MenuState").GetComponent<MenuState>();
		this.gameState = GameObject.Find("GameState").GetComponent<GameStateManager>();
		players = new Dictionary<string, GameObject>();
	}
	
	//Server
	private void StartServer() {
	    Network.InitializeServer(4, 25000, !Network.HavePublicAddress());
	    MasterServer.RegisterHost(typeName, gameName);
	}
	
	private int top = 100;
	//private int start_refresh_left = 250;
	private int start_refresh_width = 150;
	private int start_refresh_height = 100;
	private int padding = 10;
	private int game_room_height = 50;
	
	//setup
	private string playerCountEditStr = "2";
	
	void OnGUI() {
		
		
		int screenWidth = Screen.width;
		int screenHeight = Screen.height;
		int start_refresh_left = screenWidth/2-(screenHeight/3)-20;
		
		if(menuState.currentMenuState == 1){ //play menu
		    if (GUI.Button(new Rect(start_refresh_left, top, start_refresh_width, start_refresh_height), "Start Server")){
	            StartServer();
			}
	 
	        if (GUI.Button(new Rect(start_refresh_left, top+start_refresh_height+padding, start_refresh_width, start_refresh_height), "Refresh Hosts")){
				RefreshHostList();
			}
	 
	        if (hostList != null)
	        {
	            for (int i = 0; i < hostList.Length; i++)
	            {
	                
					
					if (GUI.Button(new Rect(start_refresh_left+start_refresh_width+padding, top + ((game_room_height+padding) * i), 300, game_room_height), hostList[i].gameName)){
						JoinServer(hostList[i]);
					}
	            }
	        }
		}else if(menuState.currentMenuState == 2){ //setup menu
			GUI.Label (new Rect (start_refresh_left, top-20,100,20), "Player Count:");
			string tempCount = GUI.TextField (new Rect (start_refresh_left, top, 200, 20), playerCountEditStr, 25);
			
			int numValue;
			bool parsed = Int32.TryParse(tempCount, out numValue);

			if (parsed){
				playerCountEditStr = tempCount;
				gameState.playersNeededForGame = numValue;
				
			}
			
			//gameState.playersNeededForGame = Convert.ToInt32(playerCountEditStr);
		}
	}
	
	void OnServerInitialized() {
		Debug.Log("Server Initialized");
		initializeGame();
	}
	 
	void OnConnectedToServer() {
		Debug.Log("Server Joined");
		initializeGame();
	}
	
	void initializeGame(){
		gameState.setState(GameStateManager.GamesState.WAITING_FOR_PLAYERS);
		menuState.currentMenuState = 0;
	    players.Add(Network.player.ToString(), SpawnPlayer());
	}
	 
	private GameObject SpawnPlayer() {
		GameObject spawnpoints = GameObject.Find("Spawnpoints");
		Transform spawnpoint = spawnpoints.transform.GetChild(Network.connections.Length);
	    GameObject bike = Network.Instantiate(playerPrefab, spawnpoint.position, spawnpoint.rotation, 0) as GameObject;
		return bike;
	}
	
	//Client
	
	private HostData[] hostList;
 
	private void RefreshHostList() {
	    MasterServer.RequestHostList(typeName);
	}
	 
	void OnMasterServerEvent(MasterServerEvent msEvent) {
	    if (msEvent == MasterServerEvent.HostListReceived) {
			hostList = MasterServer.PollHostList();
		}
	}
	
	private void JoinServer(HostData hostData) {
	    Network.Connect(hostData);
	}
}
