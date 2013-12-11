using UnityEngine;
using System.Collections;

public class CountDownActivator : MonoBehaviour {
	
	GameStateManager gameState;
	TextMesh countDownText;
	// Use this for initialization
	void Start () {
		gameState = GameObject.Find("GameState").GetComponent<GameStateManager>();
		countDownText = GameObject.Find("CountDownText").GetComponent<TextMesh>();
	}
	
	// Update is called once per frame
	void Update () {
		if(gameState.isState(GameStateManager.GamesState.GAME_STARTING)){
			countDownText.text = ""+(int)(gameState.gameStartsInSeconds-(Time.fixedTime - gameState.getLastChanged())+1);
		} else {
			if(countDownText.text.Length > 0){
				countDownText.text = "";
			}
		}
	}
}
