using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SavesManager
{
    public static void SaveProgress(ProgressData data)
    {
        ES3.Save("save_character", data.character);
        ES3.Save("save_health", data.health);
        ES3.Save("save_scene", data.scene);
        ES3.Save("save_position", data.position);
        ES3.Save("save_weaponInventory", data.weaponInventory);
        ES3.Save("save_currentWeaponIndex", data.currentWeaponIndex);
    }

    public static ProgressData LoadProgress()
	{
        int _character = ES3.Load("save_character", 0);

        int _health = ES3.Load("save_health", 200); //TODO: health system and effects

        int _scene = ES3.Load("save_scene", 1); //TODO: level selector?

        Vector2 _position = ES3.Load("save_position", Vector2.zero);

        List<int> defaultWeaponInventory = new List<int>();
        List<int> _weaponInventory = ES3.Load("save_weaponInventory", defaultWeaponInventory);

        int _currentWeaponIndex = ES3.Load("save_currentWeaponIndex", 0);


        return (new ProgressData(_character, _health, _scene, _position, _weaponInventory, _currentWeaponIndex));
    }
}

public class ProgressData
{
    public int character;
    public int health;
    public int scene;
    public Vector2 position;
    public List<int> weaponInventory;
    public int currentWeaponIndex;

    public ProgressData(int _character, int _health, int _scene, Vector2 _position, List<int> _weaponInventory, int _currentWeaponIndex)
	{
		character = _character;
		health = _health;
		scene = _scene;
		position = _position;
		weaponInventory = _weaponInventory;
		currentWeaponIndex = _currentWeaponIndex;
	}
}
