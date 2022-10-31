using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimIndicator : MonoBehaviour
{
    [SerializeField] LineRenderer LR;
    [SerializeField] Transform aimIndicator;


    public void SetAimIndicator(Vector3 pivot, Vector3 rotation, float length, bool done)
    {
        if (done) 
        {
            LR.enabled = false;
        } else
		{
            LR.enabled = true;
        }

        if (aimIndicator.gameObject.activeInHierarchy) aimIndicator.gameObject.SetActive(false);
        

        RaycastHit2D hit = Physics2D.Raycast(pivot, rotation, length, 1 << LayerMask.NameToLayer("Enemy") | 1 << LayerMask.NameToLayer("Wall"));

        //LR.SetPosition(1, hit.point);
        
        if (hit)
		{
            


            print("detected");
            print(hit.collider.name);
            LR.SetPosition(1, hit.point - (Vector2)pivot);

        } else
		{
            LR.SetPosition(1, rotation.normalized *length);

        }
    }

    public void SetAimIndicator(Vector2 mousePosition)
	{
        //if (LR.enabled) LR.enabled = false;
        //if (!aimIndicator.gameObject.activeInHierarchy) aimIndicator.gameObject.SetActive(true);

        aimIndicator.position = Vector2.Lerp(aimIndicator.position, mousePosition, Time.deltaTime * 30);

    }

	private void Update()
	{
        if (Settings.isMobile)
		{
            if (!LR.gameObject.activeInHierarchy) LR.gameObject.SetActive(true);
            if (aimIndicator.gameObject.activeInHierarchy) aimIndicator.gameObject.SetActive(false);
        } 
        else
		{
            if (LR.gameObject.activeInHierarchy) LR.gameObject.SetActive(false);
            if (!aimIndicator.gameObject.activeInHierarchy) aimIndicator.gameObject.SetActive(true);
        }   
    }
}
