using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ObjetAcceleration : MonoBehaviour
{
    private void OnTriggerEntrer(Collider other)
    {
        if (other.CompareTag("Boule"))
        {
            Boule boule = other.GetComponent<Boule>();
            if (boule != null )
            {
                boule.AjouterCharge();

                Destroy(boule);
            }
        }
    }
}
