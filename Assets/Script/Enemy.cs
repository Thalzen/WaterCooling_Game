using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject bulletprefab;
    [SerializeField] private GameObject cannon;
    private GameObject target;
    [SerializeField] private float speed;
    private Player _player;
    private bool IsAlreadyFiring;
    private Animator _animator;
    private bool isDestroyed;
    [SerializeField] private AppDatas _appDatas;
    [SerializeField] private AudioSource _audio;
    [SerializeField] private AudioClip lasersound;
    [SerializeField] private AudioClip destructsound;

    public delegate void EnemyEvent();

    public static event EnemyEvent EnemyKilled;
    

    private void Start()
    {
        _audio = GetComponent<AudioSource>();
        _player = FindObjectOfType<Player>();
        _animator = GetComponent<Animator>();
        target = GameObject.Find("Player");
    }

    private void Update()
    {
        var step = speed * Time.deltaTime;
        if (isDestroyed == false)
        { 
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x,4f), step);

        }
        
        Fire();
    }

    private void FixedUpdate()
    {
        gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        if (gameObject.transform.position.y >=4f)
        {
            gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Bullet") && isDestroyed == false)
        {
            isDestroyed = true;
            gameObject.GetComponent<Collider2D>().enabled = false;
            StartCoroutine(Destruction());
            
        }
        
    }

    private void Fire()
    {
        
        RaycastHit2D hit = Physics2D.Raycast(cannon.transform.position, -Vector2.up);
        if (hit.collider.gameObject.CompareTag("Player") && _player.IsUnderWater == false == IsAlreadyFiring == false && isDestroyed == false)
        {
            IsAlreadyFiring = true;
            StartCoroutine(OpenFire());
        }

    }

    private IEnumerator OpenFire()
    {
        Instantiate(bulletprefab, cannon.transform.position, cannon.transform.rotation);
        _audio.PlayOneShot(lasersound);
        yield return new WaitForSeconds(0.7f);
        IsAlreadyFiring = false;
    }

    private IEnumerator Destruction()
    {
        
        _animator.Play("Destruction");
        _audio.PlayOneShot(destructsound);
        _appDatas.killCounter++;
        EnemyKilled?.Invoke();
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
