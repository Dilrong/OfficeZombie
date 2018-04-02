using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartcleManager : MonoBehaviour {
	void Update () {
        if (!GetComponent<ParticleSystem>().isPlaying)
            Destroy(gameObject);
    }
}
