using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Core.Factory {
    public abstract class FactoryBase<T> : IFactory<T> where T : Component {
        public string Address { get; set; }

        public UnityAction<T> OnCreationDone { get; set; }

        protected FactoryBase(string address) {
            Address = address;
        }

        public void RequestCreation() {
            ((IFactory<T>)this).Create();
        }

        void IFactory<T>.Create() {
            Addressables.LoadAssetAsync<GameObject>(Address).Completed +=
                ((IFactory<T>)this).OnCreationCompletedHandler;
        }

        void IFactory<T>.OnCreationCompletedHandler(AsyncOperationHandle<GameObject> obj) {
            if (obj.Result is null) {
                Debug.LogError($"Could not find an object of type {typeof(T)} with address {Address}");
                return;
            }

            var type = obj.Result.GetComponent<T>();
            if (type is null)
                return;
            SetCreatedData(type);
        }

        protected abstract void SetCreatedData(T obj);
    }
}