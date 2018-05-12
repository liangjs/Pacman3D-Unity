using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    public GameObject mixedRealityCamera;
    public Text countText, winText;
    public int count;
    // Use this for initialization
    void Start () {
        count = 0;
        SetCountText();
        winText.text = "";

    }
	
	// Update is called once per frame
	void Update () {
        Transform cameraTransform = mixedRealityCamera.GetComponent<Transform>();
        Vector3 position = cameraTransform.localPosition;
        transform.localPosition = position;
	}

    // When this game object intersects a collider with 'is trigger' checked, 
    // store a reference to that collider in a variable named 'other'..
    void OnTriggerEnter(Collider other)
    {
        // ..and if the game object we intersect has the tag 'Pick Up' assigned to it..
        if (other.gameObject.CompareTag("Bean"))
        {
            // Make the other game object (the pick up) inactive, to make it disappear
            other.gameObject.SetActive(false);

            // Add one to the score variable 'count'
            count = count + 1;

            // Run the 'SetCountText()' function (see below)
            SetCountText();
        }
        if (other.gameObject.CompareTag("Monster"))
        {
            // Run the 'SetCountText()' function (see below)
            count = 0;
            SetCountText("dead");
        }
    }

    // Create a standalone function that can update the 'countText' UI and check if the required amount to win has been achieved
    void SetCountText(string status = "live")
    {
        // Update the text field of our 'countText' variable
        countText.text = "Count: " + count.ToString();

        // Check if dead
        if (status == "dead")
        {
            winText.text = "Game Over! Please Restart!";
        }
        // Check if our 'count' is equal to or exceeded 2
        if (count >= 2)
        {
            // Set the text value of our 'winText'
            winText.text = "You Win!";
        }
    }
}
