using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEntity : MonoBehaviour, IDamageable //TODO: add type
{
	[Header("GameEntity traits")]
	[SerializeField] int maxHealth;

	protected int health;
	protected bool isDead;

	public virtual void Start()
	{
		health = maxHealth;
		isDead = false;
	}

	public virtual void OnDamage(int damage)
	{
		if (isDead) return;

		health -= damage;

		if (health <= 0)
		{
			OnDeath();
			health = 0;
		}
	}

	public virtual void OnDeath()
	{
		if (isDead) return;

		isDead = true;
	}

}
