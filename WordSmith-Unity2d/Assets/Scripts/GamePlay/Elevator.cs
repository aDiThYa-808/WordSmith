using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float miny;
    [SerializeField] private float maxy;

    private bool movingUp = true;

    private void Update()
    {
        Vector3 position = transform.position;

        if (movingUp)
        {
            position.y += speed * Time.deltaTime;
            if (position.y >= maxy)
            {
                position.y = maxy;
                movingUp = false;
            }
        }
        else
        {
            position.y -= speed * Time.deltaTime;
            if (position.y <= miny)
            {
                position.y = miny;
                movingUp = true;
            }
        }

        transform.position = position;
    }
}
