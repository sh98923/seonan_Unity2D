using UnityEngine;

public class CharacterCollision : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Enemy"))
        {
            Vector3 curCharacterPos = transform.position;
            curCharacterPos.x = 0;
            transform.position = curCharacterPos;

            Debug.Log("Ãæµ¹");
        }
    }
}
