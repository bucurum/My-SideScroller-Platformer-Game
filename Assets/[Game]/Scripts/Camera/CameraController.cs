using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private PlayerMovementHandler player;

    public BoxCollider2D boundsBox;
    private float halfHeight;
    private float halfWidth;

    void Start()
    {
        player =  PlayerHealthController.instance.GetComponent<PlayerMovementHandler>();
        halfHeight = Camera.main.orthographicSize;
        halfWidth = halfHeight * Camera.main.aspect;
    }

    void Update()
    {
        if(player == null) return;

        transform.position = new Vector3(
        Mathf.Clamp(player.transform.position.x, boundsBox.bounds.min.x + halfWidth, boundsBox.bounds.max.x - halfWidth),
        Mathf.Clamp(player.transform.position.y, boundsBox.bounds.min.y + halfHeight, boundsBox.bounds.max.y - halfHeight),
        transform.position.z);
    
        
    }
}
