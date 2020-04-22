﻿using Inferno.Testing;

namespace Inferno.Reactive.Tests
{
    public class HostTestFixture : ReactiveObject
    {
        private TestFixture _child;

        private NonObservableTestFixture _pocoChild;

        private int _someOtherParam;

        public TestFixture Child
        {
            get => _child;
            set => this.RaiseAndSetIfChanged(ref _child, value);
        }

        public NonObservableTestFixture PocoChild
        {
            get => _pocoChild;
            set => this.RaiseAndSetIfChanged(ref _pocoChild, value);
        }

        public int SomeOtherParam
        {
            get => _someOtherParam;
            set => this.RaiseAndSetIfChanged(ref _someOtherParam, value);
        }
    }
}
