using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamPlayerFollower : MonoBehaviour
{
    [SerializeField] Transform player;
    Vector3 baseVector;

    void Start() => baseVector = player.position - transform.position;
    void Update() => transform.position = player.position - baseVector;
}
