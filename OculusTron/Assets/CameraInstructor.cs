using UnityEngine;
using System.Collections;

public class CameraInstructor : MonoBehaviour {
	
	public void showScoreBoardLater(){
		Invoke ("showScoreBoard", 2);
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
	
	void cameraFollow(GameObject go){
		GameObject maincam = GameObject.Find("Main Camera");
		SmoothFollow follower = maincam.GetComponent<SmoothFollow>();
		follower.target = go.transform;
	}
}
