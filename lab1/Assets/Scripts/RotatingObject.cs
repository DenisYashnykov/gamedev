using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingObject : MonoBehaviour
{
    [SerializeField] private Transform _transformObj;
    [SerializeField] private float _rotationSpeed;

    private void Update()
    {
        _transformObj.rotation *= Quaternion.Euler(Vector3.one * _rotationSpeed * Time.deltaTime);
    }
}
