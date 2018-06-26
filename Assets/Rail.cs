using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rail : MonoBehaviour {

    private int collisionCount = 0;
    private PlayerMovement _player;

	// Use this for initialization
	void Start () {
        _player = FindObjectOfType<PlayerMovement>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PlayerCollidesRailPart()
    {
        collisionCount++;
        if (collisionCount == 1)
        {
            _player.StartGrinding();
        }
    }

    public void PlayerExitsRailPart()
    {
        collisionCount--;
        if (collisionCount == 0)
        {
            _player.StopGrinding();
        }
    }
}
