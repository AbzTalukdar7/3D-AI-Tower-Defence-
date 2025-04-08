using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour
{
    public BuildManager buildManager;

    public Color hoverColor;
    public Vector3 positionOffset;

    public GameObject turret;

    private Renderer rend;
    private Color startColor;
    public Color alarmColor;

    public string pathTag = "Path"; // The tag assigned to your path objects
    public float detectionRange = 15f;
    public float weight = 0f;

    void Start()
    {
        rend = GetComponent<Renderer>();
        startColor = rend.material.color;
        DetectPathObjectsWithTag();
    }

    void DetectPathObjectsWithTag()
    {
        Collider[] detectedObjects = Physics.OverlapSphere(transform.position, detectionRange);
        foreach (var obj in detectedObjects)
        {
            if (obj.CompareTag(pathTag))
            {
                weight += 1f;
            }
        }
    }

    public Vector3 GetBuildPosition(){
        return transform.position + positionOffset;
    }

    public GameObject GetTurret(){
        return turret;
    }

    void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (!buildManager.CanBuild)
            return;

        if (turret != null){
            return;
        }
        buildManager.BuildTurretOn(this);
    }
    void OnMouseEnter()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (!buildManager.CanBuild)
            return;
        
        if (buildManager.HasMoney){
            rend.material.color = hoverColor;
        }else{
            rend.material.color = alarmColor;
        }
        
    }

    void OnMouseExit()
    {
        rend.material.color = startColor;
    }

    public int nodeState(){
        if (turret == null){
            return 0;
        }
        else{
            return 1;
        }
    }

    public void ResetNode(){
        Destroy(turret);
        turret = null;
    }
}
