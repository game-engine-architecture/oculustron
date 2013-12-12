using UnityEngine;
using System.Collections;

public class ExplosionAnimation : MonoBehaviour {
	
	float explodingSince = 0;
	bool isExploding = false;
	ParticleSystem particles;
	float explosionSpeed = 2f;
	Color origColor;
	Color endColor = new Color(50,50,50,0);
	Light lightobj;
	// Use this for initialization
	void Start () {
		this.lightobj = this.gameObject.GetComponent<Light>();
		this.origColor = this.lightobj.color;
		this.lightobj.enabled = false;
		particles = GameObject.Find("ExplosionParticles").GetComponent<ParticleSystem>();
		explode();
	}
	
	// Update is called once per frame
	void Update () {
		if(isExploding){
			explodingSince += Time.deltaTime * explosionSpeed;
			this.lightobj.range = explodingSince * 10f;
			if(this.explodingSince < 1.0f){
				this.lightobj.intensity = explodingSince * 8f;
			} else {
				this.lightobj.intensity = 1.0f - explodingSince * 8f;
			}
			if(explodingSince > 0.5f){
				this.lightobj.color = Color.Lerp(this.lightobj.color, this.endColor, explodingSince-0.5f);
			}
			if(this.explodingSince > 2.0f){
				this.reset();
			}
		} else {
			this.lightobj.enabled = false;
		}
	}
	
	public ExplosionAnimation explode(){
		particles.Play();	
		this.lightobj.enabled = true;
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
