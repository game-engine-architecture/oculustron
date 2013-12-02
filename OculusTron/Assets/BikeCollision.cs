using UnityEngine;
using System.Collections;

public class BikeCollision : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnControllerColliderHit(ControllerColliderHit hit) {
	   	GameObject go = GameObject.Find("Explosion");
		go.transform.position = this.gameObject.transform.position;
		go.GetComponent<ExplosionAnimation>().reset().explode();
		this.gameObject.SetActive(false);
	}
}
