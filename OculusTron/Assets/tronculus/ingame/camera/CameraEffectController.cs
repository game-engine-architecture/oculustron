using UnityEngine;
using System.Collections;

public class CameraEffectController : MonoBehaviour {
	
	public float menuGlowIntensity;
	public float menuGlowPulse;
	public int menuGlowIterations;
	public float ingameGlowIntensity;
	public int ingameGlowIterations;
	public float glowTransitionTime;
	public float iterationChangeSec = 1f;
	
	private GlowEffect glowEffect;
	private float lastIterationChange = Time.time;
	
	private GameStateManager gameState;
	// Use this for initialization
	void Start () {
		gameState = GameObject.Find("GameState").GetComponent<GameStateManager>();
		glowEffect = this.gameObject.GetComponent<GlowEffect>();
	}
	
	// Update is called once per frame
	void Update () {
		if(gameState.getGameState().Equals(GameStateManager.GamesState.MENU)){
			float goalIntensity = menuGlowIntensity + (menuGlowPulse + Mathf.Sin(Time.time)*menuGlowPulse);
			glowEffect.glowIntensity += (goalIntensity-glowEffect.glowIntensity)*Time.deltaTime*glowTransitionTime;
			if(lastIterationChange+iterationChangeSec > Time.time){
				if(glowEffect.blurIterations != menuGlowIterations){
					glowEffect.blurIterations += glowEffect.blurIterations-menuGlowIterations>0? -1: 1;
				}
				lastIterationChange = Time.time;
			}
		} else if(gameState.getGameState().Equals(GameStateManager.GamesState.WAITING_FOR_PLAYERS)){
			glowEffect.glowIntensity += (ingameGlowIntensity-glowEffect.glowIntensity)*Time.deltaTime*glowTransitionTime;
			if(lastIterationChange+iterationChangeSec > Time.time){
				lastIterationChange = Time.time;
				if(glowEffect.blurIterations != ingameGlowIterations){
					glowEffect.blurIterations += glowEffect.blurIterations-ingameGlowIterations>0? -1: 1;
				}
			}
		}
	}
}
