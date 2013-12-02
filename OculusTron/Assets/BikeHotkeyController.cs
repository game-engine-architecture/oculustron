using UnityEngine;
using System.Collections;

public class BikeHotkeyController : MonoBehaviour {
	
	GameObject oculusCam = null;
	GameObject normalCam = null;
	bool ovr = false;
	
	void Start () {		
		oculusCam = GameObject.FindWithTag("Respawn");
		normalCam = GameObject.FindWithTag("MainCamera");
		
		Debug.Log("test");
		setOVR(ovr);
		
		/*if (OVRDevice.SensorCount > 0)
        {
            normalCamera.SetActive(false);
            riftCamera.SetActive(true); // OVRCameraController
        }*/
	}
	
	void setOVR (bool on){
		oculusCam.SetActive(on);
		normalCam.SetActive(!on);
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
