using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Goal : MonoBehaviour {
    
    int score = 0;
    public TextMeshProUGUI textMesh;
    private void Start() {
        textMesh.text = score.ToString();
    }
    public void scoreGoal(){
        score ++;
        textMesh.text = score.ToString();
    }

}
