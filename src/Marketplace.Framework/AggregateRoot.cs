using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.Framework
{
    public abstract class AggregateRoot<TId> : Document, IInternalEventHandler
       where TId : Value<TId>
    {
        public TId AggregateId { get; protected set; }

        protected abstract void When(object @event);

        private List<object> _changes = new List<object>();

        protected AggregateRoot() { }

        protected void Apply(object @event)
        {
            When(@event);
            EnsureValidState();
            if (_changes is null)
            {
                _changes = new List<object>();
            }
            _changes.Add(@event);
        }

        public IEnumerable<object> GetChanges() => _changes.AsEnumerable();

        public void ClearChanges() => _changes.Clear();

        protected abstract void EnsureValidState();

        protected void ApplyToEntity(IInternalEventHandler entity, object @event)
            => entity?.Handle(@event);

        void IInternalEventHandler.Handle(object @event) => When(@event);
    }
}
