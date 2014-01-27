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
	
	void Update () {
		checkHotKey();
	}
	
	void checkHotKey(){
		if(Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.O)){
			this.ovr = !this.ovr;
		}	
	}
}
