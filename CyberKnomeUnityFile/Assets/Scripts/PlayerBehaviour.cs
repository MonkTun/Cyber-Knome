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

    float xAxis;
    float yAxis;
    Vector2 targetDir;
    Weapon myWeapon;
    float weaponRotation;

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
        xAxis = Input.GetAxisRaw("Horizontal");
        yAxis = Input.GetAxisRaw("Vertical");

        targetDir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        targetDir.Normalize();

        if (Input.GetMouseButton(0) && myWeapon)
		{
            myWeapon.Use();

        }
    }

	public void Movement() //TODO: inputManager
	{
        AN.SetBool("Walk", xAxis != 0 || yAxis != 0);

        SR.flipX = targetDir.x < 0;

        /*
        if (xAxis != 0)
        {
            SR.flipX = xAxis < 0;
        }
        */

        RB.velocity = new Vector2(xAxis, yAxis).normalized * speed;
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
            case WeaponType.gun:

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
            case WeaponType.bow:

                break;
        }
	}

    #endregion

    #region Interact

    public void Interact()
    {
        Collider2D[] nearestObjList = Physics2D.OverlapCircleAll(transform.position, 0.5f, 1 << LayerMask.NameToLayer("Interactable"));

        if (nearestObjList.Length > 0)
        {
            if (Input.GetKeyDown(KeyCode.Q)) //TODO: mobile input and key bind 
            {


                if (nearestObjList[0].TryGetComponent(out Interactable interactable))
                {
                    SetWeapon(interactable.Interact());
                    
                }
            }


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
