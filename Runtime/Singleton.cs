using UnityEngine;

namespace Loju
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {

        protected static T _instance;
        private static readonly object _instanceLock = new object();
        private static bool _quitting = false;

        public static T instance
        {
            get
            {
                lock (_instanceLock)
                {
                    if (_instance == null && !_quitting)
                    {

                        _instance = GameObject.FindObjectOfType<T>();
                        if (_instance == null)
                        {
                            GameObject go = new GameObject(typeof(T).Name);
                            _instance = go.AddComponent<T>();

                            if (Application.isPlaying) DontDestroyOnLoad(_instance.gameObject);
                        }
                    }

                    return _instance;
                }
            }
        }

        protected virtual void Awake()
        {
            if (_instance == null) _instance = gameObject.GetComponent<T>();
            else if (_instance.GetInstanceID() != GetInstanceID())
            {
                Destroy(gameObject);
                throw new System.Exception(string.Format("Instance of {0} already exists, removing {1}", GetType().FullName, ToString()));
            }
        }

        protected virtual void OnApplicationQuit()
        {
            _quitting = true;
        }

    }
}

