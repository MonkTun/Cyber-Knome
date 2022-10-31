using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : Interactable
{
	[Header("Portal")]
	[SerializeField] int toScene = 1;

	public override int Interact()
	{
		GameManager.Instance.TravelScene(toScene);

		return -1;
	}
}
