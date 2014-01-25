using UnityEngine;
using System.Collections;

public class AIDodge : MonoBehaviour {
	
	BikeInputController bikeCtrl;
	
	// Use this for initialization
	void Start () {
		bikeCtrl = gameObject.transform.parent.gameObject.GetComponent<BikeInputController>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter(Collider hit) {		
		if(hit.gameObject.name.StartsWith("bike_wall") || 
			hit.gameObject.name.StartsWith("wall") || 
			"Wall".Equals(hit.gameObject.name))
		{
			Debug.Log("wall in front!");
			bikeCtrl.aiObjectInFront = true;
			
		}
	}
}
