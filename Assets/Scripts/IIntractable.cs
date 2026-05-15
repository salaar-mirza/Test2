using UnityEngine;

public interface IIntractable 
{
    void OnGrab(Transform interactor);
    void OnRelease (Vector3 releaseVelocity);
}
