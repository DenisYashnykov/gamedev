using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleObj : MonoBehaviour
{

    [SerializeField] private Transform _trasform;
    [SerializeField] private float min=1.0f, max=5.0f;
    [SerializeField] private float _scaleSpeed;
    [SerializeField] private bool _increase=true;
    void Update()
    {
        if (_increase)
        {
            _trasform.localScale += Vector3.one*_scaleSpeed* Time.deltaTime;
            if (_trasform.localScale.x >= max) 
            { 
                _increase = false; 
            }
        }else
            
            {
               _trasform.localScale -= Vector3.one * _scaleSpeed* Time.deltaTime; 
               if(_trasform.localScale.x <= min)
                {
                _increase = true;
                }
                
            }

    }
}
