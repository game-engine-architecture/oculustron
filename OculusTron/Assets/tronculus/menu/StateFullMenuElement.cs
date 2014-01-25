using UnityEngine;
using System.Collections;

public class StateFullMenuElement : MonoBehaviour {

	public int thisMenuState;
	private MenuState state;
	
	
	void Start () {
		state = GameObject.Find("MenuState").GetComponent<MenuState>();
	}
	
	void Update () {}
	
	void OnMouseUp(){
		
		if(thisMenuState==4){
			Application.Quit();
			return;
		}

		state.currentMenuState = thisMenuState;
	}
}
