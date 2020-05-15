using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using UnityEngine.UI;

namespace Nuwn
{
    namespace Extensions
    {
        public static class Extensions
        {
            /// <summary>
            /// Chech whether or not a gameobject has a component.
            /// </summary>
            /// <typeparam name="T"></typeparam>
            /// <param name="obj"></param>
            /// <returns></returns>
            public static bool HasComponent<T>(this GameObject obj) where T : Component
            {
                return obj.GetComponent<T>() != null;
            }
            public static bool HasMethod(this object objectToCheck, string methodName)
            {
                var type = objectToCheck.GetType();
                return type.GetMethod(methodName) != null;
            }
            public static bool HasProperty(this object objectToCheck, string propertyName)
            {
                var type = objectToCheck.GetType();
                return type.GetProperty(propertyName) != null;
            }

        }
        public static class DebugExtentions
        {
            public static void LogList<T>(this List<T> list)
            {
                foreach (var l in list)
                {
                    Debug.Log(l);
                }
            }

            private static object GetPropValue<T>(T l, string property)
            {
                throw new NotImplementedException();
            }
        }
        public static class TransformExtensions
        {
            /// <summary>
            /// Checks whether or not the transform is in view of main camera
            /// </summary>
            /// <param name="transform"></param>
            /// <returns></returns>
            public static bool IsInView(this Transform transform) => Essentials.Nuwn_Essentials.IsInView(transform.position, Camera.main);
            /// <summary>
            /// Checks if transform is in view of the targeted camera
            /// </summary>
            /// <param name="transform"></param>
            /// <param name="cam"></param>
            /// <returns></returns>
            public static bool IsInView(this Transform transform, Camera cam) => Essentials.Nuwn_Essentials.IsInView(transform.position, cam);
            /// <summary>
            /// returns which camera is viewing the target, 
            /// asking for a list of all cameras you wish too look up,
            /// so it dont have to search for it and that's slow to do.
            /// </summary>
            /// <param name="transform"></param>
            /// <param name="cameras"></param>
            /// <returns></returns>
            public static List<Camera> IsInView(this Transform transform, List<Camera> cameras)
            {
                List<Camera> camerasViewing = new List<Camera>();

                foreach (var cam in cameras)
                {
                    var res = Essentials.Nuwn_Essentials.IsInView(transform.position, cam);
                    if (res)
                        camerasViewing.Add(cam);
                }
                return camerasViewing;
            }

            public static List<GameObject> GetAllChildren(this Transform transform)
            {
                List<GameObject> List = new List<GameObject>();

                for (int i = 0; i < transform.childCount; i++)
                {
                    List.Add(transform.GetChild(i).gameObject);
                }
                return List;
            }

            /// <summary>
            /// Returns a list of first level childrens of a object containing a string key
            /// </summary>
            /// <param name="transform"></param>
            /// <param name="name"></param>
            /// <returns></returns>
            public static List<GameObject> GetAllChildrenContainString(this Transform transform, string name)
            {
                List<GameObject> List = new List<GameObject>();

                for (int i = 0; i < transform.childCount; i++)
                {
                    if (transform.GetChild(i).name.Contains(name))
                        List.Add(transform.GetChild(i).gameObject);
                }
                return List;
            }
            /// <summary>
            /// Check for children first level for regex match
            /// </summary>
            /// <param name="transform"></param>
            /// <param name="regex"></param>
            /// <returns></returns>
            public static List<GameObject> GetAllChildrenRegexString(this Transform transform, string regex)
            {
                List<GameObject> List = new List<GameObject>();
                var newRegex = new Regex(regex);

                for (int i = 0; i < transform.childCount; i++)
                {
                    if (newRegex.IsMatch(transform.GetChild(i).name))
                        List.Add(transform.GetChild(i).gameObject);
                }
                return List;
            }
        }
        public static class MonoBehaviourExtentions
        {
            /// <summary>
            /// Waits n time before activating the debugger. milliseconds
            /// usage : this.setTimeout((result) => { Debug.Log("debug"); }, 1000 );
            /// </summary>
            /// <param name="instance"></param>
            /// <param name="waitTime"></param>
            /// <param name="Callback"></param>
            public static void SetTimeout(this MonoBehaviour instance, Action Callback, float waitTime) => instance.StartCoroutine(Wait((res) => Callback?.Invoke(), waitTime));
            public static void SetTimeout(this MonoBehaviour instance, Action<object> Callback, float waitTime) => instance.StartCoroutine(Wait((res) => Callback?.Invoke(true), waitTime));
            /// <summary>
            /// Continues interval with callback, use stopinterval to stop it.
            /// Calls method aftergiven time.
            /// </summary>
            /// <param name="instance"></param>
            /// <param name="Callback"></param>
            /// <param name="intervalTime"></param>
            /// <returns></returns>
            public static Coroutine SetInterval(this MonoBehaviour instance, Action<object> Callback, float intervalTime) => instance.StartCoroutine(RepeatingWait((res) => Callback?.Invoke(true), intervalTime));
            public static Coroutine SetInterval(this MonoBehaviour instance, Action Callback, float intervalTime) => instance.StartCoroutine(RepeatingWait((res) => Callback?.Invoke(), intervalTime));
            public static Coroutine SetInterval(this MonoBehaviour instance, Action Callback, float intervalTime, bool condition) => instance.StartCoroutine(RepeatingWaitConditioned((res) => Callback?.Invoke(), intervalTime, condition));
            /// <summary>
            /// 
            /// </summary>
            /// <param name="instance"></param>
            /// <param name="coroutine">The interval to stop, store it as a var</param>
            public static void StopInterval(this MonoBehaviour instance, Coroutine coroutine) => instance.StopCoroutine(coroutine);


            #region Internal functions
            static IEnumerator Wait(Action<bool> Callback, float duration)
            {
                yield return new WaitForSeconds(duration / 1000);
                Callback.Invoke(true);
            }
            static IEnumerator RepeatingWait(Action<bool> Callback, float waitTime)
            {
                while (true)
                {
                    yield return new WaitForSeconds(waitTime / 1000);
                    Callback.Invoke(true);
                }
            }
            static IEnumerator RepeatingWaitConditioned(Action<bool> Callback, float waitTime, bool condition)
            {
                while (condition)
                {
                    yield return new WaitForSeconds(waitTime / 1000);
                    Callback.Invoke(true);
                }
            }
            #endregion
        }
        public static class PHP
        {
            public static bool Empty<T>(this T type, out object value)
            {
                value = type;
                return Empty(type);
            }
            public static bool Empty<T>(this T type)
            {
                if (type == null)
                {
                    return true;
                }
                if (type.GetType() == typeof(string))
                {
                    string data = (string)(object)type;
                    return (data == "" || data == "0") ? true : false;
                }
                else if (type.GetType() == typeof(int))
                {
                    int data = (int)(object)type;
                    return (data == 0) ? true : false;
                }
                else if (type.GetType() == typeof(float))
                {
                    float data = (float)(object)type;
                    return (data == 0.0) ? true : false;
                }
                else if (type.GetType() == typeof(bool))
                {
                    bool data = (bool)(object)type;
                    return !data;
                }
                else if (type.GetType().IsArray || type.GetType().IsGenericType)
                {
                    ICollection data = (object)type as ICollection;
                    return (data.Count == 0) ? true : false;
                }
                else
                {
                    return (type == null) ? true : false;
                }
            }
        }
        public static class LayerMaskExtensions
        {
            public static bool HasLayer(this LayerMask layerMask, int layer)
            {
                if (layerMask == (layerMask | (1 << layer)))
                {
                    return true;
                }
                return false;
            }

            public static bool[] HasLayers(this LayerMask layerMask)
            {
                var hasLayers = new bool[32];

                for (int i = 0; i < 32; i++)
                {
                    if (layerMask == (layerMask | (1 << i)))
                    {
                        hasLayers[i] = true;
                    }
                }
                return hasLayers;
            }
        }
        public static class ColliderExtentions
        {
            public static Vector3 RandomPointInBounds(this Collider col)
            {
                return new Vector3(
                    UnityEngine.Random.Range(col.bounds.min.x, col.bounds.max.x),
                    UnityEngine.Random.Range(col.bounds.min.y, col.bounds.max.y),
                    UnityEngine.Random.Range(col.bounds.min.z, col.bounds.max.z)
                );
            }
            public static Vector2 RandomPointInBounds(this Collider2D col)
            {
                return new Vector2(
                    UnityEngine.Random.Range(col.bounds.min.x, col.bounds.max.x),
                    UnityEngine.Random.Range(col.bounds.min.y, col.bounds.max.y)
                );
            }
            public static bool CompareLayer(this Collider2D collision, LayerMask mask)
            {
                return (((1 << collision.gameObject.layer) & mask) != 0) ? true : false;
            }
            public static bool CompareLayer(this Collider collision, LayerMask mask)
            {
                return (((1 << collision.gameObject.layer) & mask) != 0) ? true : false;
            }
            public static bool CompareLayer(this Collision2D collision, LayerMask mask)
            {
                return (((1 << collision.gameObject.layer) & mask) != 0) ? true : false;
            }
            public static bool CompareLayer(this Collision collision, LayerMask mask)
            {
                return (((1 << collision.gameObject.layer) & mask) != 0) ? true : false;
            }
        }
        public static class AudioExtentions
        {
            public static void Fade(this AudioSource a, MonoBehaviour instance, float from, float to, float time, Action callback = null)
            {
                instance.StartCoroutine(Essentials.Nuwn_Essentials.LerpFloat((f) => { a.volume = f; }, from, to, time, (v) => { callback?.Invoke(); }));
            }
        }
        public static class ColorLerpExtentions
        {
            public static void Fade(this Image c, MonoBehaviour instance, float from, float to, float time, Action callback = null)
            {
                instance.StartCoroutine(Essentials.Nuwn_Essentials.LerpFloat((f) =>
                {
                    Color col = c.color;
                    col.a = f;
                    c.color = col;
                }, from, to, time, (v) => { callback?.Invoke(); }));
            }
        }
        public static class ArrayExtensions
        {
            public static T[] Add<T>(this T[] array, T item)
            {
                T[] returnarray = new T[array.Length + 1];
                for (int i = 0; i < array.Length; i++)
                {
                    returnarray[i] = array[i];
                }
                returnarray[array.Length] = item;
                return returnarray;
            }
            public static T[] Remove<T>(this T[] array, T item)
            {
                int index = Array.IndexOf(array, item);
                return array.Where((val, idx) => idx != index).ToArray();
            }

        }

        public static class MathExtentions
        {
            public static int ReverseNormalize(this int value, int from1, int to1, int from2, int to2)
            {
                return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
            }
            public static float ReverseNormalize(this float value, float from1, float to1, float from2, float to2)
            {
                return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
            }

        }
    }
}

