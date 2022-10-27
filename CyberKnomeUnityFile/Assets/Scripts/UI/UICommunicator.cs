using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICommunicator : MonoBehaviour
{
	#region fields

	[SerializeField] GameObject mobilePanel, PCPanel;
	[Header("Interact")]
	[SerializeField] UIAccessor[] interactUI;
	[SerializeField] Sprite talkSprite, grabSprite, openSprite;
	[Header("WeaponInventory")]
	[SerializeField] UIAccessor[] hotbarButtons;

	float interactHighlightTime;
	bool interactActive;

	#endregion

	public static UICommunicator Instance { get; private set; }

	private void OnEnable()
	{
		if (Instance == null)
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
			return;
		}
	}

	public void Update()
	{
		if (Settings.isMobile)
		{
			mobilePanel.SetActive(true);
			PCPanel.SetActive(false);
		} 
		else
		{
			mobilePanel.SetActive(false);
			PCPanel.SetActive(true);
		}
	}

	#region Interact

	public void InteractNearby(InteractableData data)
	{
		interactHighlightTime = 0.2f;

		switch (data.interactType)
		{
			case InteractableData.InteractType.talk:
				foreach (UIAccessor access in interactUI)
				{
					if (access.txt) access.txt.text = "Q - talk";
					if (access.img) access.img.sprite = talkSprite;
				}
				break;
			case InteractableData.InteractType.open:
				foreach (UIAccessor access in interactUI)
				{
					if (access.txt) access.txt.text = "Q - open";
					if (access.img) access.img.sprite = openSprite;
				}
				break;
			case InteractableData.InteractType.grab:
				foreach (UIAccessor access in interactUI)
				{
					if (access.txt) access.txt.text = "Q - grab";
					if (access.img) access.img.sprite = grabSprite;
				}
				break;
		}

		if (interactActive) return;
		StartCoroutine(InteractNearbyRoutine());

		//InteractUIImg //TODO: set images accordingly maybe even some texts
	}

	IEnumerator InteractNearbyRoutine()
	{
		foreach (UIAccessor access in interactUI)
		{
			access.gameObject.SetActive(true);
		}
		interactActive = true;

		while (interactHighlightTime > 0)
		{
			//print(interactHighlightTime);
			yield return new WaitForSeconds(0.05f);
			interactHighlightTime -= 0.05f;
		}

		foreach (UIAccessor access in interactUI)
		{
			access.gameObject.SetActive(false);
		}
		interactActive = false;
	}

	#endregion

	#region weapon inventory

	public void WeaponInventoryUpdate(List<int> _weaponInventory, int _currentWeaponIndex)
	{
		for (int i = 0; i < 4; i++)
		{
			hotbarButtons[i].SetHighlight(i == _currentWeaponIndex);

			if (i < _weaponInventory.Count)
			{
				hotbarButtons[i].img.sprite = ItemList.Instance.UltimateWeaponList[_weaponInventory[i]].sprite;
				hotbarButtons[i].img.color = Color.white;

			} else
			{
				hotbarButtons[i].img.sprite = null;
				hotbarButtons[i].img.color = Color.clear;
			}

		}
	}

	#endregion
}
