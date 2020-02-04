using System;
using System.Collections.Generic;
using System.Linq;
using Xunit.Abstractions;

namespace PetaPoco4Core.Test
{
    /// <summary>
    /// テスト基底クラス
    /// </summary>
    public class TestBase: IDisposable
    {
        protected readonly ITestOutputHelper _output;
        protected readonly ITestCommon _testCommon;

        public TestBase(ITestOutputHelper output)
        {
            _output = output;
        }

        public TestBase(ITestOutputHelper output, ITestCommon testCommon)
        {
            _output = output;
            // setup
            _testCommon = testCommon;
            _testCommon.Initialize();
        }

        public void Dispose()
        {
            // teardown
            if (_testCommon != null)
            {
                _testCommon.Cleanup();
            }
        }
    }

    public interface ITestCommon
    {
        void Initialize();
        void Cleanup();
    }

    /// <summary>
    /// 拡張メソッド
    /// </summary>
    public static class IEnumerableUtil
    {
        public static string JoinString<T>(this IEnumerable<T> values, string glue, Func<T, string> converter = null)
        {
            if (converter != null)
            {
                return string.Join(glue, values.Select(converter));
            }
            else
            {
                return string.Join(glue, values);
            }
        }
    }


}
