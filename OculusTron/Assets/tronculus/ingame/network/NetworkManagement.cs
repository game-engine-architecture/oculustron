﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
//using UnityEditor;

public class Player{
	
	static int uid = 0;
	public readonly string name;
	public readonly bool isBot;
	public readonly int id;
	
	public Player(string name, bool isBot){
		this.name = name;
		this.isBot = isBot;
		this.id = Player.uid++;
	}
}

public class NetworkManagement : MonoBehaviour {
 
	private MenuState menuState;
	private GameStateManager gameState;
	private const string typeName = "Tronculus";
	private const string gameName = "DeathMatch";
	public GameObject playerPrefab;
	private Player thisPlayer; //myself
	private List<Player> players;
	
	public int currentPlayerCount(){
		return players.Count;
	}
	
	// Use this for initialization
	void Start () {
		this.menuState = GameObject.Find("MenuState").GetComponent<MenuState>();
		this.gameState = GameObject.Find("GameState").GetComponent<GameStateManager>();
		players = new List<Player>();
		
		RefreshHostList();
		InvokeRepeating("RefreshHostList", 2.0f, 5.0f);
	}
	
	//Server
	public void StartServer() {
	    Network.InitializeServer(4, 25000, !Network.HavePublicAddress());
	    MasterServer.RegisterHost(typeName, "  DeathMatch - "+gameState.playersNeededForGame+" Player - Arena "+gameState.arenaSizeMultiplicator);
	}
	
	private int top = 100;
	//private int start_refresh_left = 250;
	private int start_refresh_width = 150;
	private int start_refresh_height = 100;
	private int padding = 10;
	private int game_room_height = 50;
	
	//setup
	private string playerCountEditStr = "2";
	
	void OnServerInitialized() {
		Debug.Log("Server Initialized");
		initializeGame();
	}
	 
	void OnConnectedToServer() {
		Debug.Log("Server Joined");
		initializeGame();
	}
	
	public void initializeGame(){
		Debug.Log("Initializing Game");
		gameState.setState(GameStateManager.GamesState.WAITING_FOR_PLAYERS);
		if(Network.isServer){
			// only the server controls the bots
			for(int i=0; i<gameState.botsCount; i++){
				string botName = "bot-"+i;
				networkView.RPC("addPlayerToGame", RPCMode.AllBuffered, botName, true);
				Debug.Log("Creating Bot: "+botName);
			}
		}
		string playerName = "player-"+Network.player.ToString();
		//create player on all other machines
		networkView.RPC("addPlayerToGame", RPCMode.OthersBuffered, playerName, false);
		//add player locally
		thisPlayer = addPlayerToGame(playerName, false);
		Debug.Log("Creating Local Player: "+playerName);
		Debug.Log("Spawning Players... ");
	}
	
	public void respawnPlayers(){
		Debug.Log("Respawning all players");
		if(Network.isServer){
			// only the server controls the bots
			for(int i=0; i<players.Count; i++){
				if(players[i].isBot){
					SpawnPlayer(players[i]);
				}
			}
		}
		//instantiate player on current connection & tell everybody else that i'm there.
		GameObject thisPlayersBike = SpawnPlayer(thisPlayer);
		GameObject cam = GameObject.Find("Main Camera");
		if(cam != null){
			SmoothFollow follow = cam.GetComponent<SmoothFollow>();
			follow.target = thisPlayersBike.transform;
		}
	}
	
	[RPC]
	Player addPlayerToGame(string playerid, bool isBot){
		gameState.addPlayer(playerid);	
		Player player = new Player(playerid, isBot);
    	players.Add(player);
		return player;
		Debug.Log("players in game:"+players.Count);
	}	
	 
	private GameObject SpawnPlayer(Player player){
		string belongsToPlayer = player.name;
		bool isAIControlled = player.isBot;
			
		GameObject spawnpoints = GameObject.Find("Spawnpoints");
		Transform spawnpoint = spawnpoints.transform.GetChild(player.id);
	    GameObject bike = Network.Instantiate(playerPrefab, spawnpoint.position, spawnpoint.rotation, 0) as GameObject;
		BikeInputController bikeCtrl = bike.GetComponent<BikeInputController>();
		bikeCtrl.isAIControlled = isAIControlled;
		bikeCtrl.belongsToPlayer = belongsToPlayer;
		return bike;
	}
	
	//Clients	
	private HostData[] _hostList;
	public HostData[] hostList
	{
    	get { return this._hostList; }
	}
	
	private void RefreshHostList() {
	    MasterServer.RequestHostList(typeName);
		//EditorApplication.Beep();
	}
	 
	void OnMasterServerEvent(MasterServerEvent msEvent) {
	    if (msEvent == MasterServerEvent.HostListReceived) {
			_hostList = MasterServer.PollHostList();
		}
	}
	
	public void JoinServer(HostData hostData) {
	    Network.Connect(hostData);
	}
}
