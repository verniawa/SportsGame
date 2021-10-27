using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {

    public bool pickedUp;
    Rigidbody2D rigidbody;
    Vector2 startingPosition;

    void Start(){
        pickedUp = false;
        rigidbody = GetComponent<Rigidbody2D>();
        startingPosition = transform.position;
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
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.layer == LayerMask.NameToLayer("Goal")){
            transform.position = startingPosition;
        }
    }

}
