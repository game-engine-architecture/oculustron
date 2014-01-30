using UnityEngine;
using System.Collections;

public class BikeHotkeyController : MonoBehaviour {
	
	private bool _ovr = false;
	public bool ovr
	{
    	get { return this._ovr; }
    	set { this._ovr = value; 
			 setOVR(ovr);	
		}
	}
		
	GameObject[] oculusCams;
	SmoothFollow mainCameraOculusController;
	void Start () {	
		oculusCams = GameObject.FindGameObjectsWithTag("OculusCam");
		mainCameraOculusController = GameObject.Find("Main Camera").GetComponent<SmoothFollow>();
		setOVR(false);
	}
	
	void setOVR (bool ovrEnable){
		foreach(GameObject cam in oculusCams){
			cam.SetActive(ovrEnable);
		}
		if(ovrEnable){
			mainCameraOculusController.setEgoCam();
		} else {
			mainCameraOculusController.setFollowCam();
		}
	} 
	
	void killAllBots(){
		foreach(GameObject bike in GameObject.FindGameObjectsWithTag("PlayerBike")){
			BikeInputController controller = bike.GetComponent<BikeInputController>();
			if(controller.isAIControlled){
				controller.die();
			}
		}
	}
	
	void leaveGame(){
		GameStateManager gameStateManager = this.GetComponent<GameStateManager>();
		bool inGame = gameStateManager.isState(GameStateManager.GamesState.MENU);
		if(!inGame){
			gameStateManager.leaveGame();
		}
	}
	
	void Update () {
		checkHotKey();
	}
	
	void checkHotKey(){
		if(Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.O)){
			this.ovr = !this.ovr;
		}
		if(Network.isServer){
			if(Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.K)){
				killAllBots();
			}
		}
		if(Input.GetKey(KeyCode.Escape) || Input.GetKey(KeyCode.Q)){
			leaveGame();
		}
	}
}
