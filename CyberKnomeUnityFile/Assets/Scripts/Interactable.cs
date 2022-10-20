using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
	[SerializeField] GameObject returnObject;
	[SerializeField] bool destroyAfterInteract;

    public GameObject Interact()
	{
		if (destroyAfterInteract) Destroy(gameObject);
		return returnObject;
	}
}
