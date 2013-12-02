using UnityEngine;
using System.Collections;

public class BikeCollision : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnTriggerEnter(Collider other){
	   	GameObject go = GameObject.Find("Explosion");
		go.transform.position = this.gameObject.transform.position;
		go.GetComponent<ExplosionAnimation>().reset().explode();
	}
}
