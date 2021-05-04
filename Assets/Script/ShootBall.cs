using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BasketballGameDefinition;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class ShootBall : MaterialController
{
    private Vector3 mousePressDownPos;
    private Vector3 mouseReleasePos;

    private Rigidbody rb;   
    private AudioSource auSource;

    public float speed = 1;

    private bool isShoot;
    private bool isBasket = false;

    private float minDist = 100f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();       
        auSource = GetComponent<AudioSource>();
    }


    private void OnMouseDown()
    {
        mousePressDownPos = Input.mousePosition;       
    }

    private void OnMouseDrag()
    {
        if (Vector3.Distance(Input.mousePosition, mousePressDownPos) > minDist)
        {
            SetMaterialOutLine(0f);
        }
        else
        {
            SetMaterialOutLine(0.01f);
        }
    }

    private void OnMouseUp()
    {       
        mouseReleasePos = Input.mousePosition;                
        if (Vector3.Distance(mouseReleasePos, mousePressDownPos) < minDist)
            return;


        Shoot(mouseReleasePos - mousePressDownPos);
        Invoke("HideObject", 4f);
    }

    
    void Shoot(Vector3 Force)
    {
        if (isShoot)
            return;

        rb.useGravity = true;
        rb.AddForce(new Vector3(Force.x, Force.y, Force.y*.5f) * speed);
        
        isShoot = true;
        GameManager.Instance.BallShooted();
        AudioController.Instance.PlaySound(SoundEffect.ShootBall, auSource);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("enter 1");
        if (other.gameObject.tag == Tags.Target.ToString())
        {
            //// doðru yerden mi giriþ yapmýþ
            isBasket = (other.transform.position.y - transform.position.y < 0);
            Debug.Log("giriþ " + isBasket.ToString());           
        }        
    }

    private void OnTriggerExit(Collider other)
    {      
        Debug.Log("exit 1");
        if (other.gameObject.tag == Tags.Target.ToString())
        {
            // doðru yerden mi çýkýþ yapmýþ
            isBasket = (other.transform.position.y - transform.position.y > 0);
            Debug.Log("çýkýþ " + isBasket.ToString());

            if (isBasket)
                SuccessfullShoot();
        }
    }

    // þu an kullanmýyorum ama deneyimi arttýrmak için kullanýlýr
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == Tags.BasketballHoop.ToString())
        {
            //ses
            AudioController.Instance.PlaySound(SoundEffect.HitHoopBall, auSource);
        }
        else if (collision.gameObject.tag == Tags.Plane.ToString())
        {
            //ses   
            AudioController.Instance.PlaySound(SoundEffect.HitGroundBall, auSource);
        }
        else if (collision.gameObject.tag == Tags.Other.ToString())
        {
            //ses
        }
    }

    public void SuccessfullShoot()
    {
        Debug.Log("sayýýý");
        GameManager.Basket.Invoke();
        AudioController.Instance.PlaySound(SoundEffect.SuccessfullBall, auSource);
    }

    private void HideObject()
    {
        ResetBall();
        this.gameObject.SetActive(false);
    }

    public override void SetMaterialColor(Color _color)
    {
        base.SetMaterialColor(_color);
    }

    public void ResetBall()
    {
        rb.velocity = Vector3.zero;
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        SetMaterialOutLine(0.01f);
        isShoot = false;       
    }

   
}