using UnityEngine;
using System.Collections;

public class HealthBar : MonoBehaviour {

    Component Bar;

    void Start() {
        Bar = GetComponent("bar");
    }

	public void UpdateHealthPercentage () {
        
	}
}
