using UnityEngine;
using System.Collections;

public class BikeCollision : MonoBehaviour {
	
	public GameObject explosionPrefab;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnControllerColliderHit(ControllerColliderHit hit) {		
		//if collision with own wall or collision with outer wall, we lose.
		if(hit.gameObject.name.StartsWith("bike_wall") || 
			hit.gameObject.name.StartsWith("wall") || 
			"Wall".Equals(hit.gameObject.name))
		{
			die();	
			networkView.RPC("playerLost", RPCMode.AllBuffered, Network.player.ToString());
			networkView.RPC("createExplosion", RPCMode.All, this.gameObject.transform.position);
			Invoke ("showScoreBoard", 2);
		}
	}
	
	[RPC]
	void playerLost(string playerid){
		GameObject.Find ("ScoreManager").GetComponent<ScoreManager>().playerLost(playerid);
		if(!Network.player.ToString().Equals(playerid)){
			//update the score board instantly for other players
			updateScoreBoard();
		}
	}
	
	[RPC]
	void createExplosion(Vector3 pos){
		Network.Instantiate(explosionPrefab, pos, Quaternion.identity, 0);
	}
	
	void cameraFollow(GameObject go){
		GameObject maincam = GameObject.Find("Main Camera");
		SmoothFollow follower = maincam.GetComponent<SmoothFollow>();
		follower.target = go.transform;
	}

	void die(){
		this.gameObject.GetComponent<CharacterController>().enabled = false;
		this.gameObject.GetComponent<BikeInputController>().enabled = false;
		this.gameObject.GetComponent<BikeCollision>().enabled = false;
		this.gameObject.GetComponent<ParticleSystem>().loop = false; 
	}
	
	void showScoreBoard(){
		cameraFollow(GameObject.Find ("ScoreBoardContainer"));
		//update score board after board was shown
		Invoke ("updateScoreBoard", 1);
		Invoke ("showPlayfield", 3);
	}
	
	void showPlayfield(){
		cameraFollow(GameObject.Find ("LevelOverviewPoint"));
	}
	
	void updateScoreBoard(){
		ScoreBoardUpater updater = GameObject.Find ("ScoreBoardContainer").GetComponent<ScoreBoardUpater>();
		updater.renderScoreBoard();
	}
}
