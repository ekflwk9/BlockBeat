using UnityEngine;

public static class Service
{
    /// <summary>
    /// 해당 컴포넌트가 붙어있다면 제거
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="_this"></param>
    public static void Remove<T>(this Component _this) where T : Component
    {
        if (_this.TryGetComponent<T>(out var _component))
        {
            MonoBehaviour.DestroyImmediate(_component);
        }
    }

    /// <summary>
    /// _instance가 없다면 DDO후 true / 이미 있다면 삭제 후 false
    /// </summary>
    /// <param name="_this"></param>
    /// <returns></returns>
    public static bool Singleton<T>(this Component _this, T _instance) where T : Component
    {
        if (!_instance)
        {
            MonoBehaviour.DontDestroyOnLoad(_this.gameObject);
            return true;
        }

        else
        {
            MonoBehaviour.Destroy(_this.gameObject);
            return false;
        }
    }

    /// <summary>
    /// 자식 중 컴포넌트가 존재하면 반환 / 없으면 추가 후 반환
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="_this"></param>
    /// <returns></returns>
    public static T RequireChildComponent<T>(this Component _this, string _childName) where T : Component
    {
        var child = Service.TryFindChild(_this, _childName);
        if (!child) return null;

        if (child.TryGetComponent<T>(out var findComponent)) return findComponent;
        else return child.gameObject.AddComponent<T>();
    }

    /// <summary>
    /// 컴포넌트가 존재하면 반환 / 없으면 추가 후 반환
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="_this"></param>
    /// <returns></returns>
    public static T RequireComponent<T>(this Component _this) where T : Component
    {
        if (_this.TryGetComponent<T>(out var component)) return component;
        else return _this.gameObject.AddComponent<T>();
    }

    /// <summary>
    /// 예외처리된 특정 자식 이름의 컴포넌트를 가져오는 메서드
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="_this"></param>
    /// <param name="_childName"></param>
    /// <returns></returns>
    public static T TryGetChildComponent<T>(this Component _this, string _childName) where T : class
    {
        var child = Service.TryFindChild(_this, _childName);
        if (child == null) return null;

        T component = null;

        if (child.TryGetComponent<T>(out var findComponent)) component = findComponent;
        else Log($"{child.name}에 {typeof(T).Name}이라는 컴포넌트는 존재하지 않음");

        return component;
    }

    /// <summary>
    /// 예외처리가된 특정 자식 오브젝트의 컴포넌트를 가져오는  메서드
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="_parent"></param>
    /// <param name="_childName"></param>
    /// <returns></returns>
    public static T TryGetChildComponent<T>(this Component _this) where T : class
    {
        var component = _this.GetComponentInChildren<T>(true);
        if (component == null) Log($"{_this.name}에 {typeof(T).Name}이라는 컴포넌트는 존재하지 않음");

        return component;
    }

    /// <summary>
    /// 예외처리가 된 자기 자신 컴포넌트를 가져오는 메서드
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="_this"></param>
    /// <returns></returns>
    public static T TryGetComponent<T>(this Component _this)
    {
        if (_this.TryGetComponent<T>(out var component)) return component;
        else Log($"{_this.name}에 {typeof(T).Name}이라는 컴포넌트는 존재하지 않음");

        return default(T);
    }

    /// <summary>
    /// 특정 이름의 자식 오브젝트를 반환
    /// </summary>
    /// <param name="_parent"></param>
    /// <param name="_childName"></param>
    /// <returns></returns>
    public static GameObject TryFindChild(this Component _parent, string _childName)
    {
        var child = Service.FindChild(_parent.transform, _childName);
        if (child == null) Log($"{_parent.name}에 {_childName}이라는 자식 오브젝트는 존재하지 않음");

        return child;
    }

    private static GameObject FindChild(Transform _parent, string _childName)
    {
        GameObject findChild = null;

        for (int i = 0; i < _parent.transform.childCount; i++)
        {
            var child = _parent.transform.GetChild(i);
            findChild = child.name == _childName ? child.gameObject : FindChild(child, _childName);
            if (findChild != null) break;
        }

        return findChild;
    }

    /// <summary>
    /// 제일 최상단 부모 오브젝트를 반환함
    /// </summary>
    /// <param name="_this"></param>
    /// <returns></returns>
    public static Transform TryFindParent(this Transform _this)
    {
        var parent = _this.transform.parent;

        if (parent == null) return _this.transform;
        else return parent.TryFindParent();
    }

    public static void Log(string _log)
    {
#if UNITY_EDITOR
        Debug.Log(_log);
#endif
    }
}