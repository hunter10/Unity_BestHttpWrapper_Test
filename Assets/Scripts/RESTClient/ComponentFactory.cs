using UnityEngine;

namespace REST
{
    public class ComponentFactory
    {
        public const string gameObjectName = "RESTfulPlugIn";
        private static GameObject _gameObject;

        private static GameObject GameObject
        {
            get
            {
                if (ComponentFactory._gameObject == null)
                    ComponentFactory._gameObject = new GameObject(gameObjectName);
                return ComponentFactory._gameObject;
            }
        }

        public static T GetComponent<T>() where T : MonoBehaviour
        {
            GameObject gameObject = ComponentFactory.GameObject;
            T obj = gameObject.GetComponent<T>();
            if (obj == null)
            {
                obj = gameObject.AddComponent<T>();
                //Debug.Log(obj.ToString());
            }
            return obj;
        }

        public static T AddComponent<T>() where T : MonoBehaviour
        {
            return ComponentFactory.GameObject.AddComponent<T>();
        }
    }
}
