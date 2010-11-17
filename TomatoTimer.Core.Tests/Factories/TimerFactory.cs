using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;

namespace TomatoTimer.Core.Tests.Factories
{
    internal class TimerFactory
    {
        public ITimer ThatWorks()
        {
            var mock = new Mock<ITimer>();
            mock.Setup(x => x.Start()).Callback(() => mock.Setup(x => x.IsEnabled).Returns(true));
            mock.Setup(x => x.Stop()).Callback(() => mock.Setup(x => x.IsEnabled).Returns(false));
            return mock.Object;
        }
    }
}
