using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody2D;

    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForce;

    private bool _isGrounded = false;

    private void Update()
    {
        float h = Input.GetAxis("Horizontal");

        transform.position += Vector3.right * h * _speed * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded)
        {
            _rigidbody2D.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
            _isGrounded = false;
        }
    }

    private void OnCollisionStay2D(Collision2D collider)
    {
        _isGrounded = true;
    }
}
