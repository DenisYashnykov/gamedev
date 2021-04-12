using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _resTime;
    [SerializeField] private PlayerController _player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Bullet")
        {
            _player.KillEnemy();

            gameObject.SetActive(false);
            DOVirtual.DelayedCall(_resTime, () =>
            {
                gameObject.SetActive(true);
            });
        }
    }
}
