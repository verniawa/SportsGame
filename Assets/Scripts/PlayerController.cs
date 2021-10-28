using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour {

    public float speed = 2f, shootSpeed = 20f, chargeRate;
    bool carrying, charging;
    Rigidbody2D rigidbody;
    public Ball ball;
    public Transform goal;
    Vector2 goalDirection;
    public float charge;
    Vector3[] shotAngleGuides;
    LineRenderer lineRenderer;
    Controls controls;
    Vector2 moveVector;
    Collider2D collider;
    private float shotCooldown = .2f;
    

    void Start(){
        rigidbody = GetComponent<Rigidbody2D>();
        carrying = false;
        lineRenderer = GetComponent<LineRenderer>();
        shotAngleGuides = new Vector3[3];
        controls = new Controls();
        controls.Enable();
        collider = GetComponent<Collider2D>();
        if (ball == null){
            ball = FindObjectOfType<Ball>();
        }
        if (goal == null){
            goal = GameObject.FindGameObjectWithTag("Goal").transform;
        }
    }

    public void OnMove(InputAction.CallbackContext context){
        moveVector = context.ReadValue<Vector2>();
    }

    void Update(){
        Vector2 positionDelta = moveVector * speed * Time.deltaTime;


        rigidbody.velocity = moveVector * speed;

        goalDirection = (goal.transform.position - transform.position).normalized;

        float distance = Vector2.Distance(transform.position, ball.transform.position);
        if (carrying){
            Vector2 minAngle = Vector2.Lerp(Vector2.Perpendicular(goalDirection).normalized, goalDirection, charge).normalized;
            Vector2 maxAngle = Vector2.Lerp(Vector2.Perpendicular(- goalDirection).normalized, goalDirection, charge).normalized;
            if (charging){
                if (charge < 1){
                    charge += chargeRate * Time.deltaTime;
                }
                shotAngleGuides[0] = transform.position + (Vector3) minAngle * 2;
                shotAngleGuides[2] = transform.position + (Vector3) maxAngle * 2;
                
            } else{
                shotAngleGuides[0] = transform.position;
                shotAngleGuides[2] = transform.position;
            }
            if (!(charging) && charge > 0){
                shoot(minAngle, maxAngle);
            }
        } else{
            charge = 0f;
            shotAngleGuides[0] = transform.position;
            shotAngleGuides[2] = transform.position;
        }
        shotAngleGuides[1] = transform.position;
        lineRenderer.SetPositions(shotAngleGuides);
    }

    public void OnShoot(InputAction.CallbackContext context){
        charging = context.ReadValueAsButton();
    }    

    void shoot(Vector2 minAngle, Vector2 maxAngle){
        if (carrying){
            ball.transform.position = transform.position + (Vector3) goalDirection * .2f;
            float shotAngle = Random.value;
            
            Vector2 direction = Vector2.Lerp(minAngle, maxAngle, shotAngle).normalized;
            Debug.DrawRay(transform.position, direction * 3, Color.red, 1f);
            
            ball.shoot(this.gameObject, direction * (shootSpeed * charge + shootSpeed));
            carrying = false;

            collider.isTrigger = true;
            StartCoroutine(cooldown());
        }
    }
    
    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ball")){
            if (!carrying && !ball.pickedUp){
                ball.pickup(this.gameObject);
                carrying = true;
            }

        }
    }

    IEnumerator cooldown(){
        yield return new WaitForSeconds(shotCooldown);
        collider.isTrigger = false;
    }
    
}
