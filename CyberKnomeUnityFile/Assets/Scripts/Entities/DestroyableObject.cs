using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableObject : GameEntity
{
	[SerializeField] SpriteRenderer SR;
	[SerializeField] Collider2D Collider;

	public override void OnDamage(int damage)
	{
		if (isDead) return;

		//print("health:" + health);
		StartCoroutine(OnDamageGFX());

		base.OnDamage(damage);
	}

	IEnumerator OnDamageGFX()
	{
		SR.color = Color.black;
		yield return new WaitForSeconds(0.1f);
		SR.color = Color.white;
	}

	public override void OnDeath()
	{
		if (isDead) return;

		print("DestroyableObject Destroyed");

		SR.enabled = false;
		Collider.enabled = false;

		Destroy(gameObject, 1);

		base.OnDeath();
	}
}
