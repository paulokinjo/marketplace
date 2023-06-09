﻿namespace Marketplace.Framework
{
    public abstract class Entity
    {
        private readonly List<object> events;

        protected Entity() => events = new List<object>();

        protected void Apply(object @event)
        {
            When(@event);
            EnsureValidState();
            events.Add(@event);
        }

        protected abstract void When(object @event);
        protected abstract void EnsureValidState();

        public IEnumerable<object> GetChanges() => events.AsEnumerable();

        public void ClearChanges() => events.Clear();
    }
}
