using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bean : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter(Collision other) {
        
    }

    void OnTriggerEnter(Collider other) {
        //if collid player, it will be disabled by Player.cs
    }       

}
