using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : GameEntity
{
	[SerializeField] Animator AN;
    //[SerializeField] SpriteRenderer SR;
	[SerializeField] NavMeshAgent agent;
    Transform target; //TODO: find nearby player

    [SerializeField] float sight;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (target)
		{
            if (Vector2.Distance(transform.position, target.position) < sight)
            {
                agent.SetDestination(target.position);
            }

            if (agent.velocity != Vector3.zero)
			{
                AN.SetBool("Walk", true);
            } else AN.SetBool("Walk", false);

            
            SR.flipX = target.position.x - transform.position.x < 0;

        } 
        else
		{
            Collider2D col = Physics2D.OverlapCircle(transform.position, sight, 1 << LayerMask.NameToLayer("Player"));

            if (col != null) 
            {
                print("collider is found");

                target = col.gameObject.transform;
            } 

            //print(col.gameObject);
		}
    }


}
