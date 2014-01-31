using UnityEngine;
using System.Collections;

public class SkyRotation : MonoBehaviour {
	
	public float rotationSpeed;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		this.transform.Rotate(new Vector3(0, Time.deltaTime*rotationSpeed, 0));
	}
}
