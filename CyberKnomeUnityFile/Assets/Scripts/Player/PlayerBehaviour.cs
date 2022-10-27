using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : GameEntity
{
	#region field_ref

	[SerializeField] SpriteRenderer SR;
    [SerializeField] Animator AN;
    [SerializeField] Rigidbody2D RB;

    [SerializeField] float speed = 2;

    [SerializeField] Transform weaponPos;

    #endregion

    #region field

    Vector2 walkDir;
    Vector2 targetDir;
    Interactable selectedInteractable;
    Weapon myWeapon;
    float weaponRotation;

    //TODO: weapon inventory
    List<int> weaponInventory = new List<int>();
    int currentWeaponIndex;
    int maxWeaponCount = 4;

	#endregion

	#region start

	public override void Start()
	{
        base.Start();

        //load save file
    }

	#endregion

	#region updates

	void Update()
    {
        if (GameManager.Instance.gameState == GameManager.State.paused) return;

        ReceiveInput();
        Movement();
        Interact();
        UpdateWeaponOrientation();
    }

    public void ReceiveInput()
	{
        walkDir = Settings.isMobile ? MobilePlayerInput.MovementInput : new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        walkDir.Normalize();

        if (!Settings.isMobile)
        {
            Vector2 aimDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            aimDirection.Normalize();

            targetDir = aimDirection;
        }
        else
        {
            targetDir = MobilePlayerInput.AimInput != Vector2.zero ? MobilePlayerInput.AimInput : MobilePlayerInput.MovementInput == Vector2.zero ? targetDir : MobilePlayerInput.MovementInput;
        }

        if (((!Settings.isMobile && Input.GetMouseButton(0)) || (Settings.isMobile && MobilePlayerInput.AimInput != Vector2.zero))&& myWeapon) //TODO: implement PlayerInput again for mobile
		{
            myWeapon.Use();
        }

        if (Input.GetAxisRaw("Mouse ScrollWheel") > 0f) //mouse wheel forward
        {

            SwapWeaponWithDirection(true);

        }
        else if (Input.GetAxisRaw("Mouse ScrollWheel") < 0f) //backward
        {
            SwapWeaponWithDirection(false);
		}

        if (Input.GetKeyDown(KeyCode.Alpha1) || (Settings.isMobile && MobilePlayerInput.WeaponIventorySelected == 0)) //TODO: mobile
		{
            SwapWeapon(0);
            MobilePlayerInput.SetWeaponIventorySelected(-1);

        } 
        else if (Input.GetKeyDown(KeyCode.Alpha2) || (Settings.isMobile && MobilePlayerInput.WeaponIventorySelected == 1))
        {
            SwapWeapon(1);
            MobilePlayerInput.SetWeaponIventorySelected(-1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) || (Settings.isMobile && MobilePlayerInput.WeaponIventorySelected == 2))
        {
            SwapWeapon(2);
            MobilePlayerInput.SetWeaponIventorySelected(-1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4) || (Settings.isMobile && MobilePlayerInput.WeaponIventorySelected == 3))
        {
            SwapWeapon(3);
            MobilePlayerInput.SetWeaponIventorySelected(-1);
        }
    }

	public void Movement() //TODO: inputManager
	{
        AN.SetBool("Walk", walkDir.x != 0 || walkDir.y != 0);

        SR.flipX = targetDir.x < 0;


        RB.velocity = walkDir * speed;
    }


    public void UpdateWeaponOrientation() //TODO: inputManager
    {
        if (myWeapon == null) return;

        switch (myWeapon.weaponType)
		{
            case WeaponType.staff:
                weaponPos.localScale = new Vector3(1, 1, 1);
                weaponPos.transform.rotation = Quaternion.identity;

                if (targetDir.x > 0)
				{
                    weaponPos.localPosition = new Vector2(0.5f, 0.5f);
                    weaponPos.localScale = new Vector2(1f, 1f);

                } else
				{
                    weaponPos.localPosition = new Vector2(-0.5f, 0.5f);
                    weaponPos.localScale = new Vector2(-1f, 1f);
                }

                break;
            case WeaponType.gun: //TODO shrink these codes
                weaponPos.localPosition = new Vector2(0, 0.5f);
                weaponPos.localScale = new Vector2(1f, 1f);

                weaponRotation = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg;

                weaponPos.transform.rotation = Quaternion.Euler(0f, 0f, weaponRotation);
                if (weaponRotation < -90 || weaponRotation > 90)
                {
                    weaponPos.localScale = new Vector3(1, -1, 1);
                }
                else
                {
                    weaponPos.localScale = new Vector3(1, 1, 1);
                }
                break;
            case WeaponType.sword:
                weaponPos.localPosition = new Vector2(0, 0.5f);
                weaponPos.localScale = new Vector2(1f, 1f);

                weaponRotation = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg;

                weaponPos.transform.rotation = Quaternion.Euler(0f, 0f, weaponRotation);
                if (weaponRotation < -90 || weaponRotation > 90)
				{
                    weaponPos.localScale = new Vector3(1, -1, 1);
                } else
				{
                    weaponPos.localScale = new Vector3(1, 1, 1);
                }

                break;
        }
	}

    #endregion

    #region Interact

    public void Interact()
    {
        Collider2D[] nearestObjList = Physics2D.OverlapCircleAll(transform.position, 2, 1 << LayerMask.NameToLayer("Interactable"));

        if (nearestObjList.Length > 0)
        {
            if (nearestObjList[0].TryGetComponent(out Interactable interactable))
            {
                InteractableData data = interactable.InteractCheck();
                selectedInteractable = data.interactable;
                UICommunicator.Instance.InteractNearby(data);
            }

            if (selectedInteractable && (Input.GetKeyDown(KeyCode.Q) || MobilePlayerInput.InteractInput)) //TODO: mobile input and key bind 
            {
                MobilePlayerInput.SetInteractInput(false);
                SetWeapon(selectedInteractable.Interact()); //TODO interact for other type
            }

        } else
		{
            selectedInteractable = null;
        }
    }

	#endregion

	#region Weapon Inventory

	public void SetWeapon(int weaponItemCode)
	{
        print(weaponInventory.Count + " " + maxWeaponCount);
        if (weaponInventory.Count >= maxWeaponCount) //if playerinventoryCount is greater or equals to maxWeaponCount
        {
            //exchange when max

            weaponInventory[currentWeaponIndex] = weaponItemCode; //update weaponInventory's currentWeaponIndex with new weaponItemCode

            if (myWeapon)
            {
                myWeapon.Drop(); //drop the old weapon
            }

            GameObject instantiatedWeapon = Instantiate(ItemList.Instance.UltimateWeaponList[weaponItemCode].weaponPrefab, weaponPos);
            myWeapon = instantiatedWeapon.GetComponent<Weapon>();
        }
        else
        {
            //add new when not max

            //destroy but not drop the current my weapon if you have one
            if (myWeapon)
            {
                Destroy(myWeapon.gameObject);
            }

            GameObject instantiatedWeapon = Instantiate(ItemList.Instance.UltimateWeaponList[weaponItemCode].weaponPrefab, weaponPos);
            myWeapon = instantiatedWeapon.GetComponent<Weapon>();

            weaponInventory.Add(weaponItemCode);
            currentWeaponIndex = weaponInventory.Count - 1;
        }
        UICommunicator.Instance.WeaponInventoryUpdate(weaponInventory, currentWeaponIndex);
    }

    public void SwapWeapon(int index)
	{
        if (weaponInventory.Count > index)
		{
            print(index);

            if (myWeapon)
            {
                Destroy(myWeapon.gameObject);
            }

            currentWeaponIndex = index;

            GameObject instantiatedWeapon = Instantiate(ItemList.Instance.UltimateWeaponList[weaponInventory[index]].weaponPrefab, weaponPos);
            myWeapon = instantiatedWeapon.GetComponent<Weapon>();

            UICommunicator.Instance.WeaponInventoryUpdate(weaponInventory, currentWeaponIndex);
        } 
    }

    public void SwapWeaponWithDirection(bool next)
	{
        if (!Settings.inverseMouseWheel)
		{
            if (next)
		    {
                if (currentWeaponIndex < weaponInventory.Count - 1)
                {
                    currentWeaponIndex++;
                }
                else currentWeaponIndex = 0;
            } else
		    {
                if (1 <= currentWeaponIndex)
                {
                    currentWeaponIndex--;
                }
                else currentWeaponIndex = weaponInventory.Count != 0 ? weaponInventory.Count - 1 : 0;
            }
		} else
		{
            if (!next)
            {
                if (currentWeaponIndex < weaponInventory.Count - 1)
                {
                    currentWeaponIndex++;
                }
                else currentWeaponIndex = 0;
            }
            else
            {
                if (1 <= currentWeaponIndex)
                {
                    currentWeaponIndex--;
                }
                else currentWeaponIndex = weaponInventory.Count != 0 ? weaponInventory.Count - 1 : 0;
            }
        }

        SwapWeapon(currentWeaponIndex);
        //print(currentWeaponIndex + " " + (weaponInventory.Count - 1));
    }

	#endregion

	#region Health

	public override void OnDamage(int damage)
	{
        if (isDead) return;

        base.OnDamage(damage);
	}

	public override void OnDeath()
	{
        if (isDead) return;

        base.OnDeath();
	}

	#endregion
}
