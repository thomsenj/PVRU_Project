using UnityEngine;
using UnityEngine.Splines;
using Unity.Mathematics;  // Für float3 und math

public class ExtendableSpline : MonoBehaviour
{
    public SplineContainer splineContainer;  // Das SplineContainer-Objekt
    public float addPointInterval = 5f;      // Intervall in Sekunden zum Hinzufügen neuer Punkte
    public float extensionDistance = 10f;    // Entfernung, um die die Spline verlängert wird
    private float timer;

    void Start()
    {
        if (splineContainer == null)
        {
            splineContainer = GetComponent<SplineContainer>();
        }
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= addPointInterval)
        {
            AddControlPoint();
            timer = 0f;
        }
    }

    void AddControlPoint()
    {
        // Ermittle den letzten Kontrollpunkt der Spline
        int pointCount = splineContainer.Spline.Count;
        if (pointCount == 0) return;

        BezierKnot lastPoint = splineContainer.Spline[pointCount - 1];

        // Bestimme die Position des neuen Kontrollpunkts basierend auf dem letzten Kontrollpunkt
        Vector3 lastPosition = (Vector3)lastPoint.Position;
        Vector3 newPointPosition = new Vector3(lastPosition.x + extensionDistance, lastPosition.y, lastPosition.z);

        // Erstelle einen neuen Kontrollpunkt
        var newPoint = new BezierKnot(newPointPosition);

        // Füge den neuen Kontrollpunkt zur Spline hinzu
        splineContainer.Spline.Add(newPoint);
    }
}
