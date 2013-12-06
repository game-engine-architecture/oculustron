using UnityEngine;
using System.Collections;

public class MenuInputScript : MonoBehaviour {
	
	GameObject menuCube = null;
	Transform menuCubeTransform = null;
	
	void Start () {
		menuCube = GameObject.FindWithTag("MenuCube");
		menuCubeTransform = menuCube.transform;
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.LookAt(menuCubeTransform);
    	transform.Translate(Vector3.right * Time.deltaTime);
		
		//if (Input.GetKeyDown ("left")){
		//	Debug.Log("left");
		//	this.transform.Rotate(0,10,0);
		//}
	}
}
