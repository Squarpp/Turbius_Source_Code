using System.Collections;
using UnityEngine;

namespace DrawerScript
{
    [RequireComponent(typeof(AudioSource))]
    public class Drawer : MonoBehaviour
    {
        public bool open;
        public float smooth = 1.0f;
        public Transform DrawerA;
        public Transform DrawerB;
        private AudioSource asource;
        public AudioClip openDrawer, closeDrawer;
        private Coroutine moveCoroutine;
        public bool locked;

        void Start()
        {
            asource = GetComponent<AudioSource>();
        }

        public void OpenDrawer()
        {
                open = !open;
                asource.clip = open ? openDrawer : closeDrawer;
                asource.Play();

                if (moveCoroutine != null)
                    StopCoroutine(moveCoroutine);

                moveCoroutine = StartCoroutine(MoveDrawer(open ? DrawerB.position : DrawerA.position));
        }

        private IEnumerator MoveDrawer(Vector3 targetPosition)
        {
            float duration = 1f / smooth; // Duración ajustable según `smooth`
            float time = 0;
            Vector3 startPosition = transform.position;

            while (time < duration)
            {
                time += Time.deltaTime;
                float t = time / duration;
                t = Mathf.SmoothStep(0, 1, t); // Suaviza la interpolación
                transform.position = Vector3.Lerp(startPosition, targetPosition, t);
                yield return null;
            }

            transform.position = targetPosition; // Asegura que llegue exactamente al destino
        }
    }
}
