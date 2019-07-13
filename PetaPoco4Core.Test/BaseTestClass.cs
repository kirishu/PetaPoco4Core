using Xunit.Abstractions;

namespace PetaPoco4Core.Test
{
    /// <summary>
    /// テスト基底クラス
    /// </summary>
    public class BaseTestClass
    {
        protected readonly ITestOutputHelper _output;

        public BaseTestClass(ITestOutputHelper output)
        {
            _output = output;
        }
    }
}
