using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public PlayerStats playerStats;

    public GameObject buildEffect;

    public TurretBlueprint turretToBuild;

    public bool CanBuild { get { return turretToBuild != null; }}

    public bool HasMoney { get { return playerStats.Money >= turretToBuild.cost; }}

    public void BuildTurretOn(Node node){
        if (playerStats.Money < turretToBuild.cost){
            return;
        }
        playerStats.Money -= turretToBuild.cost;
        GameObject turret = (GameObject)Instantiate(turretToBuild.prefab, node.GetBuildPosition(), Quaternion.identity);
        node.turret = turret;

        //GameObject effect = (GameObject)Instantiate(buildEffect, node.GetBuildPosition(), Quaternion.identity);
        //Destroy(effect, 3f);
    }

    public void SelectTurretToBuild(TurretBlueprint turret){
        turretToBuild = turret;
    }
}
