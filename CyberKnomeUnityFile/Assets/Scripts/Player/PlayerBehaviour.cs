using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : GameEntity
{
    #region field_ref

    //[SerializeField] SpriteRenderer SR;
    [SerializeField] Animator AN;
    [SerializeField] Rigidbody2D RB;

    [SerializeField] float speed = 2;
    [SerializeField] float autoAimDistance = 4;

    [SerializeField] Transform weaponPos;
    [SerializeField] AimIndicator aimIndicator;

    [Header("Saving System Related")]
	[HideInInspector]
    public int characterCode;

    #endregion

    #region field

    Vector2 walkDir;
    Vector2 targetDir;
    Interactable selectedInteractable;
    Weapon myWeapon;
    float weaponRotation;

    bool mobile_isBrawlAiming;
    bool mobile_isBrawlShooting;
    int mobile_tempBrawlCount;
    float mobile_aimTime;

    List<int> weaponInventory = new List<int>();
    public int currentWeaponIndex;
    int maxWeaponCount = 4;
    public int GetHealth
    {
        get
        {
            return health;
        }
    }
    public List<int> GetWeaponInventory
    {
        get
        {
            return weaponInventory;
        }
    }

    #endregion

    #region start

    public override void Start()
	{
        base.Start();

        //TODO:load save file... replace setupData?
    }

    public void SetupData(int _health, List<int> _weaponInventory, int _currentWeaponIndex)
	{
        health = _health;

        foreach (int i in _weaponInventory)
		{
            SetWeapon(i);
        }

        SwapWeapon(_currentWeaponIndex);
	}

	#endregion

	#region updates

	void Update()
    {
        if (GameManager.Instance.gameState == GameManager.State.paused) 
        {
            RB.velocity = Vector2.zero;
            return;
        } 
        

        ReceiveInput();
        Movement();
        Interact();
        UpdateWeaponOrientation();
    }

    public void ReceiveInput()
	{
        walkDir = Settings.isMobile ? MobilePlayerInput.MovementInput : new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        walkDir.Normalize();

        if (myWeapon)
		{
            if (!Settings.isMobile)
            {

                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 aimDirection = mousePos - (Vector2)weaponPos.position;
                aimDirection.Normalize();

                aimIndicator.SetAimIndicator(mousePos);

                targetDir = aimDirection;

                if (Input.GetMouseButton(0))
                {
                    myWeapon.Use();
                }
            }
            else
            {
                switch (myWeapon.weaponType)
				{
                    case WeaponType.gun:

                        if (mobile_isBrawlShooting)
                        {
                            if (mobile_tempBrawlCount < myWeapon.brawlAimingMax)
                            {
                                if (myWeapon.Use())
                                    mobile_tempBrawlCount++;
                            }
                            else
                            {
                                mobile_tempBrawlCount = 0;
                                mobile_isBrawlShooting = false;
                            }
                            aimIndicator.SetAimIndicator(weaponPos.position, targetDir, myWeapon.distance, true);
                            mobile_aimTime = 0;
                        }
                        else
                        {
                            if (MobilePlayerInput.AimInput != Vector2.zero)
                            {
                                mobile_aimTime += Time.deltaTime;

                                if (mobile_aimTime > 0.1f)
                                {
                                    targetDir = MobilePlayerInput.AimInput;
                                    aimIndicator.SetAimIndicator(weaponPos.position, targetDir, myWeapon.distance, false);
                                }
                                else
								{
                                    Vector2 enemyDir = FindEnemyNearby(20);
                                    if (enemyDir != Vector2.zero)
                                        targetDir = enemyDir;

                                    
                                    aimIndicator.SetAimIndicator(weaponPos.position, targetDir, myWeapon.distance, true);
                                }

                                mobile_isBrawlAiming = true;
                            }
                            else if (mobile_isBrawlAiming)
                            {
                                mobile_isBrawlAiming = false;
                                mobile_isBrawlShooting = true;

                                aimIndicator.SetAimIndicator(weaponPos.position, targetDir, myWeapon.distance, true);
                            } 
                            else
							{
                                targetDir = MobilePlayerInput.MovementInput == Vector2.zero ? targetDir : MobilePlayerInput.MovementInput;
                            }
                        }

                        break;
                    case WeaponType.staff:

                        if (MobilePlayerInput.MovementInput != Vector2.zero)
                            targetDir = MobilePlayerInput.MovementInput;

                        if (MobilePlayerInput.AimInput != Vector2.zero)
                        {
                            myWeapon.Use();
                            mobile_isBrawlAiming = false;
                        }

                        break;
                    default:

                        Vector2 enemyDir2 = FindEnemyNearby(5);

                        targetDir = enemyDir2 == Vector2.zero ? MobilePlayerInput.MovementInput : enemyDir2;

                        if (MobilePlayerInput.AimInput != Vector2.zero)
                        {
                            myWeapon.Use();
                            mobile_isBrawlAiming = false;
                        }

                        break;
                }
            }
        }

        if (!Settings.isMobile && Input.GetAxisRaw("Mouse ScrollWheel") > 0f) //mouse wheel forward
        {

            SwapWeaponWithDirection(true);

        }
        else if (!Settings.isMobile && Input.GetAxisRaw("Mouse ScrollWheel") < 0f) //backward
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

                if (targetDir.x > 0.1f)
				{
                    weaponPos.localPosition = new Vector2(0.5f, 0.5f);
                    weaponPos.localScale = new Vector2(1f, 1f);

                } else if (targetDir.x < -0.1f)
				{
                    weaponPos.localPosition = new Vector2(-0.5f, 0.5f);
                    weaponPos.localScale = new Vector2(-1f, 1f);
                }

                break;
            default: 
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
        }
	}

    public Vector2 FindEnemyNearby(float distance)
    {
        Collider2D[] nearestObjList = Physics2D.OverlapCircleAll(transform.position, distance, 1 << LayerMask.NameToLayer("Enemy"));

        if (nearestObjList.Length > 0)
		{
            Vector2 temp = nearestObjList[0].gameObject.transform.position;

            for (int i = 0; i < nearestObjList.Length; i++)
            {
                if (Vector2.Distance(transform.position, nearestObjList[i].gameObject.transform.position) < Vector2.Distance(transform.position, temp))
                {
                    temp = nearestObjList[i].gameObject.transform.position;
                }
            }

            return (temp - (Vector2)weaponPos.position).normalized;
        } 
        else
		{
            return Vector2.zero;
		}
    }

    #endregion

    #region Interact

    public void Interact()
    {
        Collider2D[] nearestObjList = Physics2D.OverlapCircleAll(transform.position, 1.5f, 1 << LayerMask.NameToLayer("Interactable"));

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
        if (weaponItemCode < 0) return;

        //print(weaponInventory.Count + " " + maxWeaponCount);
        if (weaponInventory.Count >= maxWeaponCount) //if playerinventoryCount is greater or equals to maxWeaponCount
        {
            //exchange when max

            weaponInventory[currentWeaponIndex] = weaponItemCode; //update weaponInventory's currentWeaponIndex with new weaponItemCode

            if (myWeapon)
            {
                myWeapon.Drop(); //drop the old weapon
            }

            GameObject instantiatedWeapon = Instantiate(GameDataBase.Instance.UltimateWeaponList[weaponItemCode].weaponPrefab, weaponPos);
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

            GameObject instantiatedWeapon = Instantiate(GameDataBase.Instance.UltimateWeaponList[weaponItemCode].weaponPrefab, weaponPos);
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

            GameObject instantiatedWeapon = Instantiate(GameDataBase.Instance.UltimateWeaponList[weaponInventory[index]].weaponPrefab, weaponPos);
            myWeapon = instantiatedWeapon.GetComponent<Weapon>();

            UICommunicator.Instance.WeaponInventoryUpdate(weaponInventory, currentWeaponIndex);

            mobile_isBrawlAiming = false;
            mobile_isBrawlShooting = false;
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
