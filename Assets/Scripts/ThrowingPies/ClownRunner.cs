using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClownRunner : MonoBehaviour, IHittableByPie
{
    public float speed = 2f;

    void Update()
    {
        transform.position += Vector3.right * speed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall"))
        {
            speed *= -1f;
            FlipSprite();
        }
    }

    void FlipSprite()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1f;
        transform.localScale = scale;
    }

    public void HitByPie()
    {
        Debug.Log("Runner clown got pied!");
        Destroy(gameObject);
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
    void LateUpdate()
    {
        float verticalSize = Camera.main.orthographicSize;
        float horizontalSize = verticalSize * Camera.main.aspect; // Å© USE THIS


        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, -horizontalSize + 0.5f, horizontalSize - 0.5f);
        transform.position = pos;
    }

}


