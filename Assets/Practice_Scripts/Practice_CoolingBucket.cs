using System;
using UnityEngine;

public class Practice_CoolingBucket : MonoBehaviour
{
    // The "Something Fell In The Bucket" radio channel.
    public static event Action<GameObject> OnObjectEnteredBucket;

    void OnTriggerEnter(Collider other)
    {
        // Announce that an object has entered the trigger.
        OnObjectEnteredBucket?.Invoke(other.gameObject);
    }
}
