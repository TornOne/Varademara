using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TempBack : MonoBehaviour {
	void Awake() {
		StartCoroutine(SwitchIn(5));
	}

	IEnumerator SwitchIn(float seconds) {
		yield return new WaitForSeconds(seconds);
		SceneManager.LoadScene(0);
	}
}
