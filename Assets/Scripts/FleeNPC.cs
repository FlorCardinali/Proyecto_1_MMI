using UnityEngine;

public class FleeNPC : MonoBehaviour
{
    public Transform player;
    public float velocidad = 2.5f;
    public float distanciaFlee = 5f;
    public float intervalo = 2f;
    public float distanciaVision = 1.5f;
    public float multiplicadorEvasion = 3f;
    public LayerMask capaObstaculos;

    private Vector2 direccion;
    private float contadorTiempo;

    void Start()
    {
        direccionRandom();
    }

    void Update()
    {
        float distanciaAlPlayer = Vector2.Distance(transform.position, player.position);
        Vector2 velocidad;

        if (distanciaAlPlayer < distanciaFlee)
        {
            velocidad = Flee();
        }
        else
        {
            velocidad = Wander();
        }

        Vector2 fuerzaEvasion = CalcularEvasion(velocidad.normalized);
        Vector2 velocidadActual = velocidad + fuerzaEvasion;
        velocidadActual = Vector2.ClampMagnitude(velocidadActual, this.velocidad);

        transform.Translate(velocidadActual * Time.deltaTime);
    }

    Vector2 Flee()
    {
        Vector2 direccionPlayer = (transform.position - player.position).normalized;
        return direccionPlayer * velocidad;
    }

    Vector2 Wander()
    {
        contadorTiempo += Time.deltaTime;
        if (contadorTiempo >= intervalo)
        {
            direccionRandom();
            contadorTiempo = 0;
        }
        return direccion * (velocidad * 0.7f);
    }

    void direccionRandom()
    {
        float anguloCualquiera = Random.Range(0f, 2f * Mathf.PI);
        direccion = new Vector2(Mathf.Cos(anguloCualquiera), Mathf.Sin(anguloCualquiera));
    }

    Vector2 CalcularEvasion(Vector2 direccionMovimiento)
    {
        if (direccionMovimiento == Vector2.zero) return Vector2.zero;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direccionMovimiento, distanciaVision, capaObstaculos);
        Debug.DrawRay(transform.position, direccionMovimiento * distanciaVision, Color.red);

        if (hit.collider != null)
        {
            Vector2 direccionAlejamiento = (Vector2)transform.position - hit.point;
            return direccionAlejamiento.normalized * multiplicadorEvasion;
        }
        return Vector2.zero;
    }
}