using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private static PlayerController instance;
    public float Speed = 10;
    [SerializeField]
    private int direction = 0;
    public bool Move = false;
    [SerializeField]
    private bool falling = false;

    void Update()
    {
        var y = falling ? -10 : 0;
        if (Move)
        {
            var dt = Time.deltaTime;
            if (direction == Const.LEFT)
                transform.Translate(0, dt * y, dt * Speed);
            else if (direction == Const.RIGHT)
                transform.Translate(dt * Speed, dt * y, 0);

        }
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            Move = true;
            Turn();
        }
    }

    private void FixedUpdate()
    {
        var ray = new Ray(transform.position, Vector3.down);

        Debug.DrawRay(transform.position, Vector3.down);
        RaycastHit info;
        var hit = Physics.Raycast(transform.position, Vector3.down, out info);
        if (Move && !hit)
        {
            Fall();
        }
        else
        {
            //print(info.transform.tag);

        }
    }

    public void Turn()
    {
        print("turn");
        direction = (direction + 1) % 2;
    }

    private void Fall()
    {
        print("fall");
        falling = true;
        Invoke("Lose", 1);
    }

    private void Lose()
    {
        GameManager.Instance.GameOver();
        Destroy(gameObject);
    }
    int passed = 0;
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Tile")
        {
            var tile = other.GetComponent<Tile>();
            GameManager.Instance.TilePassed(tile);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Tile")
        {
            var tile = other.GetComponent<Tile>();
            if(tile.FirstInSector)
                GameManager.Instance.SectorPassed();
        }
    }
}
