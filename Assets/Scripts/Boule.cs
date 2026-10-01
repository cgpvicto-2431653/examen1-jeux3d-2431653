using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

/// <summary>
/// Objet représentant une boule contrôlée par le joueur.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class Boule : MonoBehaviour
{
    [SerializeField, Tooltip("La cible pour le suvi de la caméra")]
    private Transform cibleCamera;

    [SerializeField, Tooltip("Force de déplacement de la boule.")]
    private float forceDeplacement;

    [SerializeField, Tooltip("Force de la pousser grace a l'objet acceleration.")]
    private float forceAcceleration = 15f;

    // Force appliquée à la boule pour le déplacement à chaque frame.
    private Vector3 forceAppliquee;

    // Référence au Rigidbody de la boule pour appliquer la physique.
    private Rigidbody rigidbody;

    private bool jeuCommencer= false;

    private int chargesAcceleration = 0;
    private bool estEnAcceleration = false;

    public int ChargerAcceleration => chargesAcceleration;

    /// <summary>
    /// Obtient la vélocité actuelle de la boule.
    /// </summary>
    public Vector3 Velocite => rigidbody.linearVelocity;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();

        rigidbody.useGravity = true;

        if (ControleurJeu.Instance != null && ControleurJeu.Instance.Controles != null) 
        {
            ControleurJeu.Instance.Controles.actions.FindAction("Commencer").performed += CommencerJeu;
        }
    }

    private void OnDestroy()
    {
        if (ControleurJeu.Instance == null || ControleurJeu.Instance.Controles == null)
            return;

        PlayerInput controles = ControleurJeu.Instance.Controles;

        controles.actions.FindAction("Commencer").performed -= CommencerJeu;
        controles.actions.FindAction("Diriger").performed -= CommencerDirection;
        controles.actions.FindAction("Diriger").canceled -= ArreterDirection;
        controles.actions.FindAction("Accelerer").canceled -= UtiliserAcceleration;

    }

    private void CommencerJeu(InputAction.CallbackContext contexte)
    {
        if (jeuCommencer) 
        {
            return;
        }

        PlayerInput controles = ControleurJeu.Instance.Controles;

        controles.actions.FindAction("Commencer").performed -= CommencerJeu;

        jeuCommencer = true;
        rigidbody.useGravity = true;


        controles.actions.FindAction("Diriger").performed += CommencerDirection;
        controles.actions.FindAction("Diriger").canceled += ArreterDirection;
        controles.actions.FindAction("Accelerer").canceled += UtiliserAcceleration;

    }

    private void Update()
    {
        if (cibleCamera != null)
        {
            cibleCamera.position = rigidbody.position;
        }
    }

    private void FixedUpdate()
    {
        if (jeuCommencer)
        {
            Diriger();
        }
    }

    private void CommencerDirection(InputAction.CallbackContext contexte)
    {
        forceAppliquee += contexte.ReadValue<float>() * forceDeplacement * Vector3.right;
    }

    private void ArreterDirection(InputAction.CallbackContext contexte)
    {
        forceAppliquee = Vector3.zero;
    }

    private void Diriger()
    {
        if(!Mathf.Approximately(forceAppliquee.sqrMagnitude, 0.0f))
        {
            rigidbody.AddForce(forceAppliquee, ForceMode.Force);
        }
    }

    public void AjouterCharge()
    {
        if (chargesAcceleration < 3)
        {
            chargesAcceleration++;
        }
    }

    private void UtiliserAcceleration(InputAction.CallbackContext context)
    {
        Debug.Log($"touche w appuyer, charge actuelles : {chargesAcceleration}");
        if (chargesAcceleration > 0 && !estEnAcceleration)
        {
            StartCoroutine(RoutineAcceleration());
        }
    }

    private IEnumerator RoutineAcceleration()
    {
        estEnAcceleration = true;
        chargesAcceleration--;

        float tempEcoule = 0f;
        while (tempEcoule < 1.0f)
        {
            rigidbody.AddForce(Vector3.forward * forceAcceleration, ForceMode.Acceleration);
            tempEcoule += Time.deltaTime;
            yield return null;
        }
        estEnAcceleration = false;
    }

}
