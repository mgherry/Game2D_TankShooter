using UnityEngine;
using System.Collections;
using System;
using System.Reflection;

public class IManagerBase<T> : MonoBehaviour where T:UnityEngine.MonoBehaviour{

    protected static T instance;
    protected static object syncRoot = new UnityEngine.Object();

    protected IManagerBase() { }

    public void Start()
    {
        if (instance == null)
            instance = GetComponent<T>();
    }

    public void Awake()
    {
        if (instance == null)
            instance = GetComponent<T>();
    }

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
				instance = FindObjectOfType<T>();

				if (instance == null) {
					lock (syncRoot)
					{
						CreateInstance();
					}
				}
            }

            return instance;
        }
    }

    private static void CreateInstance()
	{
		if (instance == null)
		{
			GameObject newObject = new GameObject("Unnamed");
			newObject.AddComponent<T>();
		}

		// TODO: Kardos kurva anyját
		/*if (instance == null)
        {
            Type t = typeof(T);
            // Ensure there are no public constructors...
            ConstructorInfo[] ctors = t.GetConstructors();
            if (ctors.Length > 0)
            {
                throw new InvalidOperationException(String.Format("{0} has at least one accesible ctor making it impossible to enforce singleton behaviour", t.Name));
            }
            // Create an instance via the private constructor
            instance = (T)Activator.CreateInstance(t, true);
        }*/
	}
}
