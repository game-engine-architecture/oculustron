using UnityEngine;
using System.Collections;

public class BikeHotkeyController : MonoBehaviour {
	
	public bool ovr = false;
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
	
	void Update () {
		checkHotKey();
	}
	
	void checkHotKey(){
		if(Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.O)){
			ovr = !ovr;
			setOVR(ovr);
		}	
	}
}
