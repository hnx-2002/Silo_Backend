using netDxf;
using System;
using System.Collections.Generic;

namespace TPFun_Cad
{
    /// <summary>
    /// 二维坐标
    /// </summary>
    public class Point2D
    {
        /// <summary>
        /// 二维坐标(x,y)
        /// </summary>
        /// <param name="inX"></param>
        /// <param name="inY"></param>
        public Point2D(double inX, double inY)
        {
            X = inX;
            Y = inY;
        }

        /// <summary>
        /// 初值（0,0）
        /// </summary>
        public Point2D()
        {
            X = 0;
            Y = 0;
        }

        /// <summary>
        /// X
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// Y
        /// </summary>
        public double Y { get; set; }

        /// <summary>
        /// Vector2转Point2D
        /// </summary>
        /// <param name="inVect"></param>
        public static Point2D TransPoint2D(Vector2 inVect)
        {
            return new Point2D(inVect.X, inVect.Y);
        }

        /// <summary>
        /// Vector3转Point2D
        /// </summary>
        /// <param name="inVect"></param>
        public static Point2D TransPoint2D(Vector3 inVect)
        {
            return new Point2D(inVect.X, inVect.Y);
        }

        /// <summary>
        /// Point2D转Vector2
        /// </summary>
        /// <param name="inPoint"></param>
        public static Vector2 TransVector2(Point2D inPoint)
        {
            return new Vector2(inPoint.X, inPoint.Y);
        }
    }
}