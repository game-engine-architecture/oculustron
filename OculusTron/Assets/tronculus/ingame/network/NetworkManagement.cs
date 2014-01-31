using UnityEngine;
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
	
	public static void reset(){
		Player.uid = 0;
	}
}

public class NetworkManagement : MonoBehaviour {
 
	private GameStateManager gameState;
	private const string typeName = "Tronculus";
	private const string gameName = "DeathMatch";
	
	public GameObject playerPrefab;
	private Player thisPlayer; //myself
	private List<Player> players;
	
	public string customMasterServerIp = "192.168.1.1";
	
	private bool _useCustomMasterServer;
	public bool useCustomMasterServer
	{
    	get { return this._useCustomMasterServer; }
    	set { this._useCustomMasterServer = value; 
			 /* MasterServer.ipAddress = (value) ? customMasterServerIp: "72.52.207.14";
			  Network.natFacilitatorIP = MasterServer.ipAddress;
			  Network.natFacilitatorPort = MasterServer.port;*/
		}
	}
	
	public int currentPlayerCount(){
		return players.Count;
	}
	
	public Player getLocalPlayer(){
		return thisPlayer;
	}
	
	// Use this for initialization
	void Start () {
		this.gameState = GameObject.Find("GameState").GetComponent<GameStateManager>();
		players = new List<Player>();
		
		_useCustomMasterServer = false;
		
		RefreshHostList();
		InvokeRepeating("RefreshHostList", 2.0f, 5.0f);
	}
	
	//Server
	public void StartServer() {
	    Network.InitializeServer(8, 25000, !Network.HavePublicAddress());
		MasterServer.RegisterHost(typeName, "  DeathMatch - "+gameState.playersNeededForGame+" Player - Arena "+gameState.arenaSizeMultiplicator);
		gameState.setArenaSizeForClients(gameState.arenaSizeMultiplicator);
	}
	
	void OnServerInitialized() {
		Debug.Log("Server Initialized");
		initializeGame();
	}
	 
	void OnConnectedToServer() {
		Debug.Log("players already on server: "+players.Count);
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
			cam.GetComponent<CameraInstructor>().cameraFollow(thisPlayersBike);
		}
	}
	
	[RPC]
	Player addPlayerToGame(string playerid, bool isBot){
		gameState.addPlayer(playerid);	
		Player player = new Player(playerid, isBot);
    	players.Add(player);
		Debug.Log("players in game:"+players.Count);
		return player;
	}	
	
	public void leaveGame(){
		Network.Disconnect();
		players.Clear();
	}
	
	private GameObject SpawnPlayer(Player player){
		string belongsToPlayer = player.name;
		bool isAIControlled = player.isBot;
			
		GameObject spawnpoints = GameObject.Find("Spawnpoints");
		Transform spawnpoint;
		if(isAIControlled){
			//bots are continously numbered
			spawnpoint = spawnpoints.transform.GetChild(player.id%8);
		} else {
			spawnpoint = spawnpoints.transform.GetChild((int.Parse(Network.player.ToString())+gameState.botsCount+gameState.getGameRound())%8);
		}
	    GameObject bike = Network.Instantiate(playerPrefab, spawnpoint.position, spawnpoint.rotation, 0) as GameObject;
		BikeInputController bikeCtrl = bike.GetComponent<BikeInputController>();
		bikeCtrl.setPlayerNumber(player.id);
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
		Debug.Log(String.Join(".", hostData.ip)+":"+hostData.port);
		Network.Connect(String.Join(".", hostData.ip), hostData.port);
		
		//Network.Connect("127.0.0.1", 25000);
		//Network.Connect(hostData);
	}
}
