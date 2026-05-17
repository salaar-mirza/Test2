using UnityEngine;

public class TestInteractable : MonoBehaviour, IInteractable
{

    public void OnInteract(Transform interactor)
    {
        Debug.Log($"I, {gameObject.name}, was interacted with!");
        GetComponent<MeshRenderer>().material.color = Color.cyan;
    }


}
