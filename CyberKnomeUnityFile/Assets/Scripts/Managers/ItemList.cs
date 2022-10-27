using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemList : MonoBehaviour
{
	[System.Serializable]
	public class ItemData
	{
		public string name;
		public string description;
		public Sprite sprite;
		public GameObject weaponPrefab;
		public GameObject itemPrefab;
	}

	[SerializeField] ItemData[] ultimateWeaponList;
	[SerializeField] ItemData[] ultimateItemList;

	public ItemData[] UltimateWeaponList 
	{ 
		get
		{
			return ultimateWeaponList;
		}
	}

	public ItemData[] UltimateItemList
	{
		get
		{
			return ultimateItemList;
		}
	}

	public static ItemList Instance;

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		} else
		{
			Destroy(gameObject);
			return;
		}
	}
}

