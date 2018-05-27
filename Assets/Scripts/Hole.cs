using System.Collections;
using Assets.Scripts.Helpers;
using UnityEngine;

public class Hole : MonoBehaviour {

    public SpriteRenderer inside;
    public SpriteRenderer outside;

	// Use this for initialization
	void Start () { 
	}
	
	// Update is called once per frame
	void Update () {
	}

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(SpriteHelper.FateToAlpha(inside,1.0f));
        }
    }
}
