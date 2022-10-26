using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
	[SerializeField] InteractableData interactData;
	[SerializeField] GameObject returnObject;
	[SerializeField] bool destroyAfterInteract;

	public InteractableData InteractCheck()
	{
		return interactData;
	}

    public GameObject Interact()
	{
		if (destroyAfterInteract) Destroy(gameObject);
		return returnObject;
	}
}

[System.Serializable]
public class InteractableData
{
	public enum InteractType //TODO add more
	{
		grab,
		talk,
		open,
	}

	public InteractType interactType;
	public Interactable interactable;
}
