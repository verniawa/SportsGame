using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour {

    public float speed = 2f;
    public float shootSpeed = 20f, chargeRate;
    bool carrying;
    Rigidbody2D rigidbody;
    public Ball ball;
    public Transform goal;
    Vector2 goalDirection;
    public float charge;
    Vector3[] shotAngleGuides;
    LineRenderer lineRenderer;
    Controls controls;
    Vector2 moveVector;
    

    void Start(){
        rigidbody = GetComponent<Rigidbody2D>();
        carrying = false;
        lineRenderer = GetComponent<LineRenderer>();
        shotAngleGuides = new Vector3[3];
        controls = new Controls();
        controls.Enable();
    }


    void Update(){
        Vector2 positionDelta = controls.Game.Move.ReadValue<Vector2>().normalized * speed * Time.deltaTime;


        rigidbody.position += positionDelta;

        goalDirection = (goal.transform.position - transform.position).normalized;
        Debug.DrawRay(transform.position, goalDirection, Color.black, .1f);

        float distance = Vector2.Distance(transform.position, ball.transform.position);
        if (carrying){
            Vector2 minAngle = Vector2.Lerp(Vector2.Perpendicular(goalDirection).normalized, goalDirection, charge).normalized;
            Vector2 maxAngle = Vector2.Lerp(Vector2.Perpendicular(- goalDirection).normalized, goalDirection, charge).normalized;
            if (controls.Game.Shoot.ReadValue<float>() > 0){
                if (charge < 1){
                    charge += chargeRate * Time.deltaTime;
                }
                shotAngleGuides[0] = transform.position + (Vector3) minAngle * 2;
                shotAngleGuides[2] = transform.position + (Vector3) maxAngle * 2;
                
            } else{
                shotAngleGuides[0] = transform.position;
                shotAngleGuides[2] = transform.position;
            }
            if (!(controls.Game.Shoot.ReadValue<float>() > 0) && charge > 0){
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

    

    void shoot(Vector2 minAngle, Vector2 maxAngle){
        if (carrying){
            float shotAngle = Random.value;
            

            Vector2 direction = Vector2.Lerp(minAngle, maxAngle, shotAngle).normalized;
            Debug.DrawRay(transform.position, direction * 3, Color.red, 1f);

            Rigidbody2D ballRB = ball.GetComponent<Rigidbody2D>();
            ballRB.AddForce(direction * (shootSpeed * charge + shootSpeed), ForceMode2D.Impulse);
            ball.pickedUp = false;
            ball.transform.parent = null;
            carrying = false;
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

    // public void OnMove(InputAction.CallbackContext value){
    //     moveVector = value.ReadValue;
    // }
    
}
