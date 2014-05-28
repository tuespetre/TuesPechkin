using Pechkin;

namespace PechkinTests
{
    public class PechkinTests : PechkinAbstractTests<IPechkin>
    {
        protected override IPechkin ProduceTestObject(GlobalConfig cfg)
        {
            return Factory.Create(cfg);
        }

        protected override void TestEnd()
        {
            
        }
    }
}
