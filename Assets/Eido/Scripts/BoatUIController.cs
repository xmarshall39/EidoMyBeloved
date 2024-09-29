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
    [Space]
    public TextMeshProUGUI scoreText, timeText;

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
            shapeLeft.gameObject.SetActive(false);
        }

        shapeRight.gameObject.SetActive(true);
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
            shapeRight.gameObject.SetActive(false);
        }

        shapeLeft.gameObject.SetActive(true);
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
            colorLeft.gameObject.SetActive(false);
        }

        colorRight.gameObject.SetActive(true);
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
            colorRight.gameObject.SetActive(false);
        }

        colorLeft.gameObject.SetActive(true);
        playerController.colorTarget = CurrentColor;
    }

    private void Start()
    {
        GameStateManager.OnTimerUpdate += GameStateManager_OnTimerUpdate;
    }

    private void GameStateManager_OnTimerUpdate(int time)
    {
        timeText.text = $"Time: {time}";
        scoreText.text = $"Score: {GameStateManager.Instance.Score}";
    }
}
