using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTransform : MonoBehaviour
{

    public Transform transformToFollow;
    public bool rotate = true;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = transformToFollow.position;
        if( rotate ) transform.rotation = Quaternion.AngleAxis( transformToFollow.eulerAngles.y, Vector3.up );
    }
}
