using UnityEngine;
using System.Collections;

public class BikeInputController : MonoBehaviour {
	
	public CharacterController characterController;
	Transform modelTransform;
	public float movementSpeed = 1;
	public float rotationSpeed = 130;
	
	// Use this for initialization
	void Start () {
		this.modelTransform = this.GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {
		float deltaT = Time.deltaTime;
		float hori = Input.GetAxis("Horizontal");
		if (Input.GetKeyDown ("left")){
			modelTransform.Rotate(new Vector3(0, -90, 0));	
		} else if(Input.GetKeyDown ("right")){
			modelTransform.Rotate(new Vector3(0, 90, 0));	
		}
		
		
		//rotationSpeed*hori
		float vert = Input.GetAxis("Vertical");
		Vector3 movement = new Vector3(0,0,0);
		movement += this.modelTransform.forward * vert * movementSpeed;
		//deltaT*movementSpeed
		characterController.Move(movement);
	}
}
