using UnityEngine;

public class MiniGameBullet : MonoBehaviour
{
    private Vector2 _velocity;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    public void Init(Vector2 direction, float speed)
    {
        _velocity = direction.normalized * speed;
    }
    // Update is called once per frame
    private void Update()
    {
        transform.Translate(_velocity * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ConversationTimer.Instance.ReduceTime(1f); // hard coded for now
            Destroy(gameObject);
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
