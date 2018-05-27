using System.Collections;
using Assets.Scripts.Helpers;
using UnityEngine;

public class Hole : MonoBehaviour {

    public SpriteRenderer inside;
    public SpriteRenderer outside;

    private Player player;
    private Bounds colBounds;
    private bool _isInside = false;

    public bool IsInside
    {
        set
        {
            if (value != _isInside)
            {
                _isInside = !IsInside;
                float targetAlpha = IsInside ? 1.0f : 0.0f;
                // We don't want multiple alpha fades at the same time
                StopAllCoroutines();
                StartCoroutine(SpriteHelper.FadeToAlpha(inside, targetAlpha));
            }
        }
        get
        {
            return _isInside;
        }
    }

    private void Start()
    {
        player = FindObjectOfType<Player>();
        colBounds = GetComponent<EdgeCollider2D>().bounds;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Enter hole trigger");
            IsInside = IsPlayerInside();
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Exit hole trigger");
            IsInside = IsPlayerInside();
        }
    }

    private bool IsPlayerInside()
    {
        Vector3 pos = player.transform.position;
        // Project the player position on the collider plan
        Vector3 posProjection = new Vector3(pos.x, pos.y, colBounds.center.z);
        return colBounds.Contains(posProjection);
    }
}
