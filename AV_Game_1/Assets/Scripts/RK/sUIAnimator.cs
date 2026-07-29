using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace AVSim.UI
{
    public class sUIAnimator : MonoBehaviour
    {
        public enum AnimationType
        {
            Pulse,
            Spin,
            Move,
            Fade,
            Scale,
            Shake,
            Bounce
        }

        [Header("Animation")]
        public AnimationType animationType;

        public bool playOnStart = true;
        public bool loop = true;

        [Header("Timing")]
        public float duration = 1f;
        public float delay = 0f;

        [Header("Values")]
        public float amount = 1.2f;

        public Vector3 moveOffset = new Vector3(0, 50, 0);
        public Vector3 rotationAxis = Vector3.forward;


        [Header("Ease")]
        public AnimationCurve ease = AnimationCurve.EaseInOut(0, 0, 1, 1);


        RectTransform rect;
        CanvasGroup canvasGroup;

        Vector3 startPosition;
        Vector3 startScale;
        Quaternion startRotation;


        Coroutine animationRoutine;


        void Awake()
        {
            rect = GetComponent<RectTransform>();

            canvasGroup = GetComponent<CanvasGroup>();

            startPosition = rect.localPosition;
            startScale = rect.localScale;
            startRotation = rect.localRotation;


            if (canvasGroup == null)
                canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }


        void Start()
        {
            //if (playOnStart)
            //    Play();
        }

        private void OnEnable()
        {
            if (playOnStart)
                Play();
        }


        public void Play()
        {
            Stop();

            animationRoutine = StartCoroutine(Animate());
        }


        public void Stop()
        {
            if (animationRoutine != null)
            {
                StopCoroutine(animationRoutine);
                animationRoutine = null;
            }

            ResetTransform();
        }



        IEnumerator Animate()
        {
            if (delay > 0)
                yield return new WaitForSeconds(delay);


            do
            {
                float timer = 0;


                while (timer < duration)
                {
                    timer += Time.deltaTime;

                    float t = Mathf.Clamp01(timer / duration);

                    t = ease.Evaluate(t);


                    ApplyAnimation(t);


                    yield return null;
                }


            } while (loop);
        }



        void ApplyAnimation(float t)
        {
            switch (animationType)
            {

                case AnimationType.Pulse:

                    float pulse = Mathf.Lerp(
                        1,
                        amount,
                        Mathf.Sin(t * Mathf.PI)
                    );

                    rect.localScale = startScale * pulse;

                    break;



                case AnimationType.Scale:

                    rect.localScale =
                        Vector3.Lerp(
                            startScale,
                            startScale * amount,
                            t
                        );

                    break;



                case AnimationType.Spin:

                    rect.localRotation =
                        startRotation *
                        Quaternion.AngleAxis(
                            360 * t,
                            rotationAxis
                        );

                    break;



                case AnimationType.Move:

                    rect.localPosition =
                        Vector3.Lerp(
                            startPosition,
                            startPosition + moveOffset,
                            t
                        );

                    break;



                case AnimationType.Fade:

                    canvasGroup.alpha =
                        Mathf.Lerp(
                            0,
                            amount,
                            t
                        );

                    break;



                case AnimationType.Shake:

                    rect.localPosition =
                        startPosition +
                        Random.insideUnitSphere *
                        amount;

                    break;



                case AnimationType.Bounce:

                    float bounce =
                        Mathf.Abs(
                            Mathf.Sin(t * Mathf.PI)
                        );

                    rect.localPosition =
                        startPosition +
                        Vector3.up *
                        (bounce * amount);

                    break;
            }
        }



        void ResetTransform()
        {
            rect.localPosition = startPosition;
            rect.localScale = startScale;
            rect.localRotation = startRotation;

            if (canvasGroup)
                canvasGroup.alpha = 1;
        }
    }
}