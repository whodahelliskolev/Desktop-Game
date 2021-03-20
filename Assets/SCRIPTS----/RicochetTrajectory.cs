using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RicochetTrajectory : MonoBehaviour
{


    public GameObject Crosshair;
    public GameObject INVALIDTARGET_TEXT;

    public LineRenderer trajectoryRenderer;
    public GameObject BulletOrigin;
    public GameObject RicochetHitPoint;
    public float trajectoryLength;
    public bool canShowTrajectory;
    RaycastHit firstBounce;
    RaycastHit SecondBounce;


    private GunMechanics gunMechanics;
    private Vector3 Direction;
    private Vector3 SecondBounceOrigin;
    private bool firstBounceRegistered;
    private bool secondBounceRegistered;
    public LayerMask FirstBounceMask;
    public LayerMask SecondBounceMask;


    private void Awake()
    {
        gunMechanics = GetComponent<GunMechanics>();
    }

    void RaycastTrajectory()
    {
        
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));

        RaycastHit hit;
        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit,trajectoryLength, FirstBounceMask, QueryTriggerInteraction.Ignore))
        {
            targetPoint = hit.point;
            canShowTrajectory = true;
            
            Crosshair.SetActive(true);
            INVALIDTARGET_TEXT.SetActive(false);
        }
                    
        else {
            targetPoint = ray.GetPoint(trajectoryLength);
            
            
        }

        if (hit.collider == null)
        {
            canShowTrajectory = false;

            Crosshair.SetActive(false);
            INVALIDTARGET_TEXT.SetActive(true);
        }
       


            Direction = (targetPoint - BulletOrigin.transform.position).normalized;
            Physics.Raycast(BulletOrigin.transform.position, Direction, out firstBounce, trajectoryLength, FirstBounceMask, QueryTriggerInteraction.Ignore);

            if (firstBounce.collider != null)
            {

                firstBounceRegistered = true;
                SecondBounceOrigin = firstBounce.point;
                Vector3 ReflectedSecondBounce = Vector3.Reflect(Direction, firstBounce.normal);
                Physics.Raycast(SecondBounceOrigin, ReflectedSecondBounce, out SecondBounce, trajectoryLength, SecondBounceMask, QueryTriggerInteraction.Ignore);

                if (SecondBounce.collider != null)
                {
                    

                }

            }
            else if (firstBounce.collider == null)
            {

                firstBounceRegistered = false;

            }

        

        
    }



    void DrawRichochetLines()
    {
        if (firstBounceRegistered)
        {

            if (!firstBounce.collider.CompareTag("Enemy"))
            {
                trajectoryRenderer.positionCount = 4;
                trajectoryRenderer.SetPosition(0, BulletOrigin.transform.position);
                trajectoryRenderer.SetPosition(1, firstBounce.point);
                trajectoryRenderer.SetPosition(2, SecondBounceOrigin);
                trajectoryRenderer.SetPosition(3, SecondBounce.point);
                RicochetHitPoint.transform.position = SecondBounce.point;
            }

            else if (firstBounce.collider.CompareTag("Enemy") || firstBounce.collider.gameObject.layer == 14)   /// stop bounce trajectory if its on enemy or Glass
            {
                trajectoryRenderer.positionCount = 2;
                trajectoryRenderer.SetPosition(0, BulletOrigin.transform.position);
                trajectoryRenderer.SetPosition(1, firstBounce.point);
                RicochetHitPoint.transform.position = firstBounce.point;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        
           if(ChangeAmmo.CURRENT_BULLET_TYPE == 3 && gunMechanics.isAiming)
            {
                RaycastTrajectory();
                DrawRichochetLines();
            } 
        

            else if (ChangeAmmo.CURRENT_BULLET_TYPE == 0)
             {
            Crosshair.SetActive(true);
            INVALIDTARGET_TEXT.SetActive(false);
             }





        
       
        
        

    }
}
