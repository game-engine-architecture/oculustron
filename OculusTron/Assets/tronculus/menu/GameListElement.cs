using UnityEngine;
using System.Collections;

public class GameListElement : MonoBehaviour {
	
	public string contentString;
	private TextMesh text;
	
	
	void Start () {
		text = gameObject.GetComponent<TextMesh>();
		contentString = "Open game - 4 Player - 32 ";
	}
	
	void Update () {}
	
	void OnMouseEnter() {
		text.text = "< "+contentString;
    }
	
	void OnMouseExit() {
		text.text = "  "+contentString;
		
    }	
}
