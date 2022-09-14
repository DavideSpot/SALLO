using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rotator : MonoBehaviour
{
    LineRenderer ray;
    public float speedMultiplier = 0.1f;

    void Start()
    {
        ray = gameObject.AddComponent<LineRenderer>();
        ray.alignment = LineAlignment.View;
        ray.startWidth = 0.5f;
        ray.endWidth = 0.5f;
        ray.SetPosition(0, transform.position);
    }

    private void FixedUpdate()
    {
        this.transform.RotateAround(Vector3.zero, Vector3.up, Input.GetAxis("Horizontal")*speedMultiplier);
        ray.SetPosition(1, transform.TransformDirection(Vector3.forward) * 10f);
    }

}
