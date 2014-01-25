using UnityEngine;
using System.Collections;

public class GameListElement : MonoBehaviour {
	
	private HostData _hostData;
	public HostData hostData
	{
    	get { return this._hostData;}
		set { this._hostData = value; 
			  updateFameNameText("  "); }
	}
	
	private TextMesh text;
	
	void Start () {
		text = gameObject.GetComponent<TextMesh>();
		updateFameNameText("  ");
	}
	
	private void updateFameNameText(string pretex){
		if ((text!=null)&&(_hostData!=null)) text.text = pretex+_hostData.gameName;
	}
	
	void Update () {}
	
	void OnMouseEnter() {
		updateFameNameText("<  ");
    }
	
	void OnMouseExit() {
		updateFameNameText("  ");	
    }	
}
