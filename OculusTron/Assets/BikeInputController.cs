using UnityEngine;
using System.Collections;

public class BikeInputController : MonoBehaviour {
	
	public CharacterController characterController;
	Transform modelTransform;
	public float movementSpeed = 1f;
	public float rotationSpeed = 9.4f;
	Vector3 rotation = new Vector3(1,0,0);
	int directionIndex = 0;

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
			directionIndex += 1;
			directionIndex %= 4;
			GameObject.Find("CameraLeft").transform.Rotate(Vector3.left);
			GameObject.Find("CameraRight").transform.Rotate(Vector3.left);
		} else if(Input.GetKeyDown ("right")){
			rotation = Quaternion.AngleAxis(90, Vector3.up) * rotation;
			directionIndex += 3;
			directionIndex %= 4;
			GameObject.Find("CameraLeft").transform.Rotate(Vector3.right);
			GameObject.Find("CameraRight").transform.Rotate(Vector3.right);
		}
		
		float rotadiff = Vector3.Dot(rotation.normalized, this.modelTransform.right.normalized);
		rotadiff *= deltaT * rotationSpeed;
		this.modelTransform.rotation *= Quaternion.AngleAxis(rotadiff * 180, Vector3.up);
		Vector3 pos = this.modelTransform.position;
		pos.y = 0;
		this.modelTransform.position = pos;
		
		//rotationSpeed*hori
		float vert = Input.GetAxis("Vertical");
		Vector3 movement = new Vector3(0,0,0);
		movement += rotation * vert * movementSpeed;
		//deltaT*movementSpeed
		characterController.Move(movement);
	}
	
	public int getDirectionIndex(){
		return this.directionIndex;
	}
	
	void OnCollisionEnter(Collision collision) {
		Debug.Log("COLLISION!");
        foreach (ContactPoint contact in collision.contacts) {
            Debug.DrawRay(contact.point, contact.normal, Color.white);
        }
    }
}
