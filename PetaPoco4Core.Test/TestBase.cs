using System;
using System.Collections.Generic;
using System.Linq;
using Xunit.Abstractions;

namespace PetaPoco4Core.Test
{
    /// <summary>
    /// テスト基底クラス
    /// </summary>
    public abstract class TestBase : IDisposable
    {
        protected readonly ITestOutputHelper _output;

        private TestBase() { }

        /// <summary>
        /// テストクラスの初期処理
        /// </summary>
        /// <param name="output"></param>
        /// <param name="testCommon"></param>
        public TestBase(ITestOutputHelper output)
        {
            _output = output;
            Initialize();
        }

        public void Dispose()
        {
            Cleanup();
        }

        internal virtual void Initialize() { }
        internal virtual void Cleanup() { }
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
