using UnityEngine;
using System.Collections;

public class MenuInputScript : MonoBehaviour {
	

	public float rotation_speed;
	
	private float current = 1f;
	
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		GameObject stateGO = GameObject.Find("MenuState");
		MenuState state = stateGO.GetComponent<MenuState>();
		float goal = state.currentState;
		current -= ((current-goal)*Time.deltaTime)*rotation_speed;
		this.gameObject.transform.eulerAngles = new Vector3(0, current*90, 0);

		
		//if (Input.GetKeyDown ("left")){
		//	Debug.Log("left");
		//	this.transform.Rotate(0,10,0);
		//}
	}
}
