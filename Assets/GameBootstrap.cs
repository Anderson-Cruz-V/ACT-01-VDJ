using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameBootstrap : MonoBehaviour
{
    private Transform player;
    private PlayerHealth health;
    private TMP_Text scoreText;
    private TMP_Text livesText;
    private GameObject endPanel;
    private TMP_Text endTitle;
    private int score;
    private Puntaje scoreManager;
    private bool finished;
    private bool paused;
    private float goalX;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void Install()
    {
        if (SceneManager.GetActiveScene().name == "SampleScene" && FindAnyObjectByType<GameBootstrap>() == null)
            new GameObject("Game Manager").AddComponent<GameBootstrap>();
    }

    private void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject == null) return;
        player = playerObject.transform;
        health = playerObject.GetComponent<PlayerHealth>() ?? playerObject.AddComponent<PlayerHealth>();
        health.OnHealthChanged += RefreshLives;
        health.OnDefeated += () => Finish(false);
        BuildUi();
        scoreManager = FindAnyObjectByType<Puntaje>();
        if (scoreManager != null)
        {
            scoreManager.PuntosCambiados += RefreshScore;
            TMP_Text legacyText = scoreManager.GetComponent<TMP_Text>();
            if (legacyText != null) legacyText.enabled = false;
            RefreshScore(scoreManager.Puntos);
        }
        RefreshLives(health.CurrentHealth);
       
    }

    private void Update()   
    {
        if (player == null || finished) return;
        if (Input.GetKeyDown(KeyCode.Escape)) TogglePause();
    }

 
    private Sprite MakeSprite(Color color)
    {
        var texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, color);
        texture.Apply();
        return Sprite.Create(texture, new Rect(0, 0, 1, 1), new Vector2(.5f, .5f), 1f);
    }

    private void BuildUi()
    {
        Canvas canvas = UiFactory.Canvas("Interfaz del Juego");
        scoreText = UiFactory.Label(canvas.transform, "PUNTAJE  0000", 30, TextAlignmentOptions.TopLeft);
        UiFactory.Anchor(scoreText.rectTransform, new Vector2(0, 1), new Vector2(0, 1), new Vector2(190, -48), new Vector2(330, 55));
        livesText = UiFactory.Label(canvas.transform, "VIDAS  3", 30, TextAlignmentOptions.TopRight);
        UiFactory.Anchor(livesText.rectTransform, new Vector2(1, 1), new Vector2(1, 1), new Vector2(-155, -48), new Vector2(260, 55));
        TMP_Text hint = UiFactory.Label(canvas.transform, "A/D o flechas: mover   •   ESPACIO: saltar   •   ESC: pausa", 20, TextAlignmentOptions.Bottom);
        UiFactory.Anchor(hint.rectTransform, new Vector2(.5f, 0), new Vector2(.5f, 0), new Vector2(0, 12), new Vector2(760, 40));

        endPanel = UiFactory.Panel(canvas.transform, new Color(.02f, .03f, .08f, .92f));
        endPanel.SetActive(false);
        endTitle = UiFactory.Label(endPanel.transform, "", 58, TextAlignmentOptions.Center);
        UiFactory.Anchor(endTitle.rectTransform, new Vector2(.5f, .68f), new Vector2(.5f, .68f), Vector2.zero, new Vector2(800, 110));
        UiFactory.Button(endPanel.transform, "JUGAR DE NUEVO", new Vector2(0, -25), () => { Time.timeScale = 1f; SceneManager.LoadScene("SampleScene"); });
        UiFactory.Button(endPanel.transform, "VOLVER AL MENÚ", new Vector2(0, -115), () => { Time.timeScale = 1f; SceneManager.LoadScene("Menu"); });
    }

    public void ReachGoal() => Finish(true);

    private void RefreshLives(int value) => livesText.text = $"VIDAS  {value}";

    private void RefreshScore(int value)
    {
        score = value;
        scoreText.text = $"PUNTAJE  {score:0000}";
    }

    private void TogglePause()
    {
        paused = !paused;
        GameAudioManager.Play(GameSound.Pause, 0.2f);
        Time.timeScale = paused ? 0f : 1f;
        if (paused) { endTitle.text = "PAUSA"; endPanel.SetActive(true); }
        else endPanel.SetActive(false);
    }

    private void Finish(bool won)
    {
        if (finished) return;
        finished = true;
        GameAudioManager.Play(won ? GameSound.Victory : GameSound.GameOver, 0.5f);
        endTitle.text = won ? "¡MISIÓN CUMPLIDA!" : "MISIÓN FALLIDA";
        endTitle.color = won ? new Color(1f, .72f, .08f) : new Color(1f, .25f, .2f);
        endPanel.SetActive(true);
        Time.timeScale = 0f;
    }
}


public class PlayerHealth : MonoBehaviour
{
    public int CurrentHealth { get; private set; } = 3;
    public event System.Action<int> OnHealthChanged;
    public event System.Action OnDefeated;
    private bool invulnerable;
    private Vector3 checkpoint;
    private SpriteRenderer sprite;

    private void Awake() { checkpoint = transform.position; sprite = GetComponent<SpriteRenderer>(); }
    private void Update() { if (transform.position.y < -12f) Damage(true); }
    private void OnTriggerEnter2D(Collider2D other) { if (other.CompareTag("Enemy")) Damage(false); }

    private void Damage(bool fell)
    {
        if (invulnerable) return;
        CurrentHealth--;
        OnHealthChanged?.Invoke(CurrentHealth);
        if (CurrentHealth <= 0) { OnDefeated?.Invoke(); return; }
        StartCoroutine(Recover(fell));
    }

    private IEnumerator Recover(bool respawn)
    {
        invulnerable = true;
        if (respawn) { transform.position = checkpoint; GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero; }
        for (int i = 0; i < 8; i++) { if (sprite) sprite.enabled = !sprite.enabled; yield return new WaitForSeconds(.1f); }
        if (sprite) sprite.enabled = true;
        invulnerable = false;
    }
}

public static class UiFactory
{
    public static Canvas Canvas(string name)
    {
        GameObject go = new GameObject(name, typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        Canvas canvas = go.GetComponent<Canvas>(); canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        CanvasScaler scaler = go.GetComponent<CanvasScaler>(); scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize; scaler.referenceResolution = new Vector2(1920, 1080);
        if (Object.FindAnyObjectByType<EventSystem>() == null) new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
        return canvas;
    }

    public static TMP_Text Label(Transform parent, string value, float size, TextAlignmentOptions alignment)
    {
        GameObject go = new GameObject(value, typeof(RectTransform), typeof(TextMeshProUGUI)); go.transform.SetParent(parent, false);
        TMP_Text label = go.GetComponent<TMP_Text>(); label.text = value; label.fontSize = size; label.alignment = alignment; label.color = Color.white; label.fontStyle = FontStyles.Bold;
        label.outlineWidth = .18f; label.outlineColor = new Color(0, 0, 0, .85f); return label;
    }

    public static GameObject Panel(Transform parent, Color color)
    {
        GameObject go = new GameObject("Panel", typeof(RectTransform), typeof(Image)); go.transform.SetParent(parent, false);
        RectTransform rect = go.GetComponent<RectTransform>(); rect.anchorMin = Vector2.zero; rect.anchorMax = Vector2.one; rect.offsetMin = rect.offsetMax = Vector2.zero;
        go.GetComponent<Image>().color = color; return go;
    }

    public static void Button(Transform parent, string text, Vector2 position, UnityEngine.Events.UnityAction action)
    {
        GameObject go = new GameObject(text, typeof(RectTransform), typeof(Image), typeof(Button)); go.transform.SetParent(parent, false);
        RectTransform rect = go.GetComponent<RectTransform>(); Anchor(rect, new Vector2(.5f, .45f), new Vector2(.5f, .45f), position, new Vector2(420, 72));
        go.GetComponent<Image>().color = new Color(.95f, .38f, .05f, 1f);
        Button button = go.GetComponent<Button>();
        button.onClick.AddListener(() => GameAudioManager.Play(GameSound.Button));
        button.onClick.AddListener(action);
        TMP_Text label = Label(go.transform, text, 25, TextAlignmentOptions.Center); label.rectTransform.anchorMin = Vector2.zero; label.rectTransform.anchorMax = Vector2.one; label.rectTransform.offsetMin = label.rectTransform.offsetMax = Vector2.zero;
    }

    public static void Anchor(RectTransform rect, Vector2 min, Vector2 max, Vector2 position, Vector2 size)
    { rect.anchorMin = min; rect.anchorMax = max; rect.anchoredPosition = position; rect.sizeDelta = size; }
}
