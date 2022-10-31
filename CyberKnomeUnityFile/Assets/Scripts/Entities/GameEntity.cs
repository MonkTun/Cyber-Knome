using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEntity : MonoBehaviour, IDamageable //TODO: add type
{
	[Header("GameEntity")]
	[SerializeField] protected SpriteRenderer SR;
	[SerializeField] protected Collider2D Collider;
	[SerializeField] protected int maxHealth;

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

		StartCoroutine(OnDamageGFX());
	}

	public virtual void OnDeath()
	{
		if (isDead) return;

		isDead = true;
		SR.enabled = false;
		Collider.enabled = false;

		Destroy(gameObject, 1);
	}

	IEnumerator OnDamageGFX()
	{
		SR.color = Color.black;
		yield return new WaitForSeconds(0.1f);
		SR.color = Color.white;
	}
}
