using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boat : MonoBehaviour
{
    private Vector3 lastMoveDir = Vector3.left;

    private float rotationProgress = 0;
    private float lastY = 0;
    private float nextY = 0;

    public MeshRenderer[] shapeIcons;
    

    public Vector3 moveDir = Vector3.left;
    public float baseSpeed = 10;
    public float currentSpeed = 10;

    public float rotationSpeed = 2;

    public BoatShape shape = BoatShape.Circle;
    public BoatColor color = BoatColor.Red;


    public void InitBoat(BoatSpawnEntry entry)
    {
        for(int i = 0; i < shapeIcons.Length; ++i)
        {
            if (i == entry.BoatShape - 1)
            {
                shapeIcons[i].gameObject.SetActive(true);
                //[TODO] Handle Icon Color
            }
            else shapeIcons[i].gameObject.SetActive(false);
        }

        //[TODO] Handle Boat Color...

        shape = (BoatShape)entry.BoatShape;
        color = (BoatColor)entry.BoatColor;
    }

    public void SetMovementDirection(Vector3 dir)
    {
        lastMoveDir = new Vector3(transform.localEulerAngles.x, 0, 0);
        float target = transform.eulerAngles.y;
        if (transform.eulerAngles.y > 45) target -= 360;
        lastY = Mathf.Clamp(target, -45, 45);
        nextY = dir == Vector3.up ? 45 : -45;
        moveDir = dir;
        rotationProgress = 0;
    }

    public void SpeedUp() => currentSpeed = baseSpeed * 1.5f;
    public void ResetSpeed() => currentSpeed = baseSpeed;
    public void SlowDown()
    {
        if (currentSpeed == baseSpeed * 1.5) currentSpeed = baseSpeed;
        else currentSpeed = baseSpeed / 2;
    }

    public void Crash()
    {
        GameStateManager.Instance.Score -= 1;
        Destroy(this.gameObject);
    }

    public void Score()
    {
        GameStateManager.Instance.Score += 1;
        Destroy(this.gameObject);
    }

    void Update()
    {
        if (!GameStateManager.Instance.TimePaused)
        {
            // Move forward at all times
            transform.position += transform.forward * currentSpeed * Time.deltaTime;
            if (nextY != transform.localEulerAngles.y)
            {
                transform.eulerAngles = new Vector3(0, Mathf.Clamp(Mathf.LerpAngle(lastY, nextY, rotationProgress), -45, 45), 0);
                //transform.rotation = Quaternion.Slerp(Quaternion.Euler(lastMoveDir), Quaternion.Euler(moveDir), rotationProgress);
                rotationProgress += Time.deltaTime * rotationSpeed;
            }
        }
    }
}
