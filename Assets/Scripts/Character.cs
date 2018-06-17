using UnityEngine;

using System.Collections;

public abstract class Character : MonoBehaviour, IMovable {

    public Sprite picture;
    public string characterName;
    public bool canMove = true;

    public void Freeze()
    {
        canMove = false;
    }

    public void EndFreeze()
    {
        canMove = true;
    }

    public void GoToPosition(Vector3 pos, float time)
    {
        StartCoroutine(SmoothMoveToPosition(pos, time));
    }

    public IEnumerator SmoothMoveToPosition(Vector3 pos, float time)
    {
        Vector3 currentPos = transform.position;
        float t = 0.0f;
        while (t < 1)
        {
            t += Time.deltaTime / time;
            transform.position = Vector3.Lerp(currentPos, pos, t);
            yield return null;
        }
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }
}
