using UnityEngine;
using System.Collections;

public class TurnSoundPlayer : MonoBehaviour {
	
	public AudioClip[] turnsounds;
	private AudioSource audioPlayer;
	
	// Use this for initialization
	void Start () {
		this.audioPlayer = this.gameObject.GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void playTurnSound(){
		this.audioPlayer.clip = turnsounds[Random.Range(0, turnsounds.Length)];
		this.audioPlayer.Play();
	}
}
