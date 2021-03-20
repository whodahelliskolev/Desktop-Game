using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBreakGlass : MonoBehaviour
{
    private ShatterableGlass Glass;
    public float VelocityRequired;
    private ShatterableGlassInfo GlassInfo;
    // Start is called before the first frame update

    private void Awake()
    {
        Glass = this.GetComponent<ShatterableGlass>();
    }


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == 16 && collision.rigidbody.velocity.magnitude > VelocityRequired)
        {


            Vector3 Direction = collision.transform.position - transform.position;
            ShatterableGlassInfo GlassInfo = new ShatterableGlassInfo(collision.GetContact(0).point, Direction);
            GlassInfo.HitPoint = collision.GetContact(0).point;
            Glass.Shatter3D(GlassInfo);
        }

        else if(collision.gameObject.layer == 9 && collision.rigidbody.velocity.magnitude > VelocityRequired)
        {


            Vector3 Direction = collision.transform.position - transform.position;
            ShatterableGlassInfo GlassInfo = new ShatterableGlassInfo(collision.GetContact(0).point, Direction);
            GlassInfo.HitPoint = collision.GetContact(0).point;
            Glass.Shatter3D(GlassInfo);
        }
    }
}
