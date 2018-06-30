using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailPart : MonoBehaviour {

    private Rail _fullRail;

	// Use this for initialization
	void Start () {
        _fullRail = GetComponentInParent<Rail>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerBottom"))
        {
            Vector3 playerVel = collision.attachedRigidbody.velocity;
            float newX = playerVel.x == 0.0f ? 1.0f : playerVel.x;
            playerVel = new Vector3(newX, 0.0f, 0.0f);
            _fullRail.PlayerCollidesRailPart(playerVel);
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerBottom"))
        {
           _fullRail.PlayerExitsRailPart();
        }
    }
}
