using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FolowOject : MonoBehaviour
{
    [SerializeField]
    private Transform ToFollow;
    void Update()
    {
        transform.position = ToFollow.position;
        transform.rotation = ToFollow.rotation;
    }
}
