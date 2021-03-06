﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rail : MonoBehaviour {

    private int collisionCount = 0;
    private PlayerMovement _player;
    public float referenceHeight;
    private List<Vector3> _directions;

	// Use this for initialization
	void Start () {
        _player = FindObjectOfType<PlayerMovement>();
        _directions = new List<Vector3>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PlayerCollidesRailPart(Vector3 direction)
    {
        collisionCount++;
        _directions.Add(direction);
        if (collisionCount == 1)
        {
            _player.StartGrinding(transform.position.y + 0.5f);
            _player.ChangeGrindingDirection(direction);
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
