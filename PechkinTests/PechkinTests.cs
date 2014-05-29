using Pechkin;

namespace PechkinTests
{
    public class PechkinTests : PechkinAbstractTests<IPechkin>
    {
        protected override IPechkin ProduceTestObject(GlobalSettings cfg)
        {
            return Factory.Create(cfg);
        }

        protected override void TestEnd()
        {
            
        }
    }
}
