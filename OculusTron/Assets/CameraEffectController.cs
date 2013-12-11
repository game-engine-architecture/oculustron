using UnityEngine;
using System.Collections;

public class CameraEffectController : MonoBehaviour {
	
	public float menuGlowIntensity;
	public float ingameGlowIntensity;
	public float glowTransitionTime;
	
	private GlowEffect glowEffect;
	
	private GameStateManager gameState;
	// Use this for initialization
	void Start () {
		gameState = GameObject.Find("GameState").GetComponent<GameStateManager>();
		glowEffect = this.gameObject.GetComponent<GlowEffect>();
	}
	
	// Update is called once per frame
	void Update () {
		if(gameState.getGameState().Equals(GameStateManager.GamesState.MENU)){
			glowEffect.glowIntensity += (menuGlowIntensity-glowEffect.glowIntensity)*Time.deltaTime*glowTransitionTime;
		} else if(gameState.getGameState().Equals(GameStateManager.GamesState.WAITING_FOR_PLAYERS)){
			glowEffect.glowIntensity += (ingameGlowIntensity-glowEffect.glowIntensity)*Time.deltaTime*glowTransitionTime;
		}
	}
}
