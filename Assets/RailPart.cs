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
            _fullRail.PlayerCollidesRailPart();
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
