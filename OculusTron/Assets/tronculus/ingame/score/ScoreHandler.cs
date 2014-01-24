using UnityEngine;
using System.Collections;

public class ScoreHandler : MonoBehaviour {
	
	public int score = 0;
	GameStateManager gameState;
	TextMesh text;
	
	// Use this for initialization
	void Start () {
		renderScore();
	}
	
	// Update is called once per frame
	void Update () {
		renderScore();
	}
	
	public void lost(){
		this.score--;
		renderScore();
	}
	
	public void won(){
		this.score++;
		renderScore();
	}
	
	void renderScore(){
		if(gameState == null || text == null){
			gameState = GameObject.Find("GameState").GetComponent<GameStateManager>();
			text = GameObject.Find("ScoreText").GetComponent<TextMesh>();	
		}
		if(gameState.getGameState().Equals(GameStateManager.GamesState.GAME_RUNNING)){
			text.text = "Score: "+score;			
		} else {
			if(text.text.Length > 0){
				text.text = "";	
			}
		}
	}
}
