using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileInputManager : MonoBehaviour
{
	public void OnInputMovement(Vector2 input) //TODO: put all input here also option for pc
	{
		MobilePlayerInput.SetPlayerInputMovement(input);
	}

	public void OnInputAim(Vector2 input)
	{
		MobilePlayerInput.SetPlayerInputAim(input);
	}

	public void OnInteract()
	{
		//print("onInteract");
		MobilePlayerInput.SetInteractInput(true);
	}

	public void OnWeaponInventorySelect(int index)
	{
		MobilePlayerInput.SetWeaponIventorySelected(index);
	}
}

public static class MobilePlayerInput
{
	public static Vector2 MovementInput { get; private set; }
	public static Vector2 AimInput { get; private set; }

	public static bool InteractInput { get; private set; }

	public static int WeaponIventorySelected { get; private set; }

	public static void SetPlayerInputMovement(Vector2 input)
	{
		MovementInput = input;
	}

	public static void SetPlayerInputAim(Vector2 input)
	{
		AimInput = input;
	}

	public static void SetInteractInput(bool input)
	{
		InteractInput = input;
	}

	public static void SetWeaponIventorySelected(int input)
	{
		WeaponIventorySelected = input;
	}
} 
