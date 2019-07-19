using System;
using System.Collections.Generic;
using System.Linq;
using Xunit.Abstractions;

namespace PetaPoco4Core.Test
{
    /// <summary>
    /// テスト基底クラス
    /// </summary>
    public class TestBase
    {
        protected readonly ITestOutputHelper _output;

        public TestBase(ITestOutputHelper output)
        {
            _output = output;
        }

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
