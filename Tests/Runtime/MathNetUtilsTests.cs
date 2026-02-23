using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Spatial.Euclidean;
using NUnit.Framework;
using UnityEngine;
using WindowFracture.Runtime;

namespace Titan.Windowfracture.Tests
{
    public class MathNetUtilsTests
    {
        [Test]
        public void CompareVectorAngle_ReturnsCounterClockwise()
        {
            var origin = new Point2D(0, 0);
            var a = new Point2D(1, 0);
            var b = new Point2D(0, 1);

            Assert.That(MathNetUtils.CompareVectorAngle(origin, a, b), Is.EqualTo(1));
        }

        [Test]
        public void CompareVectorAngle_ReturnsClockwise()
        {
            var origin = new Point2D(0, 0);
            var a = new Point2D(0, 1);
            var b = new Point2D(1, 0);

            Assert.That(MathNetUtils.CompareVectorAngle(origin, a, b), Is.EqualTo(-1));
        }

        [Test]
        public void CompareVectorAngle_ReturnsCollinear()
        {
            var origin = new Point2D(0, 0);
            var a = new Point2D(1, 0);
            var b = new Point2D(2, 0);

            Assert.That(MathNetUtils.CompareVectorAngle(origin, a, b), Is.EqualTo(0));
        }

        [Test]
        public void BarycentricInterpolation_OnTriangleVertex_ReturnsUnitWeight()
        {
            var triangle = new[]
            {
                new Point2D(0, 0),
                new Point2D(1, 0),
                new Point2D(0, 1)
            };

            Vector3 bary = MathNetUtils.BarycentricInterpolation(new Point2D(0, 0), triangle);

            Assert.That(bary.x, Is.EqualTo(1f).Within(1e-5f));
            Assert.That(bary.y, Is.EqualTo(0f).Within(1e-5f));
            Assert.That(bary.z, Is.EqualTo(0f).Within(1e-5f));
        }

        [Test]
        public void BarycentricInterpolation_InsideTriangle_WeightsSumToOne()
        {
            var triangle = new[]
            {
                new Point2D(0, 0),
                new Point2D(2, 0),
                new Point2D(0, 2)
            };

            Vector3 bary = MathNetUtils.BarycentricInterpolation(new Point2D(0.5, 0.5), triangle);

            Assert.That(bary.x + bary.y + bary.z, Is.EqualTo(1f).Within(1e-5f));
            Assert.That(bary.x, Is.GreaterThan(0f));
            Assert.That(bary.y, Is.GreaterThan(0f));
            Assert.That(bary.z, Is.GreaterThan(0f));
        }

        [Test]
        public void Point2DComparer_UsesTolerance()
        {
            var a = new MathNetUtils.IndexedPoint(new Vector3(1f, 1f, 0f), 0);
            var b = new MathNetUtils.IndexedPoint(new Vector3(1.0005f, 1.0005f, 0f), 1);
            var c = new MathNetUtils.IndexedPoint(new Vector3(1.1f, 1.1f, 0f), 2);
            var comparer = new MathNetUtils.Point2DComparer(0.001);

            Assert.That(comparer.Equals(a, b), Is.True);
            Assert.That(comparer.Equals(a, c), Is.False);
        }

        [Test]
        public void IndexedPoint_TransformBy_AppliesScale()
        {
            var point = new MathNetUtils.IndexedPoint(new Vector3(2f, 3f, 0f), 0);
            var matrix = new DiagonalMatrix(2, 2, new[] { 2d, 4d });
            point.TransformBy(matrix);

            Assert.That(point.X, Is.EqualTo(4d).Within(1e-8));
            Assert.That(point.Y, Is.EqualTo(12d).Within(1e-8));
        }
    }
}
