using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourcePool : MonoBehaviour {


	private List<AudioSource> audioSources;

	private void Awake(){
		audioSources = new List<AudioSource>();
	}

	public AudioSource GetSource(){
		foreach (AudioSource source in audioSources){
			if(!source.isPlaying){
				return source;
			}
		}

		GameObject audioObject = new GameObject("AudioSource");
		AudioSource source2 = audioObject.AddComponent<AudioSource>();
		audioSources.Add(source2);

		return source2;
	}
}
