using UnityEngine;
using System.Collections;
using System;

public class MenuActionBehaviour : MonoBehaviour {

	public enum MenuActionType {STARTGAME, REFRESHHOSTS, INCPLAYER, DECPLAYER, INCBOTS, DECBOTS, INCMAPSIZE, DECMAPSIZE,};
	
	public MenuActionType menuActionType;
	
	private NetworkManagement networkManagement; 
	private GameStateManager gameState;
	private TextMesh playercountText;
	private TextMesh botscountText;
	private TextMesh arenasizeText;
	
	void Start () {
		networkManagement = GameObject.Find("NetworkManager").GetComponent<NetworkManagement>();
		gameState = GameObject.Find("GameState").GetComponent<GameStateManager>();
		playercountText = GameObject.Find("playercount_Text").GetComponent<TextMesh>();
		botscountText = GameObject.Find("botscount_Text").GetComponent<TextMesh>();
		
		arenasizeText = GameObject.Find("arenasize_Text").GetComponent<TextMesh>();		
	}
	
	void Update () {}
	
	void OnMouseUp() {
		switch(menuActionType){
			case MenuActionType.STARTGAME:
        		networkManagement.StartServer();
        	break;
    		case MenuActionType.REFRESHHOSTS:
				networkManagement.RefreshHostList();
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
				//gameState.playersNeededForGame++;
				//playercountText.text = Convert.ToString(gameState.playersNeededForGame);
			break;
			case MenuActionType.DECBOTS:
				//gameState.playersNeededForGame--;
				//playercountText.text = Convert.ToString(gameState.playersNeededForGame);
			break;
			case MenuActionType.INCMAPSIZE:
				gameState.arenaSizeMultiplicator*=2;
				arenasizeText.text = Convert.ToString(gameState.arenaSizeMultiplicator);
			break;
			case MenuActionType.DECMAPSIZE:
				gameState.arenaSizeMultiplicator/=2;
				arenasizeText.text = Convert.ToString(gameState.arenaSizeMultiplicator);
			break;
		}
		
	}
	
}
