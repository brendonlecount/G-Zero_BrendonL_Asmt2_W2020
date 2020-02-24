using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Brendon LeCount 2/11/2020
// This utility script disables the mesh renderer for whatever its attached to upon play.

public class HideOnPlay : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
		GetComponent<MeshRenderer>().enabled = false;
    }
}
