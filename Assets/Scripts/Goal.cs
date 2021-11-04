using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Goal : MonoBehaviour {
    
    int score = 0;
    public TextMeshProUGUI textMesh;
    ParticleSystem particleSystem;
    Shake cameraShake;
    private void Start() {
        textMesh.text = score.ToString();
        particleSystem = GetComponent<ParticleSystem>();
        cameraShake = Camera.main.GetComponent<Shake>();
    }
    public void scoreGoal(){
        score ++;
        textMesh.text = score.ToString();
        particleSystem.Play();
        StartCoroutine(cameraShake.cameraShake(.5f, 1f));
    }
    private void OnCollisionEnter2D(Collision2D collision) {
        ContactPoint2D contactPoint = collision.contacts[0];
        
    }

}
