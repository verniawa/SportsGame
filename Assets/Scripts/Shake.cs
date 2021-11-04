using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake : MonoBehaviour {
    public IEnumerator cameraShake(float duration, float magnitude){
        Vector3 initialPosition = transform.localPosition;
        float elapsed = 0f;
        float trauma = magnitude;
        float seed = Random.value * 50;
        while (elapsed < duration){
            trauma = Mathf.Lerp(magnitude, 0f, elapsed / duration);
            float x = Mathf.Lerp(-1f, 1f, Mathf.PerlinNoise(seed + elapsed * 10f, 0f)) * trauma; 
            float y = Mathf.Lerp(-1f, 1f, Mathf.PerlinNoise(0f, seed + elapsed * 10f)) * trauma;
            
            transform.localPosition = new Vector3(x, y, initialPosition.z);
            elapsed += Time.deltaTime;

            yield return null;
        }
        transform.localPosition = initialPosition;
    }
}
