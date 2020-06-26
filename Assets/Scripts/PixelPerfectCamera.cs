using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
  * Berechnet die Differenz in der Skala und vergrössert die Werte im Spiel. 
  * Wenn die Skala basierend auf der Auflösung des Geräts, auf dem das
  * Spiel spielt, neu berechnet wird, ändert sie einfach die Grösse der 
  * Kamera, so dass sie mehr hineinzoomt, um das optimale Aussehen zu erhalten.     
  */
public class PixelPerfectCamera : MonoBehaviour
{

    public static PixelPerfectCamera Instance { get; private set; }
    //Behaltet den Überblick über das Pixel-Einheit-Verhältnis, das für das Artwork eingestellt ist.
    public static float pixelsToUnits = 1f;


    public static float scale = 1f;

    // Stellt die native Pixelauflösung des Spiels dar (in diesem Fall für das iPad Pro 12.9").
    public Vector2 nativeResolution = new Vector2(2732, 2048);

    // runs before the Start() method
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        //Referenz auf die Kamera, mit der gearbeitet wird
        var camera = GetComponent<Camera>();

        //überprüft die 2D Kamerasicht
        if (camera.orthographic)
        {
            /*
             * nimmt die aktuelle Höhe des Bildschirms und teilt sie durch die 
             * definierte native Höhenauflösung.
             */
            scale = Screen.height / nativeResolution.y;

            /*
             * ändert Pixel zu Einheiten, so dass es sich auf die Skala im Spiel 
             * bezogen wird.dies kann als Bezugspunkt verwendet werden, um z.B.
             * zu wissen, ob die pixelsToUnits jetzt aufgrund einer größeren
             * Auflösung viel größer ist.
             */
            pixelsToUnits *= scale;
            camera.orthographicSize = (Screen.height / 2.0f) / pixelsToUnits;

        }
        DontDestroyOnLoad(gameObject);
    }
}
