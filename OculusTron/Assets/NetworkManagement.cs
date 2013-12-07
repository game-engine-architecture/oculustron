using UnityEngine;
using System.Collections;

public class NetworkManagement : MonoBehaviour {
 
	private MenuState menuState;
	private const string typeName = "Tronculus";
	private const string gameName = "DeathMatch";
	public GameObject playerPrefab;
	
	// Use this for initialization
	void Start () {
		this.menuState = GameObject.Find("MenuState").GetComponent<MenuState>();
	}
	
	//Server
	private void StartServer() {
	    Network.InitializeServer(4, 25000, !Network.HavePublicAddress());
	    MasterServer.RegisterHost(typeName, gameName);
	}
	
	private int top = 100;
	private int start_refresh_left = 250;
	private int start_refresh_width = 150;
	private int start_refresh_height = 100;
	private int padding = 10;
	
	private int game_room_height = 50;
	
	void OnGUI() {
		if(menuState.currentState == 1){ //play menu
		    if (GUI.Button(new Rect(start_refresh_left, top, start_refresh_width, start_refresh_height), "Start Server")){
	            StartServer();
				menuState.currentState = 0;			
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
		}
	}
	
	void OnServerInitialized() {
		Debug.Log("Server Initialized");
	    SpawnPlayer();
	}
	 
	void OnConnectedToServer() {
		Debug.Log("Server Joined");
	    SpawnPlayer();
	}
	 
	private void SpawnPlayer() {
		GameObject spawnpoints = GameObject.Find("Spawnpoints");
		Transform spawnpoint = spawnpoints.transform.GetChild(Network.connections.Length);
	    Network.Instantiate(playerPrefab, spawnpoint.position, spawnpoint.rotation, 0);
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
