using Assets.Scripts.Helpers;
using UnityEngine;

public class ExitHole : MonoBehaviour {

    public SpriteRenderer inside;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            StartCoroutine(SpriteHelper.FadeToAlpha(inside, 0.0f));
        }
    }
}
