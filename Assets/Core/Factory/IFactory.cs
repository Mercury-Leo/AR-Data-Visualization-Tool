using UnityEngine;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Core.Factory {
    public interface IFactory<T> {
        public string Address { get; set; }

        public UnityAction<T> OnCreationDone { get; set; }

        public void RequestCreation();

        protected internal void Create();

        protected internal void OnCreationCompletedHandler(AsyncOperationHandle<GameObject> obj);
    }
}