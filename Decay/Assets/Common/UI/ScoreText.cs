using UnityEngine;
using UnityEngine.UI;

namespace Common
{
    /// <summary>
    /// Animates a piece of UI text that floats upwards and fades over time.
    /// Once the text has fully faded, it is destroyed
    /// </summary>
    public class ScoreText : MonoBehaviour
    {
        /// <summary>
        /// The distance that the text should rise over its lifetime
        /// </summary>
        [SerializeField]
        private float riseDistance = 1.0f;

        /// <summary>
        /// The period of time, in seconds, that the text rises and fades over
        /// </summary>
        [SerializeField]
        private float fadeTime = 1.0f;

        /// <summary>
        /// The text UI instance to set text on
        /// </summary>
        [SerializeField]
        private Text scoreText;

        /// <summary>
        /// The colour of the text
        /// </summary>
        [SerializeField]
        private Color colour = new Color(0, 0, 0);

        /// <summary>
        /// The colour of the text
        /// </summary>
        public Color Colour
        {
            get => colour;
            set
            {
                if (scoreText)
                {
                    scoreText.color = value;
                }
                colour = value;
            }
        }
  
        private string text = "";

        /// <summary>
        /// The actual text shown
        /// </summary>
        public string Text
        {
            get => text;
            set
            {
                if (scoreText)
                {
                    scoreText.text = value;
                }
                text = value;
            }
        }

        private Vector3 startPosition;
        private float startTime;

        void Start()
        {
            if (text.Length > 0)
            {
                scoreText.text = text;
            }
            scoreText.color = colour;
            startPosition = transform.position;
            startTime = Time.time;
        }

        void Update()
        {
            float endTime = startTime + fadeTime;
            if (Time.time >= endTime)
            {
                Destroy(gameObject);
            }
            else
            {
                float factor = Mathf.InverseLerp(startTime, startTime + fadeTime, Time.time);
                float smoothed = Mathf.SmoothStep(0.0f, 1.0f, factor);

                transform.position = Vector3.Lerp(startPosition, startPosition + Vector3.up * riseDistance, smoothed);

                Color color = scoreText.color;
                color.a = 1.0f - factor;
                scoreText.color = color;
            }
        }
    }
}
