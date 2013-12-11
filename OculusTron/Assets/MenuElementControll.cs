using UnityEngine;
using System.Collections;

public class MenuElementControll : MonoBehaviour {
	
	public int thisMenuState;
	
	void OnMouseEnter() {
		renderer.material.color = new Color(0.6f,0.6f,0.6f,0.6f);
 
    }
	
	void OnMouseExit() {
		renderer.material.color = Color.white;
       
    }
	
	void OnMouseUp(){
		//Application.Quit();	
		 //Debug.Log(" click");
		
		//Application.LoadLevel(1);
		GameObject stateGO = GameObject.Find("MenuState");
		MenuState state = stateGO.GetComponent<MenuState>();
		state.currentMenuState = thisMenuState;
	}
		
}


