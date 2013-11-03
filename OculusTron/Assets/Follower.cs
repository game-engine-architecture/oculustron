using UnityEngine;
using System.Collections;

public class Follower : MonoBehaviour {
	
	public Transform followedTransform;
	public Transform cameraTransform;
	Transform trans;
	public float smoothness = 0.9f;
	public float rotationSmoothness = 2f;
	// Use this for initialization
	void Start () {
		trans = this.GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
		float deltaT = Time.deltaTime;
		cameraTransform.rotation.SetFromToRotation(trans.position, followedTransform.forward);
		//Vector3 rot = (cameraTransform.rotation.eulerAngles - followedTransform.rotation.eulerAngles) * rotationSmoothness * deltaT;
		Vector3 rot = followedTransform.rotation.eulerAngles;
		//Quaternion.
		//Vector3 camoffset = trans.position;
		//cameraTransform.rotation = Quaternion.Euler(camoffset * rot);
		//cameraTransform.Rotate(rot);
		
		Vector3 newcamvec = (trans.position + followedTransform.position - cameraTransform.position) * smoothness * deltaT;
		cameraTransform.position += newcamvec;
		//trans.position
		
		
		
		
		
	}
}
