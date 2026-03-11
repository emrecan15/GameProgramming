using UnityEngine;
using UnityEngine.InputSystem;

public class CarController : MonoBehaviour
{
	[Header("Temel Hareket")]
	public float laneDistance = 3.0f;
	public float forwardSpeed = 20.0f;
	public float laneChangeSpeed = 10.0f;

	[Header("Zorluk (Hýzlanma) Ayarlarý")]
	public float maxSpeed = 50.0f;
	public float acceleration = 0.5f;

	[Header("Dönüþ Ayarlarý")]
	public float turnAngle = 15.0f;
	public float turnSpeed = 15.0f;

	private int currentLane = 1; // 0: Sol, 1: Orta, 2: Sað

	void Update()
	{
		// 1. ZORLUK ARTIRIMI (Ývmelenme)
		if (forwardSpeed < maxSpeed)
		{
			forwardSpeed += acceleration * Time.deltaTime;
		}

		// 2. KONTROLLER (Input System)
		if (Keyboard.current != null)
		{
			if (Keyboard.current.rightArrowKey.wasPressedThisFrame || Keyboard.current.dKey.wasPressedThisFrame)
			{
				currentLane++;
				if (currentLane > 2) currentLane = 2;
			}

			if (Keyboard.current.leftArrowKey.wasPressedThisFrame || Keyboard.current.aKey.wasPressedThisFrame)
			{
				currentLane--;
				if (currentLane < 0) currentLane = 0;
			}
		}

		// 3. ÝLERÝ GÝDÝÞ (Sabit Hýz)
		transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime, Space.World);

		// 4. SAÐA/SOLA GEÇÝÞ (Yumuþak Hýz - X Ekseni)
		float targetX = (currentLane - 1) * laneDistance;
		float smoothX = Mathf.Lerp(transform.position.x, targetX, laneChangeSpeed * Time.deltaTime);
		transform.position = new Vector3(smoothX, transform.position.y, transform.position.z);

		// 5. YÖNELME (STEERING - Burnunu Çevirme)
		float xDiff = targetX - transform.position.x;
		float targetRotationY = xDiff * turnAngle;
		Quaternion targetRotation = Quaternion.Euler(0, targetRotationY, 0);
		transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
	}

	// 6. ÇARPIÞMA KONTROLÜ
	private void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Obstacle"))
		{
			// GameManager'a oyunun bittiðini söylüyoruz
			if (GameManager.Instance != null)
				GameManager.Instance.GameOver();
		}
		else if (other.CompareTag("Coin"))
		{
			// GameManager'a altýný aldýðýmýzý söylüyoruz
			if (GameManager.Instance != null)
				GameManager.Instance.AddCoin();

			// Altýný havuza geri gönderiyoruz (SetActive false)
			other.gameObject.SetActive(false);
		}
	}
}

/* old version
using UnityEngine;
using UnityEngine.InputSystem;

public class CarController : MonoBehaviour
{
    [Header("Temel Hareket")]
    public float laneDistance = 3.0f;
    public float forwardSpeed = 20.0f;
    public float laneChangeSpeed = 10.0f;

    [Header("Zorluk (Hýzlanma) Ayarlarý")]
    public float maxSpeed = 50.0f; // Çýkabileceði en yüksek hýz
    public float acceleration = 0.5f; // Saniyede ne kadar hýzlanacaðý

    [Header("Dönüþ Ayarlarý")]
    public float turnAngle = 15.0f; // Saða/sola geçerken burnunu ne kadar çevireceði
    public float turnSpeed = 15.0f; // Dönüþ animasyonunun hýzý

    [Header("Skor Sistemi")]
    public int score = 0; // Skorumuzu en yukarýya taþýdýk ki Unity üzerinden de görebilelim

    private int currentLane = 1; // 0: Sol, 1: Orta, 2: Sað

    void Update()
    {
        // 1. ZORLUK ARTIRIMI (Ývmelenme)
        if (forwardSpeed < maxSpeed)
        {
            // Zamanla hýzý maxSpeed deðerine kadar yavaþça artýr
            forwardSpeed += acceleration * Time.deltaTime;
        }

        // 2. KONTROLLER (Input System)
        if (Keyboard.current != null)
        {
            if (Keyboard.current.rightArrowKey.wasPressedThisFrame || Keyboard.current.dKey.wasPressedThisFrame)
            {
                currentLane++;
                if (currentLane > 2) currentLane = 2; // En sað þeritteyse daha saða gidemez
            }

            if (Keyboard.current.leftArrowKey.wasPressedThisFrame || Keyboard.current.aKey.wasPressedThisFrame)
            {
                currentLane--;
                if (currentLane < 0) currentLane = 0; // En sol þeritteyse daha sola gidemez
            }
        }

        // 3. ÝLERÝ GÝDÝÞ (Sabit Hýz)
        transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime, Space.World);

        // 4. SAÐA/SOLA GEÇÝÞ (Yumuþak Hýz - X Ekseni)
        float targetX = (currentLane - 1) * laneDistance;
        float smoothX = Mathf.Lerp(transform.position.x, targetX, laneChangeSpeed * Time.deltaTime);
        transform.position = new Vector3(smoothX, transform.position.y, transform.position.z);

        // 5. YÖNELME (STEERING - Burnunu Çevirme)
        float xDiff = targetX - transform.position.x;
        float targetRotationY = xDiff * turnAngle;
        Quaternion targetRotation = Quaternion.Euler(0, targetRotationY, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
    }

    // 6. ÇARPIÞMA KONTROLÜ (Ýki kontrolü burada BÝRLEÞTÝRDÝK)
    private void OnTriggerEnter(Collider other)
    {
        // Eðer çarptýðýmýz þey "Obstacle" (Engel) ise...
        if (other.CompareTag("Obstacle"))
        {
            Debug.Log("ENGELE ÇARPTIN! OYUN BÝTTÝ.");
            // Oyundaki zaman akýþýný durdurur
            Time.timeScale = 0f;
        }

        // Eðer içinden geçtiðimiz þey "Coin" (Altýn) ise...
        else if (other.CompareTag("Coin"))
        {
            score += 10;
            Debug.Log("Altýn toplandý! Yeni Skor: " + score);
            other.gameObject.SetActive(false); // DÝKKAT: Destroy yerine artýk bunu kullanýyoruz!
        }
    }
}

*/