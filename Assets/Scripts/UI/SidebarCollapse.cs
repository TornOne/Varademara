using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SidebarCollapse : MonoBehaviour {
	Camera cam;
	public RectTransform panelTransform;
	public Text buttonText;
	bool animating = false;
	bool isOpen = true;
	public float duration;
	public float panelClosedX, panelOpenX;
	public float cameraFullX, cameraNarrowX;

	void Start() {
		cam = Camera.main;
	}

	public void TogglePanel() {
		if (animating) {
			return;
		}
		if (isOpen) {
			StartCoroutine(AnimatePanel(panelOpenX, panelClosedX));
			StartCoroutine(AnimateCamera(cameraNarrowX, cameraFullX));
			buttonText.text = "<";
		} else {
			StartCoroutine(AnimatePanel(panelClosedX, panelOpenX));
			StartCoroutine(AnimateCamera(cameraFullX, cameraNarrowX));
			buttonText.text = ">";
		}
	}

	IEnumerator AnimatePanel(float startValue, float endValue) {
		animating = true;
		isOpen = !isOpen;
		float startTime = Time.time;
		float endTime = startTime + duration;
		float changeValue = endValue - startValue;

		while (Time.time < endTime) {
			float x = (Time.time - startTime) / duration;
			float x2 = x * x;
			panelTransform.anchoredPosition = new Vector3(startValue + (3 * x2 - 2 * x2 * x) * changeValue, 0);
			yield return null;
		}
		panelTransform.anchoredPosition = new Vector3(endValue, 0);
		animating = false;
	}

	IEnumerator AnimateCamera(float startValue, float endValue) {
		float startTime = Time.time;
		float endTime = startTime + duration;
		float changeValue = endValue - startValue;

		while (Time.time < endTime) {
			float x = (Time.time - startTime) / duration;
			float x2 = x * x;
			cam.rect = new Rect(startValue + (3 * x2 - 2 * x2 * x) * changeValue, 0, 1, 1);
			yield return null;
		}
		cam.rect = new Rect(endValue, 0, 1, 1);
	}
}
