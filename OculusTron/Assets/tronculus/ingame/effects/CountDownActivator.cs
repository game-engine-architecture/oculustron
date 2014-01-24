using UnityEngine;
using System.Collections;

public class CountDownActivator : MonoBehaviour {
	
	GameStateManager gameState;
	TextMesh countDownText;
	GameObject countDownTextGo;
	int lastsecond = -1;
	Animation countDownTextAni;
	// Use this for initialization
	void Start () {
		gameState = GameObject.Find("GameState").GetComponent<GameStateManager>();
		countDownTextGo = GameObject.Find("CountDownText");
		countDownText = countDownTextGo.GetComponent<TextMesh>();
		countDownTextAni = countDownTextGo.GetComponent<Animation>();
	}
	
	// Update is called once per frame
	void Update () {
		if(gameState.isState(GameStateManager.GamesState.WAITING_FOR_PLAYERS)){
			countDownText.text = "Waiting for other players...";		
			countDownText.transform.localPosition = new Vector3(0,100,15);
		} else if(gameState.isState(GameStateManager.GamesState.GAME_STARTING)){
			int currsec = (int)(gameState.gameStartsInSeconds-(Time.fixedTime - gameState.getLastChanged())+1);
			if(lastsecond != currsec){
				lastsecond = currsec;
				countDownTextAni.Stop();
				countDownTextAni.Play("CountDownAnimation");
			}
			countDownText.text = ""+currsec;
		} else {
			if(countDownText.text.Length > 0){
				countDownText.text = "";
			}
		}
	}
}
