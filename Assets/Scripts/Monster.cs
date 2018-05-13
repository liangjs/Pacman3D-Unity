﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour {

    // Use this for initialization
    public float speed;
    private Vector3 dir, player_location, now_location;
    private Player player;

    void Start () {
        //speed = 0.5f;
        GameObject playerObj;
        playerObj = GameObject.Find("Player");
        player = playerObj.GetComponent<Player>();

    }
	
    void Rotate_Face()
    {
        transform.localRotation = Quaternion.LookRotation(dir);
        transform.Rotate(0.0f, -97.5f, 0.0f);
        // transform.localEulerAngles.Set(transform.localEulerAngles.x, transform.localEulerAngles.y - 150.0f, transform.localEulerAngles.z);
    }

	// Update is called once per frame
	void Update () {
        // Check if Monster can see Player, if so, change direction to player
        now_location = transform.localPosition;
        player_location = player.transform.localPosition;
        Ray ray_to_player = new Ray(now_location, player_location - now_location);
        RaycastHit hit_info = new RaycastHit();
        int layer_mark = (1 << 9) | (1 << 10); // only hit wall or player
        bool hasCollider = Physics.Raycast(ray_to_player, out hit_info, 1e9f, layer_mark);
        //dir = player_location - now_location;
        if (hasCollider && hit_info.collider.CompareTag("Player"))
            dir = player_location - now_location;

        Rotate_Face();

        if (hasCollider && hit_info.collider.CompareTag("Player")) {
            Rigidbody monsterRigidbody = GetComponent<Rigidbody>();
            // Vector3 velocity = monsterRigidbody.velocity;
            Vector3 normalizedDir = Vector3.Normalize(dir);
            Vector3 velocity = normalizedDir * speed;
            monsterRigidbody.velocity = velocity;
        }
    }

    void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("Wall")) {
            Rigidbody monsterRigidbody = GetComponent<Rigidbody>();
            Vector3 inVelocity = monsterRigidbody.velocity;
            Vector3 collisionNormal = other.contacts[0].normal;
            Vector3 outVelocity = Vector3.Reflect(inVelocity, collisionNormal);
            monsterRigidbody.velocity = outVelocity;
        }
    }
}
