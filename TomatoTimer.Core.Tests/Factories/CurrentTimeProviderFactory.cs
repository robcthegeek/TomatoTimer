using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;

namespace TomatoTimer.Core.Tests.Factories
{
    internal class CurrentTimeProviderFactory
    {
        public ICurrentTimeProvider ThatWorks()
        {
            return ThatReturns(DateTime.Now);
        }

        public ICurrentTimeProvider ThatReturns(DateTime value)
        {
            var mock = new Mock<ICurrentTimeProvider>();
            mock.Setup(x => x.Now).Returns(value);
            return mock.Object;
        }

        public Mock<ICurrentTimeProvider> MockThatReturns(DateTime value)
        {
            var mock = new Mock<ICurrentTimeProvider>();
            mock.Setup(x => x.Now).Returns(value);
            return mock;
        }
    }
}
