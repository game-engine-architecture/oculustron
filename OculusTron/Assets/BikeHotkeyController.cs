using UnityEngine;
using System.Collections;

public class BikeHotkeyController : MonoBehaviour {
	
	Component oculusCam = null;
	
	void Start () {
	
	}
	
	void Update () {
		checkHotKey();
	}
	
	void checkHotKey(){
		if(Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.O)){
			
			
			if (oculusCam==null){
				Debug.Log("add ovr controller");
				//oculusCam = this.gameObject.AddComponent("OVRCameraController");
				oculusCam = this.gameObject.AddComponent("OVRPlayerController");
				//Debug.Log("add ovr controller");
			}else{
				Debug.Log("remove ovr controller");
				Destroy(oculusCam);
				oculusCam = null;	
			}
			
			
		}	
	}
}
