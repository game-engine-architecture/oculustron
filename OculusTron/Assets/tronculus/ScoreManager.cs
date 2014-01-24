using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScoreManager : MonoBehaviour {
	
	Dictionary<string, int> score = new Dictionary<string, int>();
	List<string> deadPlayers = new List<string>();
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void addPlayer(string playerid){
		score[playerid] = 0;
		GameObject.Find ("ScoreBoardContainer").GetComponent<ScoreBoardUpater>().renderScoreBoard();
	}
	
	public void playerLost(string playerid){
		deadPlayers.Add(playerid);
		foreach (KeyValuePair<string, int> pair in score){
			if(deadPlayers.Contains(pair.Key)){
				//every alive player gets a point
				continue;
			}
			score[pair.Key] += 1;
		}
	}
	
	public void playerWon(string playerid){
		score[playerid] -= 1;
	}
	
	public Dictionary<string, int> getScores(){
		return new Dictionary<string, int>(this.score);
	}
}
