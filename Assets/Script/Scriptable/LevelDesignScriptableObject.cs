using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Level", order = 1)]
public class LevelDesignScriptableObject : ScriptableObject
{
    // Ball
    public Color Ballcolor;
    public float Ballsize;
    public float Ballmass;
    public float BallSpeed;
    public Vector3 BallStartPosition;
    public int BallCount;

    //player
    public Vector3 playerPosition;
    public Vector3 playerRotation;

    //enviroment
    public Material skyboxMaterial;

    //game
    public int SuccessCount;
}

