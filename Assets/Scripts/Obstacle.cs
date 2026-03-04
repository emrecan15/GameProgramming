using UnityEngine;

public class Obstacle : MonoBehaviour
{
	private Transform player;

	void OnEnable()
	{
		// Engel havuzdan çekilip sahneye her geldiðinde oyuncuyu (arabayý) bulur
		if (player == null)
		{
			GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
			if (playerObj != null)
			{
				player = playerObj.transform;
			}
		}
	}

	void Update()
	{
		// Eðer engel, arabanýn Z ekseninde 10 birim GERÝSÝNDE kaldýysa...
		if (player != null && transform.position.z < player.position.z - 10f)
		{
			// Kendini kapatýp havuza geri dön! (Böylece sistem bunu tekrar kullanabilir)
			gameObject.SetActive(false);
		}
	}
}