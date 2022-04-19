using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private int health = 150;
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float padding = 0.5f;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileSpeed = 10f;
    [SerializeField] private float projectileFiringPeriod = 0.1f;
    [SerializeField] private AudioClip deathSFX;
    [SerializeField] [Range(0, 1)] private float deathSFXVolume = 0.7f; // range 0-1
    [SerializeField] private AudioClip shootSound;
    [SerializeField] [Range(0, 1)] private float shootSoundVolume = 0.2f; // range 0-1
    private Coroutine _firingCoroutine;
    private float _xMin;
    private float _xMax;
    private float _yMin;
    private float _yMax;

    private void Start()
    {
        SetUpMoveBoundaries();
    }

    private void Update()
    {
        Move();
        Fire();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if (!damageDealer) return;
        ProcessHit(damageDealer);
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        health -= damageDealer.GetDamage();
        damageDealer.Hit();
        if (health <= 0) Die();

    }

    private void Die()
    {
        FindObjectOfType<Level>().LoadGameOver();
        Destroy(gameObject);
        if (Camera.main != null) AudioSource.PlayClipAtPoint(deathSFX, Camera.main.transform.position, deathSFXVolume);
    }

    public int GetHealth()
    {
        return health;
    }

    private void SetUpMoveBoundaries()
    {
        var gameCamera = Camera.main;
        if (gameCamera == null) return;
        _xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding;
        _xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;
        _yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding;
        _yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - padding;
    }

    private void Fire()
    {
        if (Input.GetButtonDown("Fire1"))
            _firingCoroutine = StartCoroutine(FireContinously());
        if (Input.GetButtonUp("Fire1"))
            StopCoroutine(_firingCoroutine);
    }

    private IEnumerator FireContinously()
    {
        while (true)
        {
            var t = transform;
            var projectile =
                Instantiate(projectilePrefab, t.position, t.rotation);
            projectile.GetComponent<Rigidbody2D>().velocity =
                new Vector2(0, projectileSpeed);
            if (Camera.main != null) AudioSource.PlayClipAtPoint(shootSound, Camera.main.transform.position, shootSoundVolume);

            yield return new WaitForSeconds(projectileFiringPeriod);
        }
    }

    private void Move()
    {
        var pos = transform.position;
        var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;
        var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
        var newXPos = Mathf.Clamp(pos.x + deltaX, _xMin, _xMax);
        var newYPos = Mathf.Clamp(pos.y + deltaY, _yMin, _yMax);
        transform.position = new Vector2(newXPos, newYPos);
    }
}