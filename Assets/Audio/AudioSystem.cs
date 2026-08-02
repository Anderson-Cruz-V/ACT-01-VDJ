using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public enum GameSound
{
    Jump, Landing, Damage, KunaiThrow, Rasengan, EnemyAttack,
    EnemyDefeated, CollectPoints, Button, Pause, Victory, GameOver
}

public sealed class AudioCatalog : ScriptableObject
{
    public AudioClip levelMusic;
    public AudioClip jump;
    public AudioClip landing;
    public AudioClip damage;
    public AudioClip kunaiThrow;
    public AudioClip rasengan;
    public AudioClip enemyAttack;
    public AudioClip enemyDefeated;
    public AudioClip collectPoints;
    public AudioClip button;
    public AudioClip pause;
    public AudioClip victory;
    public AudioClip gameOver;

    public AudioClip Get(GameSound sound)
    {
        switch (sound)
        {
            case GameSound.Jump: return jump;
            case GameSound.Landing: return landing;
            case GameSound.Damage: return damage;
            case GameSound.KunaiThrow: return kunaiThrow;
            case GameSound.Rasengan: return rasengan;
            case GameSound.EnemyAttack: return enemyAttack;
            case GameSound.EnemyDefeated: return enemyDefeated;
            case GameSound.CollectPoints: return collectPoints;
            case GameSound.Button: return button;
            case GameSound.Pause: return pause;
            case GameSound.Victory: return victory;
            case GameSound.GameOver: return gameOver;
            default: return null;
        }
    }
}

[DefaultExecutionOrder(-1000)]
public sealed class GameAudioManager : MonoBehaviour
{
    private const string CatalogResource = "AudioCatalog";
    private static GameAudioManager instance;
    private readonly Dictionary<GameSound, float> lastPlayed = new Dictionary<GameSound, float>();
    private AudioCatalog catalog;
    private AudioSource musicSource;
    private AudioSource sfxSource;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Install()
    {
        if (instance == null) new GameObject("Audio Manager").AddComponent<GameAudioManager>();
    }

    private void Awake()
    {
        if (instance != null && instance != this) { Destroy(gameObject); return; }
        instance = this;
        DontDestroyOnLoad(gameObject);
        catalog = Resources.Load<AudioCatalog>(CatalogResource);
        musicSource = CreateSource("Music", 0.22f, true);
        sfxSource = CreateSource("SFX", 0.78f, false);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private AudioSource CreateSource(string sourceName, float volume, bool loop)
    {
        AudioSource source = gameObject.AddComponent<AudioSource>();
        source.name = sourceName;
        source.playOnAwake = false;
        source.loop = loop;
        source.volume = volume;
        source.spatialBlend = 0f;
        return source;
    }

    private void OnDestroy()
    {
        if (instance == this) SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ConfigureListener();
        if (scene.name == "SampleScene")
        {
            PlayLevelMusic();
            foreach (ComplexEnemy enemy in FindObjectsByType<ComplexEnemy>(FindObjectsInactive.Exclude))
                if (enemy.GetComponent<EnemyAudioRelay>() == null) enemy.gameObject.AddComponent<EnemyAudioRelay>();
        }
        else musicSource.Stop();
    }

    private static void ConfigureListener()
    {
        AudioListener[] listeners = FindObjectsByType<AudioListener>(FindObjectsInactive.Include);
        AudioListener keeper = null;
        foreach (AudioListener listener in listeners)
        {
            if (!listener.gameObject.activeInHierarchy) continue;
            if (keeper == null) { keeper = listener; listener.enabled = true; }
            else listener.enabled = false;
        }
        if (keeper == null && Camera.main != null) Camera.main.gameObject.AddComponent<AudioListener>();
    }

    private void PlayLevelMusic()
    {
        if (catalog == null || catalog.levelMusic == null) return;
        if (musicSource.clip == catalog.levelMusic && musicSource.isPlaying) return;
        musicSource.clip = catalog.levelMusic;
        musicSource.Play();
    }

    public static void Play(GameSound sound, float cooldown = 0.08f)
    {
        if (instance == null || instance.catalog == null) return;
        float now = Time.unscaledTime;
        if (instance.lastPlayed.TryGetValue(sound, out float previous) && now - previous < cooldown) return;
        AudioClip clip = instance.catalog.Get(sound);
        if (clip == null) return;
        instance.lastPlayed[sound] = now;
        instance.sfxSource.PlayOneShot(clip);
    }

    // Public hooks for animation events or existing projectile scripts.
    public static void PlayKunaiThrow() => Play(GameSound.KunaiThrow);
    public static void PlayRasengan() => Play(GameSound.Rasengan);
    public static void PlayEnemyDefeated() => Play(GameSound.EnemyDefeated);
}

public sealed class EnemyAudioRelay : MonoBehaviour
{
    private ComplexEnemy enemy;
    private float nextAttackSound;

    private void Awake() => enemy = GetComponent<ComplexEnemy>();

    private void Update()
    {
        if (enemy == null || enemy.player == null) return;
        float dx = Mathf.Abs(enemy.player.position.x - transform.position.x);
        float dy = Mathf.Abs(enemy.player.position.y - transform.position.y);
        if (dx <= enemy.attackRange && dy <= enemy.verticalRange && Time.time >= nextAttackSound)
        {
            GameAudioManager.Play(GameSound.EnemyAttack, enemy.attackCooldown * 0.75f);
            nextAttackSound = Time.time + enemy.attackCooldown;
        }
    }

    // Disponible para enlazarlo al evento de derrota existente o a un Animation Event.
    public void SonidoDerrotado()
    {
        GameAudioManager.PlayEnemyDefeated();
        GameAudioVfx.Burst(transform.position, new Color(.45f, .12f, .08f, .95f), 18, .18f);
    }
}

public static class GameAudioVfx
{
    public static void Burst(Vector3 position, Color color, int count = 12, float size = 0.16f)
    {
        GameObject go = new GameObject("Audio VFX Burst");
        go.SetActive(false);
        go.transform.position = position;
        ParticleSystem particles = go.AddComponent<ParticleSystem>();
        var main = particles.main;
        main.duration = 0.25f;
        main.loop = false;
        main.startLifetime = new ParticleSystem.MinMaxCurve(0.25f, 0.5f);
        main.startSpeed = new ParticleSystem.MinMaxCurve(1.4f, 3.2f);
        main.startSize = new ParticleSystem.MinMaxCurve(size * 0.6f, size * 1.4f);
        main.startColor = color;
        main.simulationSpace = ParticleSystemSimulationSpace.World;
        main.stopAction = ParticleSystemStopAction.Destroy;
        var emission = particles.emission;
        emission.rateOverTime = 0f;
        emission.SetBursts(new[] { new ParticleSystem.Burst(0f, (short)count) });
        var shape = particles.shape;
        shape.shapeType = ParticleSystemShapeType.Circle;
        shape.radius = 0.12f;
        var renderer = particles.GetComponent<ParticleSystemRenderer>();
        renderer.sortingOrder = 100;
        go.SetActive(true);
        particles.Play();
    }
}

#if UNITY_EDITOR
[InitializeOnLoad]
public static class AudioCatalogBuilder
{
    private const string CatalogPath = "Assets/Resources/AudioCatalog.asset";

    static AudioCatalogBuilder()
    {
        EditorApplication.delayCall += Build;
    }

    [MenuItem("Tools/Naruto/Rebuild Audio Catalog")]
    public static void Build()
    {
        if (EditorApplication.isPlayingOrWillChangePlaymode) return;
        if (!AssetDatabase.IsValidFolder("Assets/Resources")) AssetDatabase.CreateFolder("Assets", "Resources");
        AudioCatalog catalog = AssetDatabase.LoadAssetAtPath<AudioCatalog>(CatalogPath);
        if (catalog == null)
        {
            catalog = ScriptableObject.CreateInstance<AudioCatalog>();
            AssetDatabase.CreateAsset(catalog, CatalogPath);
        }
        catalog.levelMusic = Load("Assets/Audio/Music/level_music.wav");
        catalog.jump = Load("Assets/Audio/SFX/jump.ogg");
        catalog.landing = Load("Assets/Audio/SFX/landing.ogg");
        catalog.damage = Load("Assets/Audio/SFX/damage.ogg");
        catalog.kunaiThrow = Load("Assets/Audio/SFX/kunai_throw.ogg");
        catalog.rasengan = Load("Assets/Audio/SFX/rasengan.ogg");
        catalog.enemyAttack = Load("Assets/Audio/SFX/enemy_attack.ogg");
        catalog.enemyDefeated = Load("Assets/Audio/SFX/enemy_defeated.ogg");
        catalog.collectPoints = Load("Assets/Audio/SFX/collect_points.ogg");
        catalog.button = Load("Assets/Audio/UI/button.mp3");
        catalog.pause = Load("Assets/Audio/UI/pause.mp3");
        catalog.victory = Load("Assets/Audio/UI/victory.mp3");
        catalog.gameOver = Load("Assets/Audio/UI/game_over.mp3");
        EditorUtility.SetDirty(catalog);
        AssetDatabase.SaveAssets();
    }

    private static AudioClip Load(string path) => AssetDatabase.LoadAssetAtPath<AudioClip>(path);
}
#endif
