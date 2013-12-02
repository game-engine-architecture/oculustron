using UnityEngine;
using System.Collections;

public class ExplosionAnimation : MonoBehaviour {
	
	float explodingSince = 0;
	bool isExploding = false;
	float explosionSpeed = 0.5f;
	Color origColor;
	Color endColor = new Color(50,50,50,50);
	Light lightobj;
	// Use this for initialization
	void Start () {
		this.lightobj = this.gameObject.GetComponent<Light>();
		this.origColor = this.lightobj.color;
	}
	
	// Update is called once per frame
	void Update () {
		if(isExploding){
			this.lightobj.range = explodingSince * 10f;
			this.lightobj.intensity = explodingSince * 8f;
			explodingSince += Time.deltaTime * explosionSpeed;
			if(explodingSince > 0.5f){
				this.lightobj.color = Color.Lerp(this.lightobj.color, this.endColor, explodingSince);
			}
		}
	}
	
	public ExplosionAnimation explode(){
		this.isExploding = true;
		return this;
	}
	
	public ExplosionAnimation reset(){
		lightobj.color = origColor;
		this.isExploding = false;
		this.explodingSince = 0;
		return this;
	}
}
