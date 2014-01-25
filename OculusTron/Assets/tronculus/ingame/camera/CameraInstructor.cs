using UnityEngine;
using System.Collections;

public class CameraInstructor : MonoBehaviour {
	
	public void showScoreBoardLater(){
		Invoke ("showScoreBoard", 1);
	}
	
	void showScoreBoard(){
		cameraFollow(GameObject.Find ("ScoreBoardContainer"));
		//update score board after board was shown
		Invoke ("updateScoreBoard", 1);
		Invoke ("showPlayfield", 2);
	}
	
	void showPlayfield(){
		cameraFollow(GameObject.Find ("LevelOverviewPoint"));
	}
	
	void updateScoreBoard(){
		GameObject.Find ("ScoreBoardContainer").GetComponent<ScoreBoardUpater>().renderScoreBoard();
	}
	
	void cameraFollow(GameObject go){
		GameObject maincam = GameObject.Find("Main Camera");
		SmoothFollow follower = maincam.GetComponent<SmoothFollow>();
		follower.target = go.transform;
	}
}
