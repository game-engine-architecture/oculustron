using UnityEngine;
using System.Collections;
using System;

public class MenuActionBehaviour : MonoBehaviour {

	public enum MenuActionType {STARTGAME, REFRESHHOSTS, INCPLAYER, DECPLAYER, INCBOTS, 
								DECBOTS, INCMAPSIZE, DECMAPSIZE, CONNECTHOST, 
		                        CUSTOMSERVERCHECK, OCULUSVIEWCHECK};
	
	
	public MenuActionType menuActionType;
	public GameObject hostPrefab;
	
	private NetworkManagement networkManagement; 
	private GameStateManager gameState;
	private GameObject GameList; 
	private ArrayList games;
	
	private TextMesh playercountText;
	private TextMesh botscountText;
	private TextMesh arenasizeText;
	
	
	void Start () {
		networkManagement = GameObject.Find("NetworkManager").GetComponent<NetworkManagement>();
		gameState = GameObject.Find("GameState").GetComponent<GameStateManager>();
		playercountText = GameObject.Find("playercount_Text").GetComponent<TextMesh>();
		botscountText = GameObject.Find("botscount_Text").GetComponent<TextMesh>();
		arenasizeText = GameObject.Find("arenasize_Text").GetComponent<TextMesh>();	
		GameList = GameObject.Find("AvailableGamesList");
		games = new ArrayList();
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
				for (int i=0;i<hostList.Length;i++){
					GameObject obj = GameObject.Instantiate(hostPrefab, new Vector3(0,0,0), Quaternion.identity) as GameObject;
					obj.transform.parent = GameList.transform;
					obj.transform.localPosition = new Vector3(0.0f,i*(-0.2f),0.0f);
					
					GameListElement gle = obj.GetComponent<GameListElement>();
					gle.hostData = hostList[i];
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
		}
		
	}
	
}
