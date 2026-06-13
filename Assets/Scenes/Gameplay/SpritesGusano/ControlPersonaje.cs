using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControlPersonaje : MonoBehaviour
{
    //VARIABLES
    public float speed;
    public float jumpForce;

    private float move;
    private bool saltando;
    
    //VARIABLES PARA EL LIMITE DE SALTOS
    [SerializeField] private Transform sensorSuelo;
    [SerializeField] private float radioCirculo = 1f;
    [SerializeField] private LayerMask suelo;
    [SerializeField] private bool personajeEnSuelo;
    
    //VARIABLES TIPO ESTRUCTURAS
    Rigidbody2D rb2D;     
    Animator animator;
    public InputSystem_Actions acciones;
    
    void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        acciones = new InputSystem_Actions();
    }

    void OnEnable()
    {
        acciones.Player.Enable();

        acciones.Player.Move.performed += movimiento;
        acciones.Player.Move.canceled += movimiento;

        acciones.Player.Jump.performed += salto;
    }
    
    void OnDisable()
    {
        acciones.Player.Move.performed -= movimiento;
        acciones.Player.Move.canceled -= movimiento;
        
        acciones.Player.Jump.performed -= salto;
        acciones.Player.Jump.canceled-= salto;
        
        acciones.Player.Disable();
        
    }

    
    void movimiento(InputAction.CallbackContext ctx)
    {
        move = ctx.ReadValue<Vector2>().x;
    }

    void salto(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && personajeEnSuelo)
        {
            rb2D.linearVelocityY = jumpForce;
        }
    }
    
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(sensorSuelo.position, radioCirculo);
    }

    void Update()
    {
        personajeEnSuelo = Physics2D.OverlapCircle(sensorSuelo.position, radioCirculo, suelo);
    }
    private void FixedUpdate()
    {
        rb2D.linearVelocityX = move * speed;
    }
}
