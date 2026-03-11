using UnityEngine;
using TMPro; // Ekrana yazý yazdýrmak için gereken kütüphane!

public class GameManager : MonoBehaviour
{
	public static GameManager Instance;

	[Header("Referanslar")]
	public Transform playerTransform;
	public TextMeshProUGUI scoreText; // Ekrana baðlayacaðýmýz yazý objesi
	public TextMeshProUGUI coinText;  // YENÝ: Ekrana baðlayacaðýmýz altýn yazý objesi

	[Header("Skor Bilgileri")]
	public float currentScore;
	public int totalCoins;

	private bool isGameActive = true;
	private float startZPos;

	private void Awake()
	{
		if (Instance == null) Instance = this;
		else Destroy(gameObject);
	}

	void Start()
	{
		Time.timeScale = 1f;

		if (playerTransform != null)
		{
			startZPos = playerTransform.position.z;
		}

		// YENÝ: Oyun baþladýðýnda ekrandaki altýn yazýsýný 0 olarak ayarla
		UpdateCoinUI();
	}

	void Update()
	{
		if (isGameActive && playerTransform != null)
		{
			// Mesafe Skoru
			float distanceScore = playerTransform.position.z - startZPos;

			// Toplam Skor
			currentScore = distanceScore + (totalCoins * 10);

			// SKORU EKRANA YAZDIRMA KISMI:
			if (scoreText != null)
			{
				scoreText.text = "Score: " + Mathf.FloorToInt(currentScore).ToString();
			}
		}
	}

	public void AddCoin()
	{
		totalCoins++;

		// YENÝ: Altýn alýndýðýnda ekrandaki yazýyý da anýnda güncelle
		UpdateCoinUI();
	}

	// YENÝ: Altýn metnini güncelleyen yardýmcý fonksiyon
	private void UpdateCoinUI()
	{
		if (coinText != null)
		{
			coinText.text = "Coins: " + totalCoins.ToString();
		}
	}

	public void GameOver()
	{
		isGameActive = false;
		Debug.Log("ENGELE ÇARPTIN! Final Skor: " + Mathf.FloorToInt(currentScore));
		Time.timeScale = 0f;
	}
}