using RicoShot.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSwitcher : MonoBehaviour
{
    [SerializeField] private GameObject alphaBullet;
    [SerializeField] private GameObject bravoBullet;

    public Renderer ApplyBulletColor(Team team)
    {
        var alphaRenderer = alphaBullet.GetComponent<Renderer>();
        alphaRenderer.enabled = team == Team.Alpha;
        var bravoRenderer = bravoBullet.GetComponent<Renderer>();
        bravoRenderer.enabled = team == Team.Bravo;
        return team == Team.Alpha ? alphaRenderer : bravoRenderer;
    }
}
