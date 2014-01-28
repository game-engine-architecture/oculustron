using UnityEngine;
using System.Collections;

public class MusicController : MonoBehaviour {
	
	GameStateManager gameState;
	AudioSource gameMusic;
	
	void Start () {
		this.gameState = gameState = GameObject.Find("GameState").GetComponent<GameStateManager>();
		this.gameMusic = this.GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		if(gameState.isState(GameStateManager.GamesState.MENU)){
			if(!this.gameMusic.isPlaying){
				this.gameMusic.volume = 1.0f;
				this.gameMusic.Play();	
			}
		} else {
			if(this.gameMusic.isPlaying){
				// make it quiet
				this.gameMusic.volume -= 0.5f * Time.deltaTime;
				if(this.gameMusic.volume < 0.01f){
					this.gameMusic.Stop();
				}
			}
		}
	}
}
