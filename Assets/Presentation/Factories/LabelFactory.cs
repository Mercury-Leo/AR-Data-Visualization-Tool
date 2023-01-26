using Core.Factory;
using Presentation.Components.Label.Scripts;

namespace Presentation.Factories {
    public class LabelFactory : FactoryBase<LabelInitializer> {
        public LabelFactory(string address) : base(address) { }

        protected override void SetCreatedData(LabelInitializer label) {
            OnCreationDone?.Invoke(label);
        }
    }
}