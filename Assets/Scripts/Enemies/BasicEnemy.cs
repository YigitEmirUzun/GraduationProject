using UnityEngine;

public class BasicEnemy : Enemy
{
    public float attackPrepTimer = 1;

    protected override void Update()
    {
        base.Update();
        if(distance < agent.stoppingDistance + 1)
        {
            attackPrepTimer -= Time.deltaTime;
        }
    }

    public override void Attack(float damage)
    {
        base.Attack(damage);

        
    }

}
