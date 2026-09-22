using UnityEngine;

public class SeekNPC : MonoBehaviour
{
    public Transform player;
    public float velocidadMaxima = 3f;
    public float distanciaParaPerseguir = 6f;
    public float radioDeFrenado = 1.5f;
    public float intervalo = 2f;
    public float distanciaVision = 1.5f;
    public float multiplicadorEvasion = 3f;
    public LayerMask capaObstaculos;

    private Vector2 direccionRandom;
    private float contadorTiempo;
    public float distanciaDeParada = 0.8f;

    void Start()
    {
        ElegirDireccionRandom();
    }

    void Update()
    {
        float distanciaAlPlayer = Vector2.Distance(transform.position, player.position);
        Vector2 velocidad;

        if (distanciaAlPlayer < distanciaParaPerseguir)
        {
            velocidad = SeekYParar(distanciaAlPlayer);
        }
        else
        {
            velocidad = Wander();
        }

        Vector2 fuerzaEvasion = CalcularEvasion(velocidad.normalized);
        Vector2 velocidadActual = velocidad + fuerzaEvasion;
        velocidadActual = Vector2.ClampMagnitude(velocidadActual, velocidadMaxima);

        transform.Translate(velocidadActual * Time.deltaTime);
    }

    Vector2 SeekYParar(float distancia)
    {
        if (distancia <= distanciaDeParada)
        {
            return Vector2.zero;
        }

        Vector2 velocidad = (player.position - transform.position).normalized * velocidadMaxima;
        if (distancia < radioDeFrenado)
        {
            velocidad *= (distancia / radioDeFrenado);
        }
        return velocidad;
    }

    Vector2 Wander()
    {
        contadorTiempo += Time.deltaTime;
        if (contadorTiempo >= intervalo)
        {
            ElegirDireccionRandom();
            contadorTiempo = 0;
        }
        return direccionRandom * (velocidadMaxima * 0.5f);
    }

    void ElegirDireccionRandom()
    {
        float anguloAleatorio = Random.Range(0f, 2f * Mathf.PI);
        direccionRandom = new Vector2(Mathf.Cos(anguloAleatorio), Mathf.Sin(anguloAleatorio));
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