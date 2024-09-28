using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public BoatColor colorTarget = BoatColor.None;
    public BoatShape shapeTarget = BoatShape.None;

    void Update()
    {
        if(GameStateManager.Instance.State == GameState.InGame)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                BoatManager.Instance.SetMovementDirection(colorTarget, shapeTarget, Vector3.left * 75);
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                BoatManager.Instance.SpeedUp(colorTarget, shapeTarget);

            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                BoatManager.Instance.SetMovementDirection(colorTarget, shapeTarget, Vector3.right * 75);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                BoatManager.Instance.SlowDown(colorTarget, shapeTarget);
            }
        }
    }
}
