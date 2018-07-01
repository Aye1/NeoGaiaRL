using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailPart : MonoBehaviour {

    private Rail _fullRail;
    public Vector3 direction;

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
            float sign = playerVel.x >= 0.0f ? 1.0f : -1.0f;
            Vector3 newDir = sign * direction.normalized;
            _fullRail.PlayerCollidesRailPart(newDir);
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
