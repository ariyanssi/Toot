using UnityEngine;

public class PlayerSkinManager : MonoBehaviour
{
    [SerializeField] private GameObject blueSkin;
    [SerializeField] private GameObject whiteSkin;

    private void Start()
    {
        blueSkin.SetActive(true);
        whiteSkin.SetActive(false);
    }

    public void ChangeToWhiteSkin()
    {
        blueSkin.SetActive(false);
        whiteSkin.SetActive(true);
    }
}