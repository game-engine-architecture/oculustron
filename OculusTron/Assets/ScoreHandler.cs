using UnityEngine;
using System.Collections;

public class ScoreHandler : MonoBehaviour {
	
	public int score = 0;
	
	// Use this for initialization
	void Start () {
		renderScore();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void lost(){
		this.score--;
		renderScore();
	}
	
	public void won(){
		this.score++;
		renderScore();
	}
	
	void renderScore(){
		TextMesh text = GameObject.Find("ScoreText").GetComponent<TextMesh>();
		text.text = "Score: "+score;
	}
}
