using UnityEngine;
using System.Collections;

public class MenuInputScript : MonoBehaviour {
	

	private MoveDirection moveDirection = MoveDirection.right;
	private int calc=0;
	private int oldGoal = 0;
	
	public float rotation_speed;
	
	private float current = 1f;
	
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		GameObject stateGO = GameObject.Find("MenuState");
		MenuState state = stateGO.GetComponent<MenuState>();
		int goal = state.currentMenuState;
		
		if(oldGoal!=goal){
			if((goal==0&&oldGoal==1)||(goal==1&&oldGoal==0)){
				moveDirection = MoveDirection.right;
			}else if((goal==0&&oldGoal==2)||(goal==2&&oldGoal==0)){
				moveDirection = MoveDirection.left;
			}else if((goal==0&&oldGoal==3)||(goal==3&&oldGoal==0)){
				moveDirection = MoveDirection.down;
			}
			calc = (goal==0)?0:1;
			oldGoal = goal;
		}	
		
		current -= ((current-calc)*Time.deltaTime)*rotation_speed;
		
		switch (moveDirection){
		case MoveDirection.right:
			this.gameObject.transform.eulerAngles = new Vector3(0, current*90, 0);	
			break;
		case MoveDirection.left:
			this.gameObject.transform.eulerAngles = new Vector3(0, -(current*90), 0);	
			break;
		case MoveDirection.down:
			this.gameObject.transform.eulerAngles = new Vector3(current*90, 0, 0);	
			break;
		case MoveDirection.up:
			this.gameObject.transform.eulerAngles = new Vector3(-(current*90), 0, 0);	
			break;
		}
		
	}
	
	private enum MoveDirection { left, right, up, down };
}
