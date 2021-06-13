using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsometricZ : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // relate the z-position of the object to the y-position, but scaled with a large number so we don't accidentally overstep the camera bounds (-10)
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y / 100);
    }
}
