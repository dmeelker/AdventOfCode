using Xunit;
using Range = Solution.Range;

namespace Tests
{
    public class RangeTests
    {
        [Fact]
        public void Split()
        {
            var range = new Range(0, 10);
            var results = range.Split(5);

            Assert.Equal(new Range(0, 5), results[0]);
            Assert.Equal(new Range(5, 5), results[1]);
        }

        [Fact]
        public void Split2()
        {
            var range = new Range(10, 90);
            var results = range.Split(30);

            Assert.Equal(new Range(10, 20), results[0]);
            Assert.Equal(new Range(30, 70), results[1]);
        }

        [Fact]
        public void Intersects_CompletelyWithin()
        {
            var range1 = new Range(0, 100);
            var range2 = new Range(10, 20);

            Assert.True(range1.Intersects(range2));
        }

        [Fact]
        public void Intersects_CompletelyOutside()
        {
            var range1 = new Range(0, 100);
            var range2 = new Range(200, 20);

            Assert.False(range1.Intersects(range2));
        }

        [Fact]
        public void Intersects_SideBySide()
        {
            var range1 = new Range(0, 100);
            var range2 = new Range(100, 200);

            Assert.False(range1.Intersects(range2));
        }

        [Fact]
        public void IntersectCenter()
        {
            var range1 = new Range(0, 100);
            var range2 = new Range(10, 20);

            var intersect = range1.Intersect(range2);

            Assert.Equal(new Range(10, 20), intersect.Intersection);
            Assert.Equal(new Range(0, 10), intersect.Remainder[0]);
            Assert.Equal(new Range(30, 70), intersect.Remainder[1]);
        }

        [Fact]
        public void IntersectRight()
        {
            var range1 = new Range(0, 100);
            var range2 = new Range(90, 50);

            var intersect = range1.Intersect(range2);

            Assert.Equal(new Range(90, 10), intersect.Intersection);
            Assert.Equal(new Range(0, 90), intersect.Remainder[0]);
        }

        [Fact]
        public void IntersectLeft_Partial()
        {
            var range1 = new Range(50, 100);
            var range2 = new Range(0, 100);

            var intersect = range1.Intersect(range2);

            Assert.Equal(new Range(50, 50), intersect.Intersection);
            Assert.Equal(new Range(100, 50), intersect.Remainder[0]);
        }

        [Fact]
        public void IntersectLeft_Fully()
        {
            var range1 = new Range(50, 100);
            var range2 = new Range(0, 150);

            var intersect = range1.Intersect(range2);

            Assert.Equal(new Range(50, 100), intersect.Intersection);
            Assert.Empty(intersect.Remainder);
        }

        [Fact]
        public void IntersectOtherContainsCompletely()
        {
            var range1 = new Range(50, 100);
            var range2 = new Range(0, 200);

            var intersect = range1.Intersect(range2);

            Assert.Equal(new Range(50, 100), intersect.Intersection);
            Assert.Empty(intersect.Remainder);
        }

        [Fact]
        public void IntersectGluedLeft()
        {
            var range1 = new Range(0, 100);
            var range2 = new Range(0, 50);

            var intersect = range1.Intersect(range2);

            Assert.Equal(new Range(0, 50), intersect.Intersection);
            Assert.Equal(new Range(50, 50), intersect.Remainder[0]);
        }

        [Fact]
        public void IntersectGluedRight()
        {
            var range1 = new Range(0, 100);
            var range2 = new Range(50, 50);

            var intersect = range1.Intersect(range2);

            Assert.Equal(new Range(50, 50), intersect.Intersection);
            Assert.Equal(new Range(0, 50), intersect.Remainder[0]);
        }
    }
}
