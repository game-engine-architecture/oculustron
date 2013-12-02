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
		
		//if collision with own wall or collision with outer wall, we lose.
		if(hit.gameObject.name.StartsWith(this.gameObject.name) || "Wall".Equals(hit.gameObject.name)){
			GameObject.Find("ScoreBoard").GetComponent<ScoreHandler>().lost();
		}
	}
}
