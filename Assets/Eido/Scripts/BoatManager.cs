using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum BoatColor
{
    None, Red, Green, Blue
}

public enum BoatShape
{
    None, Triangle, Square, Circle
}

/// <summary>
/// Spawn boats and keep track of them
/// </summary>
public class BoatManager : MonoBehaviour
{
    #region Singleton
    private static BoatManager _instance;
    public static BoatManager Instance { get => _instance; }
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    #endregion

    public GameObject[] boatPrefabs;
    public Transform[] boatStartPoints;

    public List<Boat> LivingBoats { get; private set; }

    void Start()
    {
        LivingBoats = FindObjectsOfType<Boat>().ToList();
    }

    //needs boat type and spawn point
    public void SpawnBoat()
    {
        var boatObj = GameObject.Instantiate(boatPrefabs[0], boatStartPoints[0]);
        if (boatObj.TryGetComponent(out Boat boatComp))
        {

        }
    }

    public void SetMovementDirection(BoatColor targetColor, BoatShape targetShape, Vector3 mov)
    {
        LivingBoats.ForEach(x => { if (x.shape == targetShape || x.color == targetColor) x.SetMovementDirection(mov); });
    }

    public void SpeedUp(BoatColor targetColor, BoatShape targetShape) => LivingBoats.ForEach(x => { if (x.shape == targetShape || x.color == targetColor) x.SpeedUp(); });
    public void ResetSpeed(BoatColor targetColor, BoatShape targetShape) => LivingBoats.ForEach(x => { if (x.shape == targetShape || x.color == targetColor) x.ResetSpeed(); });
    public void SlowDown(BoatColor targetColor, BoatShape targetShape) => LivingBoats.ForEach(x => { if (x.shape == targetShape || x.color == targetColor) x.SlowDown(); });

}
