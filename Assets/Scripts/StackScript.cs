using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackScript : MonoBehaviour
{
    public GameObject Campivot;

    public List<BlockPrefab> blocks;

    private float mouseorigin;
    private bool rot = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // For rotating the camera around the stack, first we store its origin, and then rotate accordingly.
        if (Input.GetMouseButton(0)&& rot && (Input.mousePosition.x - mouseorigin) != 0){
            
            Campivot.transform.rotation *= Quaternion.Euler(new Vector3(0, (Input.mousePosition.x - mouseorigin) / 2, 0));
            mouseorigin = Input.mousePosition.x;
        }

        if (Input.GetMouseButtonDown(0))
        {
            mouseorigin = Input.mousePosition.x;
            rot = true;
        }

        if (Input.GetMouseButtonUp(0)) {
            rot = false;
        }
    }
}
