using Unity.Burst.CompilerServices;
using UnityEditor;
using UnityEngine;

public class RaycastDemo : MonoBehaviour
{
    public float RaycastDistance = 10;
    public Texture2D blackTexture; // Background for GUIText
    public Mesh pCylinder;
    public Mesh pCone;


    private float GizmoRadius = 1;
    private Vector3 hitPoint = new Vector3(0,0,0);
    private Vector3 hitNormal = new Vector3(0, 0, 0);
    private Quaternion hitRotation;
    private float hitDistance = 0f;
    private bool hitTrue = false;
    private string hitName = "null";


    // Update is called once per frame
    void Update()
    {
        Raycast();
    }

    // This function performs a raycast and assigns the resultant data to the relevant objects
    void Raycast()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, RaycastDistance))
        {
            hitTrue = true;
            hitPoint = hit.point;
            hitNormal = hit.normal;
            hitRotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
            hitDistance = hit.distance;
            hitName = hit.transform.name; 
        }
        else
        {
            hitTrue = false;
        }
    }

    // This function draws the debug lines in the Editor
    // When Game mode is inactive, hitTrue is false as default
    void OnDrawGizmos()
    {

        if (hitTrue)
        {
            Gizmos.color = Color.cyan;
            //Gizmos.DrawLine(transform.position, (Vector3.down * RaycastDistance) + transform.position);
            Gizmos.DrawMesh(pCylinder, transform.position, Quaternion.LookRotation(Vector3.right, Vector3.down), new Vector3(0.2f, hitDistance, 0.2f));
            Gizmos.color = Color.yellow;
            //Gizmos.DrawLine(hitPoint, hitPoint + (hitNormal * (hitDistance / 2)));
            Gizmos.DrawMesh(pCylinder, hitPoint, hitRotation, new Vector3(0.2f, hitDistance / 2, 0.2f));


            Gizmos.color = Color.cyan;
            Gizmos.DrawMesh(pCone, transform.position, Quaternion.LookRotation(Vector3.right, Vector3.down), Vector3.one);
            Gizmos.color = Color.red;
            Gizmos.DrawMesh(pCone, hitPoint, hitRotation, Vector3.one);
            //Gizmos.DrawSphere(hitPoint, GizmoRadius);
            Gizmos.color = Color.yellow;
            Gizmos.DrawMesh(pCone, hitPoint + (hitNormal * (hitDistance / 2)), hitRotation, Vector3.one);
            //Gizmos.DrawSphere(hitPoint + (hitNormal * (hitDistance / 2)), GizmoRadius);

            GUIStyle styleYellow = new GUIStyle();
            styleYellow.normal.textColor = Color.yellow;
            styleYellow.normal.background = blackTexture;
            Handles.Label(hitPoint + (hitNormal * (hitDistance / 2)) + new Vector3(0, GizmoRadius * 2f, 0), "  "+hitNormal.ToString(), styleYellow);

            GUIStyle styleGreen = new GUIStyle();
            styleGreen.normal.textColor = Color.cyan;
            styleGreen.normal.background = blackTexture;
            Handles.Label(hitPoint + new Vector3(0, hitDistance / 2, 0), "  "+hitDistance.ToString(), styleGreen);

            GUIStyle styleRed = new GUIStyle();
            styleRed.normal.textColor = Color.red;
            styleRed.normal.background = blackTexture;
            Handles.Label(hitPoint + new Vector3(0, GizmoRadius * 2f, 0), "  "+hitPoint.ToString(), styleRed);
            Handles.Label(hitPoint + new Vector3(0, GizmoRadius * 3.5f, 0), "  "+hitName, styleRed);
        }
        else
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(transform.position, GizmoRadius);
        }
    }

}
