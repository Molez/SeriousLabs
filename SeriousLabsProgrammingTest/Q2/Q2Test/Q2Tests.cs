using NUnit.Framework;
using Q2App;

namespace Q2Test
{
    [TestFixture]
    public class Q2Tests
    {
        [Test]
        public void DotProduct()
        {
            var a = new Vec3(1, 2, 3);
            var b = new Vec3(1, 5, 7);

            var result = Q2.Dot(a, b);

            Assert.AreEqual(32, result);
        }

        [Test]
        public void Add()
        {
            var a = new Vec3(4, 2, -1);
            var b = new Vec3(-1, 2, 3);

            var result = Q2.Add(a, b);

            Assert.AreEqual(new Vec3(3, 4, 2), result);
        }

        [Test]
        public void Subtract()
        {
            var a = new Vec3(4, 2, -1);
            var b = new Vec3(-1, 2, 3);

            var result = Q2.Subtract(a, b);

            Assert.AreEqual(new Vec3(5, 0, -4), result);
        }

        [Test]
        public void Normalize()
        {
            var a = new Vec3(3, -4, 0);
            var result = Q2.Normalize(a);

            Assert.AreEqual(new Vec3(0.6, -0.8, 0), result);
        }

        [Test]
        public void Magnitude()
        {
            var a = new Vec3(3, -4, 0);

            var result = Q2.Magnitude(a);

            Assert.AreEqual(5, result);
        }

        [Test]
        public void ScalarMultiplication()
        {
            var a = new Vec3(3, -2, 6);

            var result = Q2.ScalarMultiplication(a, 5);

            Assert.AreEqual(new Vec3(15, -10, 30), result);
        }

        [Test]
        public void Clamp()
        {
            Assert.AreEqual(2, Q2.Clamp(2, 1, 5));
            Assert.AreEqual(5, Q2.Clamp(2, 5, 8));
            Assert.AreEqual(8, Q2.Clamp(10, 1, 8));
        }

        [Test]
        public void ImplementationEquivalency()
        {
            var a = new Vec3(2, -5, 8);
            var b = new Vec3(5, 3, 7);
            var point = new Vec3(2, -1, 3);

            var result = Q2.GetNearestPointOnSegment(point, a, b);
            var result2 = Q2.GetNearestPointOnSegment2(point, a, b);
            var result3 = Q2.GetNearestPointOnSegment3(point, a, b);
            var result4 = Q2.GetNearestPointOnSegment3(point, a, b);

            //Assert.AreEqual(result, result2);
            Assert.AreEqual(result2, result3);
            Assert.AreEqual(result3, result4);
        }

        [Test]
        public void ImplementationEquivalency2()
        {
            var a = new Vec3(1, 2, 0);
            var b = new Vec3(3, 2, 0);
            var point = new Vec3(2, 4, 0);

            var result = Q2.GetNearestPointOnSegment(point, a, b);
            var result2 = Q2.GetNearestPointOnSegment2(point, a, b);
            var result3 = Q2.GetNearestPointOnSegment3(point, a, b);
            var result4 = Q2.GetNearestPointOnSegment4(point, a, b);

            Assert.AreEqual(new Vec3(2 ,2, 0), result);
            Assert.AreEqual(new Vec3(2, 2, 0), result2);
            Assert.AreEqual(new Vec3(2, 2, 0), result3);
            Assert.AreEqual(new Vec3(2, 2, 0), result4);
        }

        [Test]
        public void ImplementationEquivalency3()
        {
            var a = new Vec3(1, 2, 0);
            var b = new Vec3(3, 2, 0);
            var point = new Vec3(-1, 4, 0);

            var result = Q2.GetNearestPointOnSegment(point, a, b);
            var result2 = Q2.GetNearestPointOnSegment2(point, a, b);
            var result3 = Q2.GetNearestPointOnSegment3(point, a, b);
            var result4 = Q2.GetNearestPointOnSegment4(point, a, b);

            //Assert.AreEqual(new Vec3(1, 2, 0), result);
            Assert.AreEqual(new Vec3(1, 2, 0), result2);
            //Assert.AreEqual(new Vec3(1, 2, 0), result3);
            Assert.AreEqual(new Vec3(1, 2, 0), result4);
        }

        [Test]
        public void ImplementationEquivalency4()
        {
            var a = new Vec3(1, 2, 0);
            var b = new Vec3(3, 2, 0);
            var point = new Vec3(5, 4, 0);

            var result = Q2.GetNearestPointOnSegment(point, a, b);
            var result2 = Q2.GetNearestPointOnSegment2(point, a, b);
            var result3 = Q2.GetNearestPointOnSegment3(point, a, b);
            var result4 = Q2.GetNearestPointOnSegment4(point, a, b);

            //Assert.AreEqual(new Vec3(3, 2, 0), result);
            Assert.AreEqual(new Vec3(3, 2, 0), result2);
            //Assert.AreEqual(new Vec3(3, 2, 0), result3); //Returns the unbounded result
            Assert.AreEqual(new Vec3(3, 2, 0), result4);
        }

        [Test]
        public void ImplementationEquivalency5()
        {
            var a = new Vec3(0, 1, 2);
            var b = new Vec3(0, 3, 2);
            var point = new Vec3(0, 2, 4);

            var result = Q2.GetNearestPointOnSegment(point, a, b);
            var result2 = Q2.GetNearestPointOnSegment2(point, a, b);
            var result3 = Q2.GetNearestPointOnSegment3(point, a, b);
            var result4 = Q2.GetNearestPointOnSegment4(point, a, b);

            Assert.AreEqual(new Vec3(0, 2, 2), result);
            Assert.AreEqual(new Vec3(0, 2, 2), result2);
            Assert.AreEqual(new Vec3(0, 2, 2), result3);
            Assert.AreEqual(new Vec3(0, 2, 2), result4);
        }
    }
}
