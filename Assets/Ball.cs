using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

    public bool pickedUp;
    Rigidbody2D rigidbody;
    Vector2 startingPosition;
    Collider2D collider;

    void Start(){
        pickedUp = false;
        rigidbody = GetComponent<Rigidbody2D>();
        startingPosition = transform.position;
        collider = GetComponent<Collider2D>();
    }


    void FixedUpdate(){
        if (pickedUp){
            rigidbody.position = transform.parent.position;
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
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Goal")){
            transform.position = startingPosition;
        }
    }
}
