using UnityEngine;
using System.Collections;

public class BikeInputController : MonoBehaviour {
	
	public CharacterController characterController;
	Transform modelTransform;
	public float movementSpeed = 1;
	public float rotationSpeed = 130;
	Vector3 rotation = new Vector3(1,0,0);
	
	// Use this for initialization
	void Start () {
		this.modelTransform = this.GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
		float deltaT = Time.deltaTime;
		float hori = Input.GetAxis("Horizontal");
		if (Input.GetKeyDown ("left")){
			//rotation
			rotation = Quaternion.AngleAxis(-90, Vector3.up) * rotation;
		} else if(Input.GetKeyDown ("right")){
			rotation = Quaternion.AngleAxis(90, Vector3.up) * rotation;
			//movementDirection.Rotate(new Vector3(0, 90, 0));	
		}
		float rotadiff = Vector3.Dot(rotation.normalized, this.modelTransform.right.normalized);
		rotadiff *= deltaT * rotationSpeed;
		this.modelTransform.rotation *= Quaternion.AngleAxis(rotadiff * 180, Vector3.up);
		
		//rotationSpeed*hori
		float vert = Input.GetAxis("Vertical");
		Vector3 movement = new Vector3(0,0,0);
		movement += rotation * vert * movementSpeed;
		//deltaT*movementSpeed
		characterController.Move(movement);
	}
}
