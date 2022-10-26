using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavesManager : MonoBehaviour
{
    public static SavesManager Instance;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else
		{
            Destroy(gameObject);
            return;
		}
    }

    public void SaveProgress
    (
        int character,
        int health,
        byte scene/*,
		GameObject weapon
        */
    )
	{
        ES3.Save("save_character", character);
        ES3.Save("save_health", health);
        ES3.Save("save_scene", scene);
        //ES3.Save("save_weapon", weapon);
    }
}
