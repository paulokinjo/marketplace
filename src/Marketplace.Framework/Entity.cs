using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Marketplace.Framework
{
    public abstract class Entity<TId> : IInternalEventHandler
       where TId : Value<TId>
    {
        private readonly Action<object> _applier;

        public TId Id { get; protected set; }

        public Entity()
        {

        }

        protected Entity(Action<object> applier) => _applier = applier;

        protected abstract void When(object @event);

        protected void Apply(object @event)
        {
            When(@event);
            _applier(@event);
        }

        void IInternalEventHandler.Handle(object @event) => When(@event);
    }
}
