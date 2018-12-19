using UnityEngine;

public class Sensor : MonoBehaviour
{
    public float DistanciaMaxima;
    public float Distancia = 0.0f;
    public Vector3 VectorDireccion;
    
    void Awake ()
    {
        VectorDireccion = DistanciaMaxima * transform.forward;
        DistanciaMaxima = 50.0f;
    }
	
	void FixedUpdate ()
    {
        Ray ray = new Ray(transform.position, DistanciaMaxima * transform.forward);
        RaycastHit hit;       

        if (Physics.Raycast(ray, out hit, DistanciaMaxima))
        {
            Debug.DrawRay(ray.origin, DistanciaMaxima * ray.direction, Color.red);
            Distancia = hit.distance;
        }
        else
        {
            Debug.DrawRay(ray.origin, DistanciaMaxima * ray.direction, Color.green);
            Distancia = 50.0f;
        }
    }

    public float GetDistancia()
    {
        return Distancia / DistanciaMaxima;
    }
}
