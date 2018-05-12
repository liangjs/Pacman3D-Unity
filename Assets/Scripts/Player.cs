using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    public GameObject mixedRealityCamera;
    public Text countText, winText;
    public int count, hit_wall;
    public double hit_wall_time;
    public string status;
    // Use this for initialization
    void Start () {
        count = 0;
        hit_wall = 0;
        SetCountText();
        winText.text = "";
        status = "live";
    }
	
	// Update is called once per frame
	void Update () {
        Transform cameraTransform = mixedRealityCamera.GetComponent<Transform>();
        Vector3 position = cameraTransform.localPosition;
        transform.localPosition = position;

        SetCountText();
	}

    // When this game object intersects a collider with 'is trigger' checked, 
    // store a reference to that collider in a variable named 'other'..
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bean"))
        {
            // Make the other game object (the Bean) inactive, to make it disappear
            other.gameObject.SetActive(false);

            // Add one to the score variable 'count'
            count = count + 1;

            //TODO: add some audio
        }

        if (other.gameObject.CompareTag("Monster"))
        {
            count = 0;
            status = "dead";
            //TODO: add some audio
        }

        if (other.gameObject.CompareTag("Wall"))
        {
            if (status == "live")
            {
                status = "wall";
                hit_wall++;
                hit_wall_time = 0;
                if (hit_wall == 3)
                    status = "dead";
            }
            //TODO: add some audio
        }
        SetCountText();
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            hit_wall_time += Time.deltaTime;
            if(hit_wall_time > 3.0)
            {
                status = "dead";
            }
        }
        SetCountText();
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            if (status == "wall")
                status = "live";
            hit_wall_time = 0;
        }
        SetCountText();
    }
    // Create a standalone function that can update the 'countText' UI and check if the required amount to win has been achieved
    void SetCountText()
    {
        // Update the text field of our 'countText' variable
        countText.text = "Count: " + count.ToString();

        // Check if dead
        if (status == "dead")
        {
            winText.text = "Game Over! Please Restart!";

            //TODO: game over
        }

        else if (status == "wall")
        {
            winText.text = "Caution: You hit the wall " + hit_wall + "times!";

            //TODO: game over
        }
        else if (status == "live")
            winText.text = "";

        // Check if our 'count' is equal to or exceeded 2
        if (count >= 2)
        {
            // Set the text value of our 'winText'
            winText.text = "You Win!";

            //TODO: game over
        }
    }
}
