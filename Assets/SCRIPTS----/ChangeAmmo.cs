using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeAmmo : MonoBehaviour
{

    public GunMechanics GunMechanics_Ref;
    public Text AmmoTypeText;
    public Animator BulletTypeAnimator;
    public static int CURRENT_BULLET_TYPE;
    public Animator GunAnimator;
    public Sprite[] CrosshairTypes;
    public Image Crosshair;


    public void Awake()
    {
        CURRENT_BULLET_TYPE = 0;
    }

    public IEnumerator SwitchAnimationToggle()
    {

        BulletTypeAnimator.SetBool("BulletSwapAnim", true);
        yield return new WaitForSeconds(0.05f);
        BulletTypeAnimator.SetBool("BulletSwapAnim", false);
    }
    public void normalAmmo()
    {
        Crosshair.sprite = CrosshairTypes[0];
        Crosshair.GetComponent<RectTransform>().sizeDelta = new Vector2(5, 5);
        StartCoroutine(SwitchAnimationToggle());
       

        GunAnimator.SetBool("RapidMode", false);
        GunAnimator.SetBool("NormalMode", true);
        GunAnimator.SetBool("ControllableMode", false);
        GunAnimator.SetBool("RicochetMode", false);
        GunMechanics_Ref.BulletType = 0;
        GunMechanics_Ref.HideRicochetTrajectory();
        CURRENT_BULLET_TYPE = 0;
        AmmoTypeText.text = "STANDART AMMO   ///";
    }

    public void controllableAmmo()
    {
        Crosshair.sprite = CrosshairTypes[1];
        Crosshair.GetComponent<RectTransform>().sizeDelta = new Vector2(90, 70);
        StartCoroutine(SwitchAnimationToggle());
        GunAnimator.SetBool("RapidMode", false);
        GunAnimator.SetBool("NormalMode", false);
        GunAnimator.SetBool("ControllableMode", true);
        GunAnimator.SetBool("RicochetMode", false);
        GunMechanics_Ref.BulletType = 1;
        CURRENT_BULLET_TYPE = 1;
        GunMechanics_Ref.HideRicochetTrajectory();
        AmmoTypeText.text = "CONTROLLABLE AMMO   ///";
    }

    public void rapidAmmo()
    {
        Crosshair.GetComponent<RectTransform>().sizeDelta = new Vector2(15, 15);
        StartCoroutine(SwitchAnimationToggle());
        Crosshair.sprite = CrosshairTypes[2];
        GunAnimator.SetBool("RapidMode", true);
        GunAnimator.SetBool("NormalMode", false);
        GunAnimator.SetBool("ControllableMode", false);
        GunAnimator.SetBool("RicochetMode", false);
        GunMechanics_Ref.BulletType = 2;
        GunMechanics_Ref.HideRicochetTrajectory();
        CURRENT_BULLET_TYPE = 2;
        AmmoTypeText.text = "RAPID AMMO   ///";
    }

    public void ricochetAmmo()
    {
        Crosshair.GetComponent<RectTransform>().sizeDelta = new Vector2(15, 15);
        StartCoroutine(SwitchAnimationToggle());
        Crosshair.sprite = CrosshairTypes[3];
        GunAnimator.SetBool("RapidMode", false);
        GunAnimator.SetBool("NormalMode", false);
        GunAnimator.SetBool("ControllableMode", false);
        GunAnimator.SetBool("RicochetMode", true);
        GunMechanics_Ref.BulletType = 3;
        CURRENT_BULLET_TYPE = 3;
        AmmoTypeText.text = "RICOCHET AMMO   ///";
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            normalAmmo();
        if (Input.GetKeyDown(KeyCode.Alpha2))
            rapidAmmo();
        if (Input.GetKeyDown(KeyCode.Alpha3))
            controllableAmmo();
        if (Input.GetKeyDown(KeyCode.Alpha4))
            ricochetAmmo();



    }       

}
