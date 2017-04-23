using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Shake : MonoBehaviour {
    public static Shake instance;
    public Vector3 startTransform;

    [Space()]
    public AudioClip[] rumbles;

    private int spikes = 0;

    void Awake() {
        instance = this;
        startTransform = transform.position;
    }
    public void shake(float t, float strength) {
        StartCoroutine(screenshake(t, strength));

    }

    IEnumerator screenshake(float t, float strength) {
        float z = transform.position.z;
        while (t > 0) {
            t -= Time.deltaTime * 10;

            transform.position = new Vector2(startTransform.x, startTransform.y) + Random.insideUnitCircle * strength / 8;
            transform.position += new Vector3(0, 0, z);
            yield return null;
        }
        transform.position = startTransform;
    }
}
