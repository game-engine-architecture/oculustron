using UnityEngine;
using System.Collections;

public class BikeHotkeyController : MonoBehaviour {
	
	public bool ovr = false;
	GameObject[] oculusCams;
	
	void Start () {	
		oculusCams = GameObject.FindGameObjectsWithTag("OculusCam");
	}
	
	void setOVR (bool ovrEnable){
		foreach(GameObject cam in oculusCams){
			cam.SetActive(ovrEnable);
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
