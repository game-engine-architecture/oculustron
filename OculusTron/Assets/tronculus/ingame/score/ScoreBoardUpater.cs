using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScoreBoardUpater : MonoBehaviour {
	
	GameStateManager manager;
	TextMesh scoreBoardText;
	
	// Use this for initialization
	void Start () {
		manager = GameObject.Find("GameState").GetComponent<GameStateManager>();
		scoreBoardText = GameObject.Find("ScoreBoardContent").GetComponent<TextMesh>();
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	public void renderScoreBoard(){
		string scoretext = "";
		foreach (KeyValuePair<string, int> pair in manager.getScores()){
			scoretext += "program "+pair.Key+": "+pair.Value+"\n";
		}
		scoreBoardText.text = scoretext;
	}
}
