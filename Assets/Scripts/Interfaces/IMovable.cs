using UnityEngine;

public interface IMovable
{
    void GoToPosition(Vector3 pos, float time);
    Vector3 GetPosition();
}