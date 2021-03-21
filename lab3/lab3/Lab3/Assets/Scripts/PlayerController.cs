﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.IO;

public class SaveData
{
    public Vector3 position;
    public Quaternion rotation;
}

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private SpriteRenderer _bullet;
    [SerializeField] private Animator _anemator;
    [SerializeField] private SpriteRenderer _mainSprite;

    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _shotDistance;
    [SerializeField] private float _moveBulletTime;

    private SaveData _data;

    private string _JSONFilePath;

    private void Start()
    {
        _data = new SaveData();

        _JSONFilePath = Path.Combine(Application.dataPath, "Save.json");
        _data = LoadJSON<SaveData>();

        if (_data != null)
        {
            transform.position = _data.position;
            transform.rotation = _data.rotation;
        }
    }

    private void OnApplicationQuit()
    {
        _data.position = transform.position;
        _data.rotation = transform.rotation;
        SaveFileJSON(_data);
    }

    private void SaveFileJSON(object obj)
    {
        File.WriteAllText(_JSONFilePath, JsonUtility.ToJson(obj));
    }

    private T LoadJSON<T>()
    {
        if (File.Exists(_JSONFilePath))
            return JsonUtility.FromJson<T>(File.ReadAllText(_JSONFilePath));
        else
            return default(T);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.D))
            Move(1);
        else if (Input.GetKey(KeyCode.A))
            Move(-1);
        else
            _anemator.SetInteger("State", 0);

        if (Input.GetKeyDown(KeyCode.Space) && CheckGround())
            Jump();

        if (Input.GetMouseButtonUp(0))
            Shot();
    }

    private bool CheckGround()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1f);

        return colliders.Length > 1;
    }

    private void Move(int direction)
    {
        transform.position += Vector3.right * direction * _speed * Time.deltaTime;
        _anemator.SetInteger("State", 1);

        _mainSprite.flipX = direction < 0f;
    }

    private void Jump()
    {
        _rigidbody2D.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
    }

    private void Shot()
    {
        var bullet = Instantiate(_bullet, transform.position, transform.rotation);
        bullet.flipX = _mainSprite.flipX;

        bullet.transform.DOLocalMoveX(bullet.transform.localPosition.x + _shotDistance * (bullet.flipX ? -1f : 1f), _moveBulletTime)
            .OnComplete(() => Destroy(bullet.gameObject));
    }
}
