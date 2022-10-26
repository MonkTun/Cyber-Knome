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
        walkDir = Settings.isMobile ? PlayerInput.MovementInput : new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        walkDir.Normalize();

        if (!Settings.isMobile)
        {
            Vector2 aimDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            aimDirection.Normalize();

            targetDir = aimDirection;
        }
        else
        {
            targetDir = PlayerInput.AimInput != Vector2.zero ? PlayerInput.AimInput : PlayerInput.MovementInput == Vector2.zero ? targetDir : PlayerInput.MovementInput;
        }

        if (((!Settings.isMobile && Input.GetMouseButton(0)) || (Settings.isMobile && PlayerInput.AimInput != Vector2.zero))&& myWeapon) //TODO: implement PlayerInput again for mobile
		{
            myWeapon.Use();
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

            if (selectedInteractable && (Input.GetKeyDown(KeyCode.Q) || PlayerInput.InteractInput)) //TODO: mobile input and key bind 
            {
                PlayerInput.SetInteractInput(false);
                SetWeapon(selectedInteractable.Interact()); //TODO interact for other type
            }

        } else
		{
            selectedInteractable = null;
        }
    }

    public void SetWeapon(GameObject weaponPrefab)
	{
        if (myWeapon)
		{
            myWeapon.Drop();
		}

        GameObject instantiatedWeapon = Instantiate(weaponPrefab, weaponPos);
        myWeapon = instantiatedWeapon.GetComponent<Weapon>();
		
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
