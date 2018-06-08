using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mace : MonoBehaviour {

    float speed;
    Vector3 direction;
    float min;
    float max;
    float units = 2.0f;

    void Start()
    {
        max = transform.position.y;
        min = transform.position.y - units;

        direction = Vector3.down;
    }

    void Update()
    {

        if (direction == Vector3.down)
        {
            speed = 10.0f;
        }

        else if (direction == Vector3.up)
        {
            speed = 1.0f;
        }


        transform.Translate(direction * speed * Time.deltaTime);

        if (transform.position.y >= max)
        {
            direction = Vector3.down;
        }

        if (transform.position.y <= min)
        {
            direction = Vector3.up;
        }
    }
}
