using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

    public bool pickedUp;
    Rigidbody2D rigidbody;
    Vector2 startingPosition;
    Collider2D collider;
    TrailRenderer trailRenderer;


    void Start(){
        pickedUp = false;
        startingPosition = transform.position;
        initialize();
    }
    private void Reset() {
        initialize();
    }

    private void initialize(){
        if (rigidbody == null){
            rigidbody = GetComponent<Rigidbody2D>();
        }
        if (collider == null){
            collider = GetComponent<Collider2D>();
        }
        if (trailRenderer == null){
            trailRenderer = GetComponent<TrailRenderer>();
        }

    }


    void FixedUpdate(){
        if (pickedUp){
            rigidbody.position = transform.parent.position;
            trailRenderer.emitting = false;
        }
    }

    public void pickup(GameObject player){
        pickedUp = true;
        transform.SetParent(player.transform);
        rigidbody.velocity = Vector2.zero;
        Physics2D.IgnoreCollision(collider, player.GetComponent<Collider2D>(), true);
    }

    public void shoot(GameObject player, Vector2 force){
        rigidbody.AddForce(force, ForceMode2D.Impulse);
        transform.parent = null;
        pickedUp = false;
        Physics2D.IgnoreCollision(collider, player.GetComponent<Collider2D>(), false);
        trailRenderer.emitting = true;
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Goal")){
            trailRenderer.emitting = false;
            transform.position = startingPosition;
            rigidbody.velocity = Vector2.zero;
            other.gameObject.GetComponent<Goal>().scoreGoal();
        }
    }
}
