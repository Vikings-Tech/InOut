using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeRotator : MonoBehaviour
{
    public float RotationSpeed = 3f;
    public float range = 5f;
    public Vector3 baseScale;

    private void Start()
    {
        baseScale = new Vector3( range, 0.01f, range);
    }
    private void Update()
    {
        transform.Rotate(transform.up, RotationSpeed * Time.deltaTime);
        transform.localScale = Random.Range(0.99f,1.01f) * baseScale;
    }
}
