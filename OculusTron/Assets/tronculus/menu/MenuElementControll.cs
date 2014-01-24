using UnityEngine;
using System.Collections;

public class MenuElementControll : MonoBehaviour {
	
	void OnMouseEnter() {
		renderer.material.color = new Color(0.6f,0.6f,0.6f,0.6f);
 
    }
	
	void OnMouseExit() {
		renderer.material.color = Color.white;
       
    }	
}


