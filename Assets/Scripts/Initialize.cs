using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initialize : MonoBehaviour {

    public GameObject wallPrefab;

	// Use this for initialization
	void Start () {
        GameObject wall = Instantiate(wallPrefab);
	}
	
	// Update is called once per frame
	void Update () {
        
	}
}
