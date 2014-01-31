using UnityEngine;
using System.Collections;

public class FlyAround : MonoBehaviour {
	
	public float x, y, z;
	public float radius;
	public float speed;
	Vector3 origpos;
	// Use this for initialization
	void Start () {
		origpos = this.gameObject.transform.position;
	}
	
	// Update is called once per frame
	void Update () {		
		this.gameObject.transform.position = origpos + Quaternion.Euler(speed*Time.time*x, speed*Time.time*y, 0) * Vector3.forward * radius;
	}
}
