using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Goal : MonoBehaviour {
    
    int score = 0;
    public TextMeshProUGUI textMesh;
    ParticleSystem particleSystem;
    private void Start() {
        textMesh.text = score.ToString();
        particleSystem = GetComponent<ParticleSystem>();
    }
    public void scoreGoal(){
        score ++;
        textMesh.text = score.ToString();
        particleSystem.Play();
    }

}
