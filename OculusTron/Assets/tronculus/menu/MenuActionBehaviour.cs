using UnityEngine;
using System.Collections;
using System;

public class MenuActionBehaviour : MonoBehaviour {

	public enum MenuActionType {STARTGAME, REFRESHHOSTS, INCPLAYER, DECPLAYER, INCBOTS, 
								DECBOTS, INCMAPSIZE, DECMAPSIZE, CONNECTHOST, 
		                        CUSTOMSERVERCHECK, OCULUSVIEWCHECK, WIIMOTECHECK};
	
	
	public MenuActionType menuActionType;
	public GameObject hostPrefab;
	
	private NetworkManagement networkManagement; 
	private GameStateManager gameState;
	private BikeHotkeyController oculusEnabler;
	private GameObject GameList; 
	private ArrayList games;
	
	private TextMesh playercountText;
	private TextMesh botscountText;
	private TextMesh arenasizeText;
	private TextMesh ipText;
	private WiiController wiiController;
	
	
	void Start () {
		networkManagement = GameObject.Find("NetworkManager").GetComponent<NetworkManagement>();
		GameObject gameObj = GameObject.Find("GameState");
		gameState = gameObj.GetComponent<GameStateManager>();
		oculusEnabler = gameObj.GetComponent<BikeHotkeyController>();
		playercountText = GameObject.Find("playercount_Text").GetComponent<TextMesh>();
		botscountText = GameObject.Find("botscount_Text").GetComponent<TextMesh>();
		arenasizeText = GameObject.Find("arenasize_Text").GetComponent<TextMesh>();	
		GameList = GameObject.Find("AvailableGamesList");
		games = new ArrayList();	
		wiiController = GameObject.Find("WIImote").GetComponent<WiiController>();
		ipText = GameObject.Find("customserverip_Edit").GetComponent<TextMesh>();	
	}
	
	void Update () {}
	
	void OnMouseUp() {
		switch(menuActionType){
			case MenuActionType.STARTGAME:
        		networkManagement.StartServer();
        	break;
    		case MenuActionType.REFRESHHOSTS:
				HostData[] hostList = networkManagement.hostList;
				
				foreach (GameObject go in games){
					DestroyImmediate(go);
				}
				
				if (!networkManagement.useCustomMasterServer){
					for (int i=0;i<hostList.Length;i++){
						GameObject obj = GameObject.Instantiate(hostPrefab, new Vector3(0,0,0), Quaternion.identity) as GameObject;
						obj.transform.parent = GameList.transform;
						obj.transform.localPosition = new Vector3(0.0f,i*(-0.2f),0.0f);
						
						GameListElement gle = obj.GetComponent<GameListElement>();
						gle.hostData = hostList[i];
						//gle.contentString = hostList[i].gameName;	
						games.Add(obj);
					}
				}else{
						GameObject obj = GameObject.Instantiate(hostPrefab, new Vector3(0,0,0), Quaternion.identity) as GameObject;
						obj.transform.parent = GameList.transform;
						obj.transform.localPosition = new Vector3(0.0f,(0),0.0f);
						
						GameListElement gle = obj.GetComponent<GameListElement>();
						HostData hostData = new HostData();		
						
						String ip = ipText.text; //"192.186.0.1";
						hostData.ip = ip.Split('.');
						hostData.port = 25000;
						hostData.gameName = "Custom Server game on "+ip;
					
						gle.hostData = hostData;
						//gle.contentString = hostList[i].gameName;	
						games.Add(obj);
				}
				
			break;
			case MenuActionType.INCPLAYER:
				gameState.playersNeededForGame++;
				playercountText.text = Convert.ToString(gameState.playersNeededForGame);
			break;
			case MenuActionType.DECPLAYER:
				gameState.playersNeededForGame--;
				playercountText.text = Convert.ToString(gameState.playersNeededForGame);
			break;
			case MenuActionType.INCBOTS:
				gameState.botsCount++;
				botscountText.text = Convert.ToString(gameState.botsCount);
			break;
			case MenuActionType.DECBOTS:
				gameState.botsCount--;
				botscountText.text = Convert.ToString(gameState.botsCount);
			break;
			case MenuActionType.INCMAPSIZE:
				gameState.arenaSizeMultiplicator*=2;
				arenasizeText.text = Convert.ToString(gameState.arenaSizeMultiplicator);
			break;
			case MenuActionType.DECMAPSIZE:
				gameState.arenaSizeMultiplicator/=2;
				arenasizeText.text = Convert.ToString(gameState.arenaSizeMultiplicator);
			break;
			case MenuActionType.CONNECTHOST:
				networkManagement.JoinServer(gameObject.GetComponent<GameListElement>().hostData);
			break;
			case MenuActionType.CUSTOMSERVERCHECK:
				bool newServerVal = !networkManagement.useCustomMasterServer;
				networkManagement.useCustomMasterServer = newServerVal;
				gameObject.GetComponent<TextMesh>().text = (newServerVal)?"√":"Δ";
			break;
			case MenuActionType.OCULUSVIEWCHECK:
				bool newOculusVal = !oculusEnabler.ovr;
				oculusEnabler.ovr = newOculusVal;
				gameObject.GetComponent<TextMesh>().text = (newOculusVal)?"√":"Δ";
			break;
			case MenuActionType.WIIMOTECHECK:
				bool newWIIMoteVal = !wiiController.wiimoteIsEnabled;
				wiiController.wiimoteIsEnabled= newWIIMoteVal;
				gameObject.GetComponent<TextMesh>().text = (newWIIMoteVal)?"√":"Δ";
			break;
		}
		
	}
	
}
