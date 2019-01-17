using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
	public Vector3 destination;
	public float speed;

	void Start() {
		StartCoroutine(ApproachDestination());
	}

	IEnumerator ApproachDestination() {
		Vector3 source = transform.position;
		float startTime = Time.time;
		float travelTime = Vector3.Distance(source, destination) / speed;
		while (Time.time < startTime + travelTime) {
			transform.position = Vector3.Lerp(source, destination, (Time.time - startTime) / travelTime);
			yield return null;
		}
		transform.position = destination;
	}
}
