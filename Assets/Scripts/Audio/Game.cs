using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour {

	public AudioClipGroup walkAudio;
	public AudioClipGroup deathAudio;
	public AudioClipGroup explodeAudio;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Alpha1)){
			walkAudio.Play();
		}
		if (Input.GetKeyDown(KeyCode.Alpha2)){
			deathAudio.Play();
		}
		if (Input.GetKeyDown(KeyCode.Alpha3)){
			explodeAudio.Play();
		}
		//walkAudio.Play();//lol
	}
}
