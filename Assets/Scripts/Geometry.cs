using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Pacman3D
{
	public class Point3D
	{
		public float[] coord = new float[3]  { 0, 0, 0};

        public float x
        {
            get { return coord[0]; }
            set { coord[0] = value; }
        }
        public float y
        {
            get { return coord[1]; }
            set { coord[1] = value; }
        }
        public float z
        {
            get { return coord[2]; }
            set { coord[2] = value; }
        }

        // construct function
        public Point3D(float x = 0, float y = 0, float z = 0)
        {
            coord[0] = x;
            coord[1] = y;
            coord[2] = z;
        }
       
		public Point3D(Point3D point3d)
	    {
		    coord[0] = point3d.coord[0];
		    coord[1] = point3d.coord[1];
		    coord[2] = point3d.coord[2];
	    }

        public Point3D(Vector3 p)
        {
            coord[0] = p.x;
            coord[1] = p.y;
            coord[2] = p.z;
        }

        // operator redefine
        public static Point3D operator+(Point3D a, Point3D b)
        {
            Point3D point3d = new Point3D(a.coord[0] + b.coord[0], a.coord[1] + b.coord[1], a.coord[2] + b.coord[2]);
            return point3d;
        }

		// public static Point3D operator += (Point3D point3d_1,Point3D point_3d_2);
		public static Point3D operator-(Point3D a,Point3D b)
        {
            Point3D point3d = new Point3D(a.coord[0] - b.coord[0], a.coord[1] - b.coord[1], a.coord[2] - b.coord[2]);
            return point3d;
        }
		// public static ref Point3D operator -=(const ref Point3D);
		public static Point3D operator-(Point3D a)
        {
            Point3D point3d=new Point3D(-a.coord[0],-a.coord[1],-a.coord[2]);
            return point3d;
        }

		public static Point3D operator*(float k,Point3D p)
        {
            Point3D point3d = new Point3D(k*p.coord[0],k*p.coord[1],k*p.coord[2]);
            return point3d;
        }
		// public static ref Point3D operator*=(float);
		public static Point3D operator/ (float k,Point3D p)
        {
            Point3D point3d=new Point3D(p.coord[0] / k, p.coord[1] / k, p.coord[2] / k);
            return point3d;
        }
        
		public float len()
        {
            return (float)System.Math.Sqrt(len2());
        }
		public float len2()
        {
            return coord[0] * coord[0] + coord[1] * coord[1] + coord[2] * coord[2];
        }
		
        
        public void normalize()
	    {
		    float l = len();
		    coord[0] /= l;
		    coord[1] /= l;
		    coord[2] /= l;
	    }

        public static float distance(Point3D p1, Point3D p2)
        {
            float d = (float)Math.Sqrt((p1.coord[0] - p2.coord[0]) * (p1.coord[0] - p2.coord[0])
                + (p1.coord[1] - p2.coord[1]) * (p1.coord[1] - p2.coord[1])
                + (p1.coord[2] - p2.coord[2]) * (p1.coord[2] - p2.coord[2]));
            return d;
        }
        /*
        public static void rotate(Point3D p, float dr,  Point3D center, Point3D axis)
	    {
		    p.coord[0] -= center.coord[0];
		    p.coord[1] -= center.coord[1];
		    p.coord[2] -= center.coord[2];
		    float q1 = (float)Math.Cos(dr / 2),
			q2 = (float)Math.Sin(dr / 2) * axis.coord[0],
			q3 = (float)Math.Sin(dr / 2) * axis.coord[1],
			q4 = (float)Math.Sin(dr / 2) * axis.coord[2];
		    float tx = p.coord[0], ty = p.coord[1], tz = p.coord[2];
		    p.coord[0] = (q1*q1 + q2*q2 - sqr(q3)
			    - sqr(q4)) * tx + 2 * (q2 * q3 - q1 * q4) * ty
			    + 2 * (q2 * q4 + q1 * q3) * tz;
		    p.coord[1] = 2 * (q2 * q3 + q1 * q4) * tx
			    + (sqr(q1) - sqr(q2) + sqr(q3) - sqr(q4)) * ty
			    + 2 * (q3 * q4 - q1 * q2) * tz;
		    p.coord[2] = 2 * (q2 * q4 - q1 * q3) * tx
			    + 2 * (q3 * q4 + q1 * q2) * ty
			    + (sqr(q1) - sqr(q2) - sqr(q3) + sqr(q4)) * tz;
		    p.coord[0] = p.coord[0] + center.coord[0];
		    p.coord[1] = p.coord[0]+ center.coord[1];
		    p.coord[2] = p.coord[1] + center.coord[2];
	    }*/

        public static void multiByChannel(Point3D a,Point3D b)
	    {
		    a.coord[0] *= b.coord[0];
		    a.coord[1] *= b.coord[1];
		    a.coord[2] *= b.coord[2];
	    }

        public static Point3D crossProduct(Point3D a, Point3D b)
        {
            Point3D point3d = new Point3D(a.coord[1] * b.coord[2] - a.coord[2] * b.coord[1],
			-a.coord[0] * b.coord[2] + a.coord[2] * b.coord[0],
			a.coord[0] * b.coord[1] - a.coord[1] * b.coord[0]);
            return point3d;
        }
    
        public static float dotsProduct(Point3D a,  Point3D b)
        {
            return a.coord[0] * b.coord[0] + a.coord[1] * b.coord[1] + a.coord[2] * b.coord[2];
        }

        
        public static Point3D elemMult(Point3D a, Point3D b)
        {
            Point3D point3d = new Point3D(a.coord[0] * b.coord[0], a.coord[1] * b.coord[1], a.coord[2] * b.coord[2]);
            return point3d;
        }


        public static float determinant(Point3D a, Point3D b, Point3D c)
        {
            return a.coord[0] * b.coord[1] * c.coord[2] - a.coord[0] * b.coord[2] * c.coord[1] +
			a.coord[1] * b.coord[2] * c.coord[0] - a.coord[1] * b.coord[0] * c.coord[2] +
			a.coord[2] * b.coord[0] * c.coord[1] - a.coord[2] * b.coord[1] * c.coord[0];
        }


        public static float calcArea(Point3D a, Point3D b, Point3D c)
        {
            return crossProduct(b - a, c - b).len() / 2;
        }

        public static Point3D FromGamePos(GamePos p)
        {
            return new Point3D(p.x, 0, p.y);
        }
    };

    public class Line
    {
        public Point3D st, ed;
        public Line(Point3D _st, Point3D _ed)
        {
            st = new Point3D(_st);
            ed = new Point3D(_ed);
        }
    }

    // Point3D operator*(float, const ref Point3D );

    public class Triangle
	{

        public Point3D[] points = new Point3D[3];
   
		Point3D normvf; /* normal vector */


        public Triangle(Point3D p1, Point3D p2, Point3D p3)
	    {
		    points[0] = p1;
		    points[1] = p2;
		    points[2] = p3;
		    Point3D ta = p2 - p1, tb = p3 - p1;
		    float xy = ta.coord[0] * tb.coord[1] - ta.coord[1] * tb.coord[0];
		    float xz = ta.coord[0] * tb.coord[2] - ta.coord[2] * tb.coord[0];
		    float yz = ta.coord[1] * tb.coord[2] - ta.coord[2] * tb.coord[1];
            Point3D point3d = new Point3D(yz,-xz,xy);
		    normvf = point3d;
            normvf.normalize();
	    }

        /*
        public void rotate(Point3D p, float dr, Point3D center, Point3D axis)
	    {
		    points[0].rotate(dr, center, axis);
		    points[1].rotate(dr, center, axis);
		    points[2].rotate(dr, center, axis);
		    normvf.rotate(dr, center, axis);
	    }*/
	};
	public class Rectangle
	{
        public Point3D[] points = new Point3D[2];
		Rectangle(Point3D p1,  Point3D p2)
        {
            points[0]=p1;
            points[1]=p2;
        }
        // public static void RectangleIntersect(Rectangle r1,Rectangle r2);
        
	
	};
	public class Circle
	{
		public Point3D c;
		public float r;
		public Circle(Point3D _c, float _r = 0)
        {
            c=_c;
            r=_r;
        }
        
        public static bool CircleIntersect(Circle c1,Circle c2)
        {
            /*float distance=(float)Math.Sqrt((c1.c.coord[0]-c2.c.coord[0])*(c1.c.coord[0]-c2.c.coord[0])
                +(c1.c.coord[1]-c2.c.coord[1])*(c1.c.coord[1]-c2.c.coord[1])
                +(c1.c.coord[2]-c2.c.coord[2])*(c1.c.coord[2]-c2.c.coord[2]));*/
            if (Point3D.distance(c1.c, c2.c) >= c1.r + c2.r) return true;
            else return false;
        }

        static float PLdistance(Point3D c, Point3D a, Point3D b)
        {
            Point3D ab = b - a;

            Point3D ac = c - a;

            float f = Point3D.dotsProduct(ab, ac);

            if (f + FloatCmp.EPS < 0) return Point3D.distance(c, a);

            float d = Point3D.dotsProduct(ab, ab);

            if (f > d) return Point3D.distance(c, b);

            f = f / d;

            Point3D D = a + f * ab;   // c在ab线段上的投影点

            return Point3D.distance(c, D);
        }

        public static bool RCIntersect(Rectangle r,Circle c)
        {
            float dis1 = PLdistance(c.c, new Point3D(r.points[0].coord[0], r.points[0].coord[1]), new Point3D(r.points[0].coord[0], r.points[1].coord[1]));
            float dis2 = PLdistance(c.c, new Point3D(r.points[0].coord[0], r.points[1].coord[1]), new Point3D(r.points[1].coord[0], r.points[1].coord[1]));
            float dis3 = PLdistance(c.c, new Point3D(r.points[1].coord[0], r.points[1].coord[1]), new Point3D(r.points[1].coord[0], r.points[0].coord[1]));
            float dis4 = PLdistance(c.c, new Point3D(r.points[1].coord[0], r.points[0].coord[1]), new Point3D(r.points[0].coord[0], r.points[0].coord[1]));
            float distance1 = (float)Math.Sqrt((c.c.coord[0] - r.points[0].coord[0])*(c.c.coord[0] - r.points[0].coord[0])
                +(c.c.coord[1]-r.points[0].coord[1])*(c.c.coord[1]-r.points[0].coord[1]));
            float distance2 = (float)Math.Sqrt((c.c.coord[0] - r.points[1].coord[0])*(c.c.coord[0] - r.points[1].coord[0])
                +(c.c.coord[1]-r.points[1].coord[1])*(c.c.coord[1]-r.points[1].coord[1]));
            float distance=Math.Min(distance1,distance2);
            return distance > c.r;
         }
	};

    public class FloatCmp
    {
        public const float EPS = 1e-5f;
        public const float INF = 1e10f;

        /* x > y  -->  +1
	    x == y -->   0
	    x < y  -->  -1  */
        public static int cmp(float x, float y = 0)
        {
            x -= y;
            return (x > EPS ? 1 : 0) - (x < -EPS ? 1 : 0);
        }
    }
	
}