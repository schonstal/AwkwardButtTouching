using UnityEngine;
using System.Collections;

public class WebCam : MonoBehaviour {
	public int cameraNumber = 0;

	private WebCamTexture webcamTexture;
  private double scaleY;

	void Start () {
		WebCamDevice[] devices = WebCamTexture.devices;
		if (devices.Length > cameraNumber) {
			webcamTexture = new WebCamTexture(devices[cameraNumber].name, 640, 480, 30);
			renderer.material.mainTexture = webcamTexture;
			webcamTexture.Play();
      Debug.Log("Successfully added camera " + cameraNumber);
		} else {
			Debug.Log("no camera");
		}
	}
}
