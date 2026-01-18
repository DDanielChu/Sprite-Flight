using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{   

    private float elapsedTime = 0f;

    private float score = 0f;

    private Label scoreText;

    private Button restartButton;

    public float scoreMultiplier = 10f;

    public float thrust = 1f;

    public float maxSpeed = 100f;

    public GameObject boosterFlame;

    public GameObject explosionEffect;

    public UIDocument uiDocument;

    Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        scoreText = uiDocument.rootVisualElement.Q<Label>("ScoreLabel");

        restartButton = uiDocument.rootVisualElement.Q<Button>("RestartButton");
        restartButton.style.display = DisplayStyle.None;
        restartButton.clicked += ReloadScene;
    }

    // Update is called once per frame
    void Update()
    {

        elapsedTime += Time.deltaTime;

        score = Mathf.FloorToInt(elapsedTime * scoreMultiplier);

        scoreText.text = "Score: " + score;

     if (Mouse.current.leftButton.isPressed)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.value);

            Vector2 direction = (mousePos - transform.position).normalized;
            transform.up = direction;

            rb.AddForce(direction * thrust);
        }   

        if (rb.linearVelocity.magnitude > maxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized*maxSpeed;
        }


        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            boosterFlame.SetActive(true);
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            boosterFlame.SetActive(false);
        }

        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);

        GameObject boom = Instantiate(explosionEffect, transform.position, transform.rotation);
        ParticleSystem ps = boom.GetComponent<ParticleSystem>();
        
        var main = ps.main;

        main.startSizeMultiplier = 1.1f;

        ps.Play();
        

        restartButton.style.display = DisplayStyle.Flex;

    }

    void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
