using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.IO;
using UnityEngine.UI;
using Unity.RemoteConfig;

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
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private AudioSource _bulletAudio;
    [SerializeField] private ParticleSystem _shotParticle;

    [SerializeField] private Text _killsCounter;

    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _shotDistance;
    [SerializeField] private float _moveBulletTime;

    private SaveData _data;

    private string _JSONFilePath;

    private int _kills;
    private void Awake()
    {
        ConfigManager.FetchCompleted += OnConfigsFetched;
        ConfigManager.FetchConfigs(new UserAttributes(), new AppAttributes());
    }
    private void OnConfigsFetched(ConfigResponse response)
    {
        _speed = ConfigManager.appConfig.GetFloat("Speed");
    }

    private void Start()
    {
        _data = new SaveData();

        _JSONFilePath = Path.Combine(Application.persistentDataPath, "Save.json");
        _data = LoadJSON<SaveData>();

        if (_data != null)
        {
            transform.position = _data.position;
            transform.rotation = _data.rotation;
        }

        _kills = PlayerPrefs.GetInt("Kills");
        _killsCounter.text = "Kills: " + _kills; 
    }

    private void OnApplicationQuit()
    {
        _data = new SaveData();

        _data.position = transform.position;
        _data.rotation = transform.rotation;
        SaveFileJSON(_data);

        PlayerPrefs.SetInt("Kills", _kills);
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

        UpdateAim();
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

        _bulletAudio.Play();

        _shotParticle.transform.rotation = _mainSprite.flipX ? Quaternion.Euler(Vector3.up * 180f) : Quaternion.identity;
        _shotParticle.Play();

        bullet.transform.DOLocalMoveX(bullet.transform.localPosition.x + _shotDistance * (bullet.flipX ? -1f : 1f), _moveBulletTime)
            .OnComplete(() => Destroy(bullet.gameObject));
    }

    private void UpdateAim()
    {
        _lineRenderer.SetPosition(0, transform.position);
        _lineRenderer.SetPosition(1, _mainSprite.flipX ? new Vector2(-10f, transform.position.y) : new Vector2(10f, transform.position.y));
    }

    public void KillEnemy()
    {
        _kills++;
        _killsCounter.text = "Kills: " + _kills;

        UnityAnalytics.Instance.OnPlayerKillEvent(_kills);
          
    }

}


public struct UserAttributes { }
public struct AppAttributes { }
