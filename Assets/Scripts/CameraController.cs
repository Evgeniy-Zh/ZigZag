using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private GameManager gameManager;
    [SerializeField]
    private Transform _target;
    private Transform target
    {
        get
        {
            if (_target == null)
                _target = GameManager.Instance.player.transform;
            return _target;
        }
        set
        {
            _target = value;
        }
    }

    // Use this for initialization
    void Start()
    {
        gameManager = GameManager.Instance;
        gameManager.OnGameStarted += Init;
    }

    private void Init()
    {
        var p = transform.position;
        p.x = target.position.x;
        p.z = target.position.z;
        transform.position = p;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.IsGameOver) return;
        var p = transform.position;
        p.x = target.position.x;
        p.z = target.position.z;
        transform.position = p;
    }

    private void OnDestroy()
    {
        gameManager.OnGameStarted -= Init;
    }
}
