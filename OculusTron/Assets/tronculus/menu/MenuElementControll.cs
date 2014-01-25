using UnityEngine;
using System.Collections;

public class MenuElementControll : MonoBehaviour {
	
	public bool justGrey; 
	
	void Start () {
		if(justGrey){
			renderer.material.color = new Color(0.7f,0.7f,0.7f,0.7f);
		}	
	}
	
	void OnMouseEnter() {
		if(!justGrey){
			renderer.material.color = new Color(0.6f,0.6f,0.6f,0.6f);
		}
    }
	
	void OnMouseExit() {
		if(!justGrey){
			renderer.material.color = Color.white;
		}
    }	
}