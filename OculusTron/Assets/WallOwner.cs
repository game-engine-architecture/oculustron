using UnityEngine;
using System.Collections;

public class WallOwner : MonoBehaviour {
	
	public string playerid;
	
	// Use this for initialization

	
	// Update is called once per frame
	void Update () {
	
	}
	
	public string getOwner(){
		return playerid;
	}
	
	public void setOwner(string playerid, int playernumber){
		this.playerid = playerid;
	}
}
