using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataBase : MonoBehaviour
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
	[SerializeField] GameObject[] ultimatePlayerList;

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

	public GameObject[] UltimatePlayerList
	{
		get
		{
			return ultimatePlayerList;
		}
	}

	public static GameDataBase Instance;

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

