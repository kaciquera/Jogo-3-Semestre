using UnityEngine;

namespace Game
{
    [System.Serializable]
    public class PrefabInstantiator<T> where T : MonoBehaviour
    {
        [SerializeField] private T prefab;
        [SerializeField] private Transform parent;

        public T Prefab => prefab;
        public Transform Parent => parent;

        public virtual T Instatiate()
        {
            return Object.Instantiate(prefab, parent);
        }

        public virtual T Instatiate(Vector3 position)
        {
            return Object.Instantiate(prefab, position, Quaternion.identity, parent);
        }

        public virtual T Instatiate(System.Action<T> obj)
        {
            T newObject = Object.Instantiate(prefab, parent);
            obj.Invoke(newObject);
            return newObject;
        }

        public virtual T Instatiate(Vector3 position, System.Action<T> obj)
        {
            T newObject = Object.Instantiate(prefab, position, Quaternion.identity, parent);
            obj.Invoke(newObject);
            return newObject;
        }
    }

}
