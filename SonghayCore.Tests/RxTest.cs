using Microsoft.VisualStudio.TestTools.UnitTesting;
using Songhay.Extensions;
using System;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Songhay.Tests
{
    [TestClass]
    public class RxTest
    {
        [TestInitialize]
        public void InitializeTest()
        {
            this.TestContext.RemovePreviousTestResults();
        }

        public TestContext TestContext { get; set; }

        [TestMethod]
        public void ShouldCountOnSubscribe()
        {
            var observable = Observable.Return<string>("Hello world!");
            observable
                .Count()
                .Subscribe(i => System.Diagnostics.Debug.WriteLine("observable.Count(): {0}", i));
            observable.Wait();
        }

        [TestMethod]
        public void ShouldGetHeartBeats()
        {
            //FUNKYKB: this try-finally block is not required because AutoDetachObserver<T> is returned.
            IDisposable subscriptionToken = null;
            try
            {
                var observable = this.GetHeartBeat();
                subscriptionToken = observable.Subscribe(i => System.Diagnostics.Debug.WriteLine("GetHeartBeat(): {0}", i));
                observable.Wait();
            }

            finally
            {
                subscriptionToken.Dispose();
            }
        }

        [TestMethod]
        public void ShouldGetHeartBeatsWithCount()
        {
            var observable = this.GetHeartBeat();
            observable.Subscribe(i => System.Diagnostics.Debug.WriteLine("GetHeartBeat(): {0}", i));
            observable.Count().Subscribe(i => System.Diagnostics.Debug.WriteLine("observable.Count(): {0}", i));
            observable.Wait();
        }

        [TestMethod]
        public void ShouldGetHeartBeatsWithTimeInterval()
        {
            var observable = this.GetHeartBeat().TimeInterval();
            observable.Subscribe(i => System.Diagnostics.Debug.WriteLine("GetHeartBeat(): {0}", i));
            observable.Wait();
        }

        [TestMethod]
        public void ShouldGetHeartBeatsWithTimeIntervalAndBuffer()
        {
            var observable = this.GetHeartBeat()
                .TimeInterval()
                .Buffer(3, 1)
                .Select((l, i) => string.Format("{0}, {1}", l.Average(x => 60 / x.Interval.TotalSeconds), i));
            observable.Subscribe(i => System.Diagnostics.Debug.WriteLine(i));
            observable.Wait();
        }

        IObservable<int> GetHeartBeat()
        {
            return Observable.Create<int>((observer, cancelToken) => this.Start(observer, cancelToken));
        }

        async Task Start(IObserver<int> observer, CancellationToken cancelToken)
        {
            int beat = 0;
            var random = new Random();
            while (beat < 10)
            {
                await Task.Delay(random.Next(500) + 700, cancelToken);
                observer.OnNext(beat);
                beat++;
            }
        }
    }
}
