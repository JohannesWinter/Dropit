using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData {
    public string managerVersion;
    public int saveNumber;

    //factory objects
    public string[] fob_names;
    public float[] fob_xPos;
    public float[] fob_yPos;
    public float[] fob_zPos;
    public float[] fob_xRot;
    public float[] fob_yRot;
    public float[] fob_zRot;
    public float[] fob_xScale;
    public float[] fob_yScale;
    public float[] fob_zScale;
    public float[] fob_durabilities;
    public float[] fob_ids;
    public float[] fob_dropTimers;
    public bool[] fob_areScraps;
    public string[] fob_inputOres_serialized;

    //ores
    public int[] ore_numbers;
    public int[] ore_upgradeLevers;
    public float[] ore_values;
    public float[] ore_baseValues;
    public float[] ore_xPos;
    public float[] ore_yPos;
    public float[] ore_zPos;
    public float[] ore_xRot;
    public float[] ore_yRot;
    public float[] ore_zRot;
    public string[] ore_visitedBeltsLists_serialized;
    public Vector3[] ore_veloc;
    public Vector3[] ore_trc;
}
