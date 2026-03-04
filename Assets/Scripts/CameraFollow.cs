using UnityEngine;

public class CameraFollow : MonoBehaviour
{
	[Header("Takip Edilecek Obje")]
	public Transform target;

	[Header("Kamera Ayarlarý")]
	public Vector3 offset = new Vector3(0f, 3f, -6f); // Kameranýn arabaya uzaklýðý ve yüksekliði
	public float xSmoothness = 5f; // Saða sola geçiþlerde kameranýn yumuþaklýðý

	void LateUpdate()
	{
		if (target == null) return;

		// X ekseninde arabayý yumuþak bir þekilde takip etmesi için Lerp kullanýyoruz.
		// Eðer kameranýn saða sola hiç gitmesini istemezsen target.position.x yerine 0 yazabilirsin.
		float smoothX = Mathf.Lerp(transform.position.x, target.position.x, xSmoothness * Time.deltaTime);

		// Kameranýn yeni pozisyonunu belirliyoruz. Y (yükseklik) ve Z (mesafe) doðrudan offset ile ayarlanýyor.
		Vector3 newPosition = new Vector3(smoothX, target.position.y + offset.y, target.position.z + offset.z);

		transform.position = newPosition;
	}
}