using UnityEngine;

public abstract class Character : MonoBehaviour {

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
}
