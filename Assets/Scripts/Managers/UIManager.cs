using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class UIManager : MonoBehaviour
{
    public AudioMixer audioMixer;

    [Header("Buttons")]
    public Button startButton;
    public Button settingsButton;
    public Button quitButton;
    public Button returnToMenuButton;
    public Button returnToGameButton;

    [Header("Menu")]
    public GameObject mainMenu;
    public GameObject settingsMenu;
    public GameObject pauseMenu;

    [Header("Text")]
    public Text volSliderText;
    public Text livesText;

    [Header("Image")]
    public Image lifeImage;
    private List<Image> lifeImages = new List<Image>();


    [Header("Slider")]
    public Slider volSlider;

    public AudioClip pauseSound;

    void StartGame()
    {
        SceneManager.LoadScene("Level");
        GetComponent<AudioSource>().mute = false;
        Time.timeScale = 1f;
    }

    void ShowSettingsMenu()
    {
        mainMenu.SetActive(false);
        settingsMenu.SetActive(true);

        if (volSlider && volSliderText)
        {
            float value;
            audioMixer.GetFloat("MasterVol", out value);
            volSlider.value = value + 80;
            volSliderText.text = Mathf.Ceil((value + 80)).ToString();
        }
    }

    void ShowMainMenu()
    {
        if (SceneManager.GetActiveScene().name == "Level")
        {
            SceneManager.LoadScene("Title");
        }
        else
        {
            settingsMenu.SetActive(false);
            mainMenu.SetActive(true);
        }
    }

    void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    void OnSliderValueChanged(float value)
    {
        if (volSliderText)
        {
            volSliderText.text = value.ToString();
            audioMixer.SetFloat("MasterVol", value - 80); // 0 to 100 = -80 to 20
        }
    }

    void ResumeGame()
    {
        pauseMenu.SetActive(false);
        GetComponent<AudioSource>().mute = false;
        Time.timeScale = 1f;
    }

    void UpdateLifeText(int value)
    {
        Debug.Log("Life change " + value);
        // livesText.text = value.ToString();
        // Destroy or add the tracked life image objects
        Transform hudTransform = transform.Find("HUD");
        Transform lifeMeterTransform = hudTransform.Find("LifeMeter");
        RectTransform lifeMeterRectTransform = lifeMeterTransform.GetComponentInChildren<RectTransform>();
        // Create the number of life images and add them to a list so they can be destroyed when player dies during update
        if (value > int.Parse(livesText.text))
        {
            Image newImage = Instantiate(lifeImage, lifeMeterRectTransform);
            newImage.rectTransform.localScale = Vector3.one;
            lifeImages.Add(newImage);
            LayoutRebuilder.ForceRebuildLayoutImmediate(lifeMeterRectTransform);
            livesText.text = value.ToString();
        }
        else
        {
            int lifeImagesIndex = lifeImages.Count - 1;
            if (lifeImagesIndex != 0)
            {
                Destroy(lifeImages[lifeImagesIndex].gameObject);
                lifeImages.RemoveAt(lifeImagesIndex);
                livesText.text = value.ToString();
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (startButton)
            startButton.onClick.AddListener(StartGame);

        if (settingsButton)
            settingsButton.onClick.AddListener(ShowSettingsMenu);

        if (quitButton)
            quitButton.onClick.AddListener(QuitGame);

        if (volSlider)
            volSlider.onValueChanged.AddListener(OnSliderValueChanged);

        if (returnToGameButton)
            returnToGameButton.onClick.AddListener(ResumeGame);

        if (returnToMenuButton)
            returnToMenuButton.onClick.AddListener(ShowMainMenu);

        if (livesText)
            GameManager.instance.onLifeValueChanged.AddListener(UpdateLifeText);

        // Find the child LifeMeter object because it has a HorizontalLayoutGroup for the life images
        Transform hudTransform = transform.Find("HUD");
        Transform lifeMeterTransform = hudTransform.Find("LifeMeter");
        RectTransform lifeMeterRectTransform = lifeMeterTransform.GetComponentInChildren<RectTransform>();
        // Create the number of life images and add them to a list so they can be destroyed when player dies during update
        for (int i = 0; i < int.Parse(livesText.text); i++)
        {
            Image newImage = Instantiate(lifeImage, lifeMeterRectTransform);
            newImage.rectTransform.localScale = Vector3.one;
            lifeImages.Add(newImage);
            LayoutRebuilder.ForceRebuildLayoutImmediate(lifeMeterRectTransform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!pauseMenu) return;

        if (Input.GetKeyDown(KeyCode.P))
        {
            pauseMenu.SetActive(!pauseMenu.activeSelf);

            // HINT
            if (pauseMenu.activeSelf)
            {
                GameManager.instance.playerInstance.GetComponent<AudioSourceManager>().PlayOneShot(pauseSound, false);
                GetComponent<AudioSource>().mute = true;
                Time.timeScale = 0f;
            }
            else
            {
                GetComponent<AudioSource>().mute = false;
                Time.timeScale = 1f;
            }
        }
    }
}
