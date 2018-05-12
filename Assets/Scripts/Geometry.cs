using System;
using System.Collections.Generic;
using System.Linq;

namespace Pacman3D
{
    class epsinf
    {
        const float EPS = 1e-5f;
	    const float INF = 1e10f;
    }
	
	public class Point3D
	{
		// float value[3];
		float[] value = new float[3]  { 0, 0, 0};

		// construct function
		public Point3D(float x = 0, float y = 0, float z = 0)
        {
            value[0] = x;
            value[1] = y;
            value[2] = z;
        }
       
		public Point3D(ref Point3D point3d)
	    {
		    value[0] = point3d.value[0];
		    value[1] = point3d.value[1];
		    value[2] = point3d.value[2];
	    }

		// operator redefine
		public static Point3D operator+(Point3D a, Point3D b)
        {
            Point3D point3d = new Point3D(a.value[0] + b.value[0], a.value[1] + b.value[1], a.value[2] + b.value[2]);
            return point3d;
        }

		// public static Point3D operator += (Point3D point3d_1,Point3D point_3d_2);
		public static Point3D operator-(Point3D a,Point3D b)
        {
            Point3D point3d = new Point3D(a.value[0] - b.value[0], a.value[1] - b.value[1], a.value[2] - b.value[2]);
            return point3d;
        }
		// public static ref Point3D operator -=(const ref Point3D);
		public static Point3D operator-(Point3D a)
        {
            Point3D point3d=new Point3D(-a.value[0],-a.value[1],-a.value[2]);
            return point3d;
        }

		public static Point3D operator*(float k,Point3D p)
        {
            Point3D point3d = new Point3D(k*p.value[0],k*p.value[1],k*p.value[2]);
            return point3d;
        }
		// public static ref Point3D operator*=(float);
		public static Point3D operator/ (float k,Point3D p)
        {
            Point3D point3d=new Point3D(p.value[0] / k, p.value[1] / k, p.value[2] / k);
            return point3d;
        }
        
		public float len()
        {
            return (float)System.Math.Sqrt(len2());
        }
		public float len2()
        {
            return sqr(p.value[0])+sqr(p.value[1])+sqr(p.value[2]);
        }
		
        
        public void normalize()
	    {
		    float l = len();
		    value[0] /= l;
		    value[1] /= l;
		    value[2] /= l;
	    }

        public static void rotate(Point3D p, float dr,  Point3D center, Point3D axis)
	    {
		    p.value[0] -= center.value[0];
		    p.value[1] -= center.value[1];
		    p.value[2] -= center.value[2];
		    float q1 = cos(dr / 2),
			q2 = sin(dr / 2) * axis.value[0],
			q3 = sin(dr / 2) * axis.value[1],
			q4 = sin(dr / 2) * axis.value[2];
		    float tx = p.value[0], ty = p.value[1], tz = p.value[2];
		    p.value[0] = (sqr(q1) + sqr(q2) - sqr(q3)
			    - sqr(q4)) * tx + 2 * (q2 * q3 - q1 * q4) * ty
			    + 2 * (q2 * q4 + q1 * q3) * tz;
		    p.value[1] = 2 * (q2 * q3 + q1 * q4) * tx
			    + (sqr(q1) - sqr(q2) + sqr(q3) - sqr(q4)) * ty
			    + 2 * (q3 * q4 - q1 * q2) * tz;
		    p.value[2] = 2 * (q2 * q4 - q1 * q3) * tx
			    + 2 * (q3 * q4 + q1 * q2) * ty
			    + (sqr(q1) - sqr(q2) - sqr(q3) + sqr(q4)) * tz;
		    p.value[0] = p.value[0] + center.value[0];
		    p.value[1] = p.value[0]+ center.value[1];
		    p.value[2] = p.value[1] + center.value[2];
	    }
        
        public static void multiByChannel(Point3D a,Point3D b)
	    {
		    a.value[0] *= b.value[0];
		    a.value[1] *= b.value[1];
		    a.value[2] *= b.value[2];
	    }
    
        static Point3D crossProduct(ref Point3D a, ref Point3D b)
        {
            Point3D point3d = new Point3D(a.value[1] * b.value[2] - a.value[2] * b.value[1],
			-a.value[0] * b.value[2] + a.value[2] * b.value[0],
			a.value[0] * b.value[1] - a.value[1] * b.value[0]);
            return point3d;
        }
    
        static float dotsProduct(ref Point3D a,  ref Point3D b)
        {
            return a.value[0] * b.value[0] + a.value[1] * b.value[1] + a.value[2] * b.value[2];
        }

        
        static Point3D elemMult(ref Point3D a, ref Point3D b)
        {
            Point3D point3d = new Point3D(a.value[0] * b.value[0], a.value[1] * b.value[1], a.value[2] * b.value[2]);
            return point3d;
        }
    
     
        static float determinant(ref Point3D a, ref Point3D b, ref Point3D c)
        {
            return a.value[0] * b.value[1] * c.value[2] - a.value[0] * b.value[2] * c.value[1] +
			a.value[1] * b.value[2] * c.value[0] - a.value[1] * b.value[0] * c.value[2] +
			a.value[2] * b.value[0] * c.value[1] - a.value[2] * b.value[1] * c.value[0];
        }

    
        static float calcArea(ref Point3D a, ref Point3D b, ref Point3D c)
        {
            return crossProduct(b - a, c - b).len() / 2;
        }
    
	
	};
	
    // Point3D operator*(float, const ref Point3D );

	public class Triangle
	{
	
        Point3D[] points=new Point3D[3];
   
		Point3D normvf; /* normal vector */


        public Triangle(Point3D p1, Point3D p2, Point3D p3)
	    {
		    points[0] = p1;
		    points[1] = p2;
		    points[2] = p3;
		    Point3D ta = p2 - p1, tb = p3 - p1;
		    float xy = ta.value[0] * tb.value[1] - ta.value[1] * tb.value[0];
		    float xz = ta.value[0] * tb.value[2] - ta.value[2] * tb.value[0];
		    float yz = ta.value[1] * tb.value[2] - ta.value[2] * tb.value[1];
            Point3D point3d = new Point3D(yz,-xz,xy);
		    normvf =point3d;
		    normvf /= normvf.len();
	    }

        public static void rotate(Point3D p, float dr, Point3D center, Point3D axis)
	    {
		    points[0].rotate(dr, center, axis);
		    points[1].rotate(dr, center, axis);
		    points[2].rotate(dr, center, axis);
		    normvf.rotate(dr, center, axis);
	    }
	};
	public class Rectangle
	{
        Point3D[] points = new Point3D[2] ;
		Rectangle( ref Point3D p1,  ref Point3D p2)
        {
            points[0]=p1;
            points[1]=p2;
        }
        // public static void RectangleIntersect(Rectangle r1,Rectangle r2);
        
	
	};
	public class Circle
	{
		Point3D c;
		float r;
		Circle(Point3D _c, float _r = 0)
        {
            c=_c;
            r=_r;
        }
        
        public static bool CircleIntersect(Circle c1,Circle c2)
        {
            float distance=sqrt((c1.c.value[0]-c2.c.value[0])*(c1.c.value[0]-c2.c.value[0])
                +(c1.c.value[1]-c2.c.value[1])*(c1.c.value[1]-c2.c.value[1])
                +(c1.c.value[2]-c2.c.value[2])*(c1.c.value[2]-c2.c.value[2]));
            if(distance(c1.c,c2.c)>=c1.r+c2.r) return true;
            else return false;
        }

        public static bool RCIntersect(Rectangle r,Circle c)
        {
            float distance1 = sqrt((c.c.value[0] - r.points[0].value[0])*(c.c.value[0] - r.points[0].value[0])
                +(c.c.value[1]-r.points[0].value[1])*(c.c.value[1]-r.points[0].value[1]));
            float distance2 = sqrt((c.c.value[0] - r.points[1].value[0])*(c.c.value[0] - r.points[1].value[0])
                +(c.c.value[1]-r.points[1].value[1])*(c.c.value[1]-r.points[1].value[1]));
            float distance=min(distance1,distance2);
            return distance > c.r;
         }
	};
	/* x > y  -->  +1
	x == y -->   0
	x < y  -->  -1  */
	delegate int dcmp(float x, float y = 0)
	{
		x -= y;
		return int(x > EPS) - int(x < -EPS);
	}

	// C# template
	template<class T> inline T sqr(T x)
	{
		return x * x;
	}
}