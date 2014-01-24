using UnityEngine;
using System.Collections;

public class BikeInputController : MonoBehaviour {
	
	public CharacterController characterController;
	Transform modelTransform;
	public float movementSpeed = 1f;
	public float rotationSpeed = 9.4f;
	Vector3 rotation = new Vector3(1,0,0);
	int directionIndex = 0;
	Vector3 lastCorner;
	GameStateManager gameState;

	// Use this for initialization
	void Start () {
		directionIndex = (int)Mathf.Round(this.gameObject.transform.rotation.eulerAngles.y/90f);
		lastCorner = this.gameObject.transform.position;
		if (networkView.isMine) {
			GameObject cam = GameObject.Find("Main Camera");
			if(cam != null){
				SmoothFollow follow = cam.GetComponent<SmoothFollow>();
				follow.target = this.gameObject.transform;
			}
		}
		this.modelTransform = this.GetComponent<Transform>();
		gameState = GameObject.Find("GameState").GetComponent<GameStateManager>();
	}
	
	// Update is called once per frame
	void Update () {
		if (networkView.isMine && gameState.getGameState().Equals(GameStateManager.GamesState.GAME_RUNNING)) {
			float deltaT = Time.deltaTime;
			if (Input.GetKeyDown ("left")){
				//rotation
				rotation = Quaternion.AngleAxis(-90, Vector3.up) * rotation;
				directionIndex += 1;
				directionIndex %= 4;
				lastCorner = this.gameObject.transform.position;
			} else if(Input.GetKeyDown ("right")){
				rotation = Quaternion.AngleAxis(90, Vector3.up) * rotation;
				directionIndex += 3;
				directionIndex %= 4;
				lastCorner = this.gameObject.transform.position;
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
	}
	
	public int getDirectionIndex(){
		return this.directionIndex;
	}
	
	public Vector3 getLastCorner(){
		return this.lastCorner;
	}
}
