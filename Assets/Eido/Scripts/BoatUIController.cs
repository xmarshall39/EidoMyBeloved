using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BoatUIController : MonoBehaviour
{
    public PlayerController playerController;
    public Button colorLeft, colorRight, shapeLeft, shapeRight;
    public Image shapeTargetImage;
    public TextMeshProUGUI colorText;

    [Header("Settings")]
    public Color[] colors; //These may not actually be red, blue, green
    public Sprite[] shapes;


    public BoatColor CurrentColor { get; private set; } = BoatColor.None;
    public BoatShape CurrentShape { get; private set; } = BoatShape.None;

    public void ScrollShapeLeft()
    {
        if (CurrentShape > 0)
        {
            CurrentShape -= 1;
        }

        shapeTargetImage.sprite = shapes[(int)CurrentShape];

        if (CurrentShape == BoatShape.None)
        {
            shapeLeft.enabled = false;
        }

        playerController.shapeTarget = CurrentShape;
    }

    public void ScrollShapeRight()
    {
        if ((int)CurrentShape < 3)
        {
            CurrentShape += 1;
        }

        shapeTargetImage.sprite = shapes[(int)CurrentShape];

        if (CurrentShape == BoatShape.Circle)
        {
            colorRight.enabled = false;
        }

        playerController.shapeTarget = CurrentShape;
    }

    public void ScrollColorLeft()
    {
        if (CurrentColor > 0)
        {
            CurrentColor -= 1;
        }

        colorText.color = colors[(int)CurrentColor];

        if (CurrentColor == BoatColor.None)
        {
            colorLeft.enabled = false;
        }

        playerController.colorTarget = CurrentColor;

    }

    public void ScrollColorRight()
    {
        if ((int)CurrentColor < 3)
        {
            CurrentColor += 1;
        }

        colorText.color = colors[(int)CurrentColor];

        if (CurrentColor == BoatColor.Blue)
        {
            colorRight.enabled = false;
        }

        playerController.colorTarget = CurrentColor;
    }

    void Start()
    {
        
    }

   
}
