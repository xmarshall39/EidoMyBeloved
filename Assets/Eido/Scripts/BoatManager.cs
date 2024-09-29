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

public class BoatSpawnEntry
{
    public int Timestamp = 5;
    public int StartLocation = 2;
    public int BoatShape = 0;
    public int BoatColor = 1;
    public int BoatPrefab = 0;
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

    public Transform spawnAcnchor;
    public TextAsset boatSpawnScript;
    public GameObject[] boatPrefabs;
    public Transform[] boatStartPoints;

    public List<Boat> LivingBoats { get; private set; } = new List<Boat>();
    private Queue<BoatSpawnEntry> spawns = new Queue<BoatSpawnEntry>();

    /// <summary>
    /// Our spreadsheet is 1 indexed, and 0 refers to null
    /// </summary>
    public void ReadBoatSequence()
    {
        string[] lines = boatSpawnScript.ToString().Split("\n");
        int i = 0;
        foreach(var line in lines)
        {
            if (i != 0)
            {
                string[] cols = line.Split(",");
                if (cols.Length == 5)
                {
                    var newboat = new BoatSpawnEntry()
                    {
                        Timestamp = int.Parse(cols[0]),
                        StartLocation = int.Parse(cols[1]) - 1,
                        BoatShape = int.Parse(cols[2]),
                        BoatColor = int.Parse(cols[3]),
                        BoatPrefab = int.Parse(cols[4]) - 1
                    };

                    spawns.Enqueue(newboat);
                }
            }

            ++i;
        }
    }

    void Start()
    {
        ReadBoatSequence();
        GameStateManager.OnTimerUpdate += GameStateManager_OnTimerUpdate;
    }

    private void GameStateManager_OnTimerUpdate(int time)
    {
        if (spawns.TryPeek(out BoatSpawnEntry entry) && entry.Timestamp <= time)
        {
            SpawnBoat(spawns.Dequeue());
        }
    }

    public void SpawnBoat(BoatSpawnEntry entry)
    {
        var boatObj = GameObject.Instantiate(boatPrefabs[entry.BoatPrefab], boatStartPoints[entry.StartLocation]);
        if (boatObj.TryGetComponent(out Boat boatComp))
        {
            boatComp.InitBoat(entry); //I need a light in here...
                                      // set icon color... boatComp.shapeIcon
            LivingBoats.Add(boatComp);
        }

    }

    public void SetMovementDirection(BoatColor targetColor, BoatShape targetShape, Vector3 mov)
    {
        LivingBoats.ForEach(x => { if ((x.shape == targetShape && x.shape != 0) || (x.color == targetColor && x.color != 0)) x.SetMovementDirection(mov); });
    }

    public void SpeedUp(BoatColor targetColor, BoatShape targetShape) => LivingBoats.ForEach(x => { if ((x.shape == targetShape && x.shape != 0) || (x.color == targetColor && x.color != 0)) x.SpeedUp(); });
    public void ResetSpeed(BoatColor targetColor, BoatShape targetShape) => LivingBoats.ForEach(x => { if ((x.shape == targetShape && x.shape != 0) || (x.color == targetColor && x.color != 0)) x.ResetSpeed(); });
    public void SlowDown(BoatColor targetColor, BoatShape targetShape) => LivingBoats.ForEach(x => { if ((x.shape == targetShape && x.shape != 0) || (x.color == targetColor && x.color != 0)) x.SlowDown(); });

}
