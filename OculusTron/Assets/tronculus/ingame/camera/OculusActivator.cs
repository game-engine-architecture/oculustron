using UnityEngine;
using System.Collections;

public class OculusActivator : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
		BikeHotkeyController oculusEnabler = GameObject.Find("GameState").GetComponent<BikeHotkeyController>();
		if (OVRDevice.SensorCount > 0)
        {
			oculusEnabler.ovr = true;
			GameObject.Find("oculusrift_Check").GetComponent<TextMesh>().text = "√";
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
