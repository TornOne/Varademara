using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "game/AudioClipGroup")] 
public class AudioClipGroup : ScriptableObject {
	[Range(0,2)]
	public float VolumeMin = 1f;
	[Range(0,2)]
	public float VolumeMax = 1f;
	[Range(0,2)]
	public float PitchMin = 1f;
	[Range(0,2)]
	public float PitchMax = 1f;
	public float Cooldown = 0.1f;
	public AudioClip[] AudioClips;

	private float timestamp;
	private AudioSourcePool audioSourcePool;

	private void OnEnable(){
		audioSourcePool = FindObjectOfType<AudioSourcePool>();
		timestamp = -Cooldown;
	}
	public void Play(){
		if (audioSourcePool == null) audioSourcePool = FindObjectOfType<AudioSourcePool>();

		AudioSource source = audioSourcePool.GetSource();
		source.transform.position = Vector3.zero;
		source.spatialBlend = 0;
		Play(source);
	}
	public void Play(AudioSource audioSource){
		if (AudioClips.Length == 0) return;

		//TODO implement cooldown
		if (Time.time < timestamp+Cooldown) return;
		timestamp = Time.time;

		audioSource.clip = AudioClips[Random.Range(0,AudioClips.Length)];
		audioSource.volume = Random.Range(VolumeMin,VolumeMax);
		audioSource.pitch = Random.Range(PitchMin,PitchMax);
		audioSource.Play();
	}

	public void Play(Vector3 location){
		if (audioSourcePool == null) audioSourcePool = FindObjectOfType<AudioSourcePool>();

		AudioSource source = audioSourcePool.GetSource();
		source.transform.position = location;
		source.spatialBlend = 1;
		Play(source);
	}

}
