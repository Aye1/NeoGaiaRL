using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B4KLog : Character {

    public bool isFlying = false;
    public Sprite flyingSprite;
    public Sprite basicSprite;
    private SpriteRenderer _renderer;

	// Use this for initialization
	void Start () {
        _renderer = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        UpdateSprite();
	}

    private void UpdateSprite()
    {
        Sprite currentSprite = isFlying ? flyingSprite : basicSprite;
        _renderer.sprite = currentSprite;
    }
}

