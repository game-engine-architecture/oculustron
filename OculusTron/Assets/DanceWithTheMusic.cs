using UnityEngine;
using System.Collections;

public class DanceWithTheMusic : MonoBehaviour {

	Vector3 origpos;
	public float BPM;
	public float intensity;
	
	void Start () {
		origpos = this.gameObject.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		float movement = Mathf.Sin(Mathf.PI*(Time.time/60.0f)*BPM);
		movement = movement * movement * movement;
		this.gameObject.transform.position = origpos + Vector3.forward * movement * intensity;
	}
}
