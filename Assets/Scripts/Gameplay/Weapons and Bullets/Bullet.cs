using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float bulletSpeed;
    [SerializeField] float bulletDamage;
    [SerializeField] float criticalChance;
    [SerializeField] bool isArmourPenetration;
    [SerializeField] bool isEnemyBullet;
    [SerializeField] bool isCritical;
    [SerializeField] KillCounter killCounter;
    [SerializeField] UnitStats bulletOwner;

    public bool IsEnemyBullet { get => isEnemyBullet; set => isEnemyBullet = value; }
    public bool IsArmourPenetration { get => isArmourPenetration; set => isArmourPenetration = value; }
    public float BulletDamage { get => bulletDamage; set => bulletDamage = value; }
    public float CriticalChance { get => criticalChance; set => criticalChance = Mathf.Clamp(value,0,100); }
    public bool IsCritical { get => isCritical; set => isCritical = value; }
    public KillCounter KillCounter { get => killCounter; set => killCounter = value; }
    public UnitStats BulletOwner { get => bulletOwner; set => bulletOwner = value; }

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
        UnitStats unitStats = collision.GetComponent<UnitStats>();
        if (IsEnemyBullet != collision.GetComponent<Unit>().IsEnemy)
        {
            float enemyBulletResistance = unitStats.UnitBulletResistance;
            float reducedDamage = unitStats.CalculateReducedDamage(BulletDamage, enemyBulletResistance, IsCritical);
            if (unitStats.UnitCurrentHealth <= reducedDamage)
            {
                AddDeadEnemyToCounter(unitStats.SoStats);
            }
            unitStats.UnitCurrentHealth -= reducedDamage;
            KillCounter.DamageDealt += reducedDamage;
            Destroy(gameObject);
        }
    }

    void AddDeadEnemyToCounter(SOUnitStats enemy)
    {
        KillCounter.KillsList.Add(enemy);
    }
}
