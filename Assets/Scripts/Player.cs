using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public GameObject mixedRealityCamera;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Transform cameraTransform = mixedRealityCamera.GetComponent<Transform>();
        Vector3 position = cameraTransform.localPosition;
        transform.localPosition = position;
	}
}
