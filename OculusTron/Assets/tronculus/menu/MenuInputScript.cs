using UnityEngine;
using System.Collections;

public class MenuInputScript : MonoBehaviour {
	

	private MoveDirection moveDirection = MoveDirection.right;
	private int calc=0;
	private int oldGoal = 0;
	
	public float rotation_speed;
	
	private float current = 1f;
	TextMesh ipeditfield;
	NetworkManagement networkmanager;
	
	void Start () {
		networkmanager = GameObject.Find ("NetworkManager").GetComponent<NetworkManagement>();
		ipeditfield = GameObject.Find("customserverip_Edit").GetComponent<TextMesh>();
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
		
		//enter ip
		if(goal == 2){
			if(ipeditfield.text.Length < 15){
				if(Input.GetKeyDown(KeyCode.Alpha0)){
					ipeditfield.text += "0";
					updateMasterIp();
				}
				if(Input.GetKeyDown(KeyCode.Alpha1)){
					ipeditfield.text += "1";
					updateMasterIp();
				}
				if(Input.GetKeyDown(KeyCode.Alpha2)){
					ipeditfield.text += "2";
					updateMasterIp();
				}
				if(Input.GetKeyDown(KeyCode.Alpha3)){
					ipeditfield.text += "3";
					updateMasterIp();
				}
				if(Input.GetKeyDown(KeyCode.Alpha4)){
					ipeditfield.text += "4";
					updateMasterIp();
				}
				if(Input.GetKeyDown(KeyCode.Alpha5)){
					ipeditfield.text += "5";
					updateMasterIp();
				}
				if(Input.GetKeyDown(KeyCode.Alpha6)){
					ipeditfield.text += "6";
					updateMasterIp();
				}
				if(Input.GetKeyDown(KeyCode.Alpha7)){
					ipeditfield.text += "7";
					updateMasterIp();
				}
				if(Input.GetKeyDown(KeyCode.Alpha8)){
					ipeditfield.text += "8";
					updateMasterIp();
				}
				if(Input.GetKeyDown(KeyCode.Alpha9)){
					ipeditfield.text += "9";
					updateMasterIp();
				}
				if(Input.GetKeyDown(KeyCode.Period)){
					ipeditfield.text += ".";
					updateMasterIp();
				}
			}
			if(Input.GetKeyDown(KeyCode.Backspace)){
				if(ipeditfield.text.Length > 0){
					ipeditfield.text = ipeditfield.text.Remove(ipeditfield.text.Length-1);
					updateMasterIp();
				}
			}
		}
		
	}
	
	private void updateMasterIp(){
		networkmanager.customMasterServerIp = ipeditfield.text;
		networkmanager.useCustomMasterServer = true;	
	}
	
	private enum MoveDirection { left, right, up, down };
}
