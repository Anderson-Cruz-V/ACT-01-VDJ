using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSceneController : MonoBehaviour
{
    private void Start()
    {
        Time.timeScale = 1f;
        Camera camera = Camera.main;
        if (camera != null) camera.backgroundColor = new Color(.025f, .045f, .09f);
        var canvas = UiFactory.Canvas("Menú Principal");
        var panel = UiFactory.Panel(canvas.transform, new Color(.015f, .025f, .06f, 1f));
        TMP_Text title = UiFactory.Label(panel.transform, "NARUTO\nMISIÓN DEL BOSQUE", 72, TextAlignmentOptions.Center);
        title.color = new Color(1f, .55f, .05f); UiFactory.Anchor(title.rectTransform, new Vector2(.5f, .72f), new Vector2(.5f, .72f), Vector2.zero, new Vector2(1000, 230));
        TMP_Text subtitle = UiFactory.Label(panel.transform, "Supera a los enemigos y alcanza la meta", 25, TextAlignmentOptions.Center);
        UiFactory.Anchor(subtitle.rectTransform, new Vector2(.5f, .55f), new Vector2(.5f, .55f), Vector2.zero, new Vector2(900, 55));
        UiFactory.Button(panel.transform, "INICIAR MISIÓN", new Vector2(0, -20), () => SceneManager.LoadScene("SampleScene"));
        TMP_Text controls = UiFactory.Label(panel.transform, "A/D o flechas para moverte  •  ESPACIO para saltar  •  ESC para pausar", 21, TextAlignmentOptions.Center);
        UiFactory.Anchor(controls.rectTransform, new Vector2(.5f, .16f), new Vector2(.5f, .16f), Vector2.zero, new Vector2(1000, 60));
    }
}
