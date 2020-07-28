using UnityEngine;

namespace Grigorov.Components {
    /*
        Ищем компонент на указанном геймобжекте только при первом обращении к свойству Component и кешируем его
    */
    public class CachedComponent<T> where T: Component {
        T          _component  = null;
        GameObject _gameObject = null;

        public T Component {
            get {
                if ( !_component ) {
                    _component = _gameObject.GetComponent<T>();
                }
                return _component;
            }
        }

        public CachedComponent(GameObject go) {
            _gameObject = go;
        }
    }
}