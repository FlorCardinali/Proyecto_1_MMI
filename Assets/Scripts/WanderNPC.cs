using UnityEngine;

public class WanderNPC : MonoBehaviour
{
    public float velocidad = 2f;
    public float intervalo = 2f;
    public float distanciaVision = 1.5f;
    public float multiplicadorEvasion = 3f;
    public LayerMask capaObstaculos;

    private Vector2 direccion;
    private float contadorTiempo;

    void Start()
    {
        ElegirDireccionRandom();
    }

    void Update()
    {
        contadorTiempo += Time.deltaTime;
        if (contadorTiempo >= intervalo)
        {
            ElegirDireccionRandom();
            contadorTiempo = 0;
        }

        Vector2 fuerzaEvasion = CalcularEvasion(direccion);
        Vector2 velocidadActual = (direccion * velocidad) + fuerzaEvasion;
        velocidadActual = Vector2.ClampMagnitude(velocidadActual, velocidad);

        transform.Translate(velocidadActual * Time.deltaTime);
    }

    void ElegirDireccionRandom()
    {
        float anguloCualquiera = Random.Range(0f, 2f * Mathf.PI);
        direccion = new Vector2(Mathf.Cos(anguloCualquiera), Mathf.Sin(anguloCualquiera));
    }

    Vector2 CalcularEvasion(Vector2 direccionMovimiento)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direccionMovimiento.normalized, distanciaVision, capaObstaculos);
        Debug.DrawRay(transform.position, direccionMovimiento.normalized * distanciaVision, Color.red);

        if (hit.collider != null)
        {
            Vector2 direccionAlejamiento = (Vector2)transform.position - hit.point;
            return direccionAlejamiento.normalized * multiplicadorEvasion;
        }
        return Vector2.zero;
    }
}