using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.UI;

public class Bullet : MonoBehaviour
{
    [SerializeField] float bulletSpeed;
    [SerializeField] float bulletDamage;
    [SerializeField] bool isArmourPenetration;
    [SerializeField] bool isEnemyBullet;

    public bool IsEnemyBullet { get => isEnemyBullet; set => isEnemyBullet = value; }
    public bool IsArmourPenetration { get => isArmourPenetration; set => isArmourPenetration = value; }
    public float BulletDamage { get => bulletDamage; set => bulletDamage = value; }

    void Start()
    {
        Destroy(gameObject,3f);
    }

    // Update is called once per frame
    void Update()
    {
        MoveTheBullet();
    }
    void MoveTheBullet()
    {
        transform.Translate(transform.right * bulletSpeed * Time.deltaTime, Space.World);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (IsEnemyBullet != collision.GetComponent<Unit>().IsEnemy)
        {
            collision.GetComponent<UnitStats>().UnitCurrentHealth -= BulletDamage;
            Destroy(gameObject);
        }
    }
}
