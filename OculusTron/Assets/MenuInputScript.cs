using UnityEngine;
using System.Collections;

public class MenuInputScript : MonoBehaviour {
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown ("left")){
			this.transform.Rotate(0,10,0);
		}
	}
}
