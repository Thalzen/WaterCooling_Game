using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] private float BulletSpeed = 10f;
    private int LayerIgnoreRaycast;
    private int LayerIgnoreBullet;
    private int LayerIgnoreEnemy;
    // Start is called before the first frame update
    void Start()
    {
        LayerIgnoreBullet = LayerMask.NameToLayer("Bullet");
        LayerIgnoreRaycast = LayerMask.NameToLayer("Ignore Raycast");
        LayerIgnoreEnemy = LayerMask.NameToLayer("Enemy");
        GetComponent<Rigidbody2D>().velocity = new Vector2(0f,-1f) * BulletSpeed;
        Physics.IgnoreLayerCollision(7,2);
        Physics.IgnoreLayerCollision(7,6);
        Physics.IgnoreLayerCollision(7,8);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.CompareTag("Player") || col.gameObject.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}
