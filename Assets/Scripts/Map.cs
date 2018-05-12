using System;

namespace Pacman3D
{
    public class GamePos
	{
		public int x;
		public int y;
		public GamePos(int _x = 0, int _y = 0) {
            x = _x;
            y = _y;
        }
        public GamePos(Point3D p)
        {
            x = (int)p.x;
            y = (int)p.y;
        }
	}
	

	public class GameMap
    {
        public const int Empty = 0;
        public const int Wall = 1;
        public const int Player = 2;
        public const int Bean = 3;
        public const int Monster = 4;
        public const int Obstacle = 5;
        public const float rate = 1.0f; // Game / World
        public const float x_bias = 0.0f;
        public const float y_bias = 0.0f;
        public const int Monster_Num = 2;
        public int n,m;
        public int[,] t;
		public GameMap(int _n = 0, int _m = 0)
        {
            n = _n;
            m = _m;
            t = new int[n, m];
            for (int i = 0; i < n; ++i)
            {
                for (int j = 0; j < m; ++j)
                {
                    t[i,j] = Empty;
                }
            }
        }
		
		
		public GamePos worldToGame(Point3D worldPos)
        {
            float x = worldPos.x, y = worldPos.y;
            x = x * rate + x_bias;
            y = y * rate + y_bias;
            return new GamePos((int) x , (int) y);
        }
		
		public Point3D gameToWorld(GamePos gamePos, float height)
        {
            float x = gamePos.x, y = gamePos.y;
            x = (x - x_bias) / rate;
            y = (y - y_bias) / rate;
            return new Point3D(x, y, height);
        }

        private void setType(GamePos p, int _t)
        {
            t[p.x, p.y] = _t;
        }
        private void buildWallUp(int x, int y)
        {
            setType(new GamePos(x, y), Wall);
            setType(new GamePos(x - 1, y), Wall);
            setType(new GamePos(x - 2, y), Wall);

        }
        private void buildWallLeft(int x, int y)
        {
            setType(new GamePos(x, y), Wall);
            setType(new GamePos(x, y - 1), Wall);
            setType(new GamePos(x, y - 2), Wall);
        }
        private Point3D pointShadow(Point3D worldPos) {
            float x = worldPos.x, y = worldPos.y;
            x = x* rate + x_bias;
		    y = y* rate + y_bias;
		    return new Point3D(x, y, 0.0f);
        }
        private float cross(Point3D a, Point3D b){
	    	return a.x* b.y - a.y* b.x;
        }

        private bool inTriangle(Point3D p, Triangle t){
            Point3D p0 = pointShadow(t.points[0]), p1 = pointShadow(t.points[1]), p2 = pointShadow(t.points[2]);
            float Sabc = System.Math.Abs(cross(p1 - p0, p2 - p0));
            float Spab = System.Math.Abs(cross(p0 - p, p1 - p));
            float Spac = System.Math.Abs(cross(p0 - p, p2 - p));
            float Spbc = System.Math.Abs(cross(p1 - p, p2 - p));
            return System.Math.Abs(Sabc - Spab - Spac - Spbc) < FloatCmp.EPS;
        }
		
        private void addTriangle(Triangle T)
        {
            for (int i = 0; i < n; ++i)
            {
                for (int j = 0; j < m; ++j)
                {
                    if (t[i,j] == Obstacle) continue;

                    float x = (float)i, y = (float)j;
                    Point3D p = new Point3D(x + FloatCmp.EPS, y + FloatCmp.EPS);
                    if (inTriangle(p, T))
                    {
                        setType(new GamePos(i, j), Obstacle);
                        continue;
                    }
                    p = new Point3D(x + 1 - FloatCmp.EPS, y + 1 - FloatCmp.EPS);
                    if (inTriangle(p, T))
                    {
                        setType(new GamePos(i, j), Obstacle);
                        continue;
                    }
                    p = new Point3D(x + 1 - FloatCmp.EPS, y + FloatCmp.EPS);
                    if (inTriangle(p, T))
                    {
                        setType(new GamePos(i, j), Obstacle);
                        continue;
                    }
                    p = new Point3D(x + FloatCmp.EPS, y + 1 - FloatCmp.EPS);
                    if (inTriangle(p, T))
                    {
                        setType(new GamePos(i, j), Obstacle);
                        continue;
                    }
                }
            }
        }
		public void generate() //generate a map 
        {
            Random ran = new Random();

            int[,] g = new int[n, m];
            for (int i = 0; i < n; ++i)
                for (int j = 0; j < m; ++j)
                {
                    if (t[i,j] == Empty) g[i,j] = 0;
                    else g[i,j] = 1; //obstacle
                }
            int cnt = 2, k;
            for (int i = 3; i < n; i += 3)
            {
                for (int j = 3; j < m; j += 3)
                {
                    if (t[i,j] != Empty) continue;


                    if (g[i - 3,j] != 0 && g[i,j - 3] != 0)
                    {
                        if (g[i - 3,j] == g[i,j - 3])
                        {
                            k = ran.Next(0, 99);
                            if (k < 60)
                            {
                                k = ran.Next(0, 1);
                                if (k == 0)
                                {
                                    g[i,j] = g[i - 3,j];
                                    buildWallUp(i, j);
                                }
                                else
                                {
                                    g[i,j] = g[i,j - 3];
                                    buildWallLeft(i, j);
                                }
                            }
                        }
                        else
                        {
                            k = ran.Next(0, 119);
                            if (k < 30)
                            {
                                g[i,j] = g[i - 3,j];
                                buildWallUp(i, j);
                            }
                            else if (k < 60)
                            {
                                g[i,j] = g[i,j - 3];
                                buildWallLeft(i, j);
                            }
                            else if (k < 80)
                            {
                                int clr = g[i - 3,j];
                                for (int _i = 0; _i < n; ++_i)
                                {
                                    for (int _j = 0; _j < m; ++_j)
                                    {
                                        if (g[_i,_j] == clr)
                                        {
                                            g[_i,_j] = g[i,j - 3];
                                        }
                                    }
                                }
                                g[i,j] = g[i,j - 3];
                                buildWallUp(i, j);
                                buildWallLeft(i, j);
                            }
                        }
                    }
                    else if (g[i - 3,j] != 0)
                    { //only up
                        k = ran.Next(0, 89);
                        if (k < 30)
                        {
                            g[i,j] = g[i - 3,j];
                            buildWallUp(i, j);
                        }
                        else if (k < 60)
                        {
                            g[i,j] = g[i,j - 3] = cnt++;
                            setType(new GamePos(i, j - 3), Wall);
                            buildWallLeft(i, j);
                        }
                    }
                    else if (g[i,j - 3] != 0)
                    { //only left
                        k = ran.Next(0, 89);
                        if (k < 30)
                        {
                            g[i,j] = g[i - 3,j] = cnt++;
                            setType(new GamePos(i - 3, j), Wall);
                            buildWallUp(i, j);
                        }
                        else if (k < 60)
                        {
                            g[i,j] = g[i,j - 3];
                            buildWallLeft(i, j);
                        }
                    }
                    else
                    {
                        k = ran.Next(0, 119);
                        if (k < 30)
                        {
                            g[i,j] = g[i - 3,j] = cnt++;
                            setType(new GamePos(i - 3, j), Wall);
                            buildWallUp(i, j);
                        }
                        else if (k < 60)
                        {
                            g[i,j] = g[i,j - 3] = cnt++;
                            setType(new GamePos(i, j - 3), Wall);
                            buildWallLeft(i, j);
                        }
                        else if (k < 90)
                        {
                            g[i,j] = g[i - 3,j] = g[i,j - 3] = cnt++;
                            setType(new GamePos(i - 3, j), Wall);
                            setType(new GamePos(i, j - 3), Wall);
                            buildWallUp(i, j);
                            buildWallLeft(i, j);
                        }
                    }

                }
            }

        }
        /*
        public bool detectPlayer(Monster m, int direction)
        {
            if (direction == left)
            {
                for (int i = m.p.y; i >= 0; --i)
                {
                    if (t[m.p.x,i] == Wall) break;
                    else if (t[m.p.x,i] == Player) return true;
                }
            }
            else if (direction == right)
            {
                for (int i = m.p.y; i < m; ++i)
                {
                    if (t[m.p.x,i] == Wall) break;
                    else if (t[m.p.x,i] == Player) return true;
                }
            }
            else if (direction == up)
            {
                for (int i = m.p.x; i >= 0; --i)
                {
                    if (t[i][m.p.y] == Wall) break;
                    else if (t[i][m.p.y] == Player) return true;
                }
            }
            else if (direction == down)
            {
                for (int i = m.p.x; i < n; ++i)
                {
                    if (t[i][m.p.y] == Wall) break;
                    else if (t[i][m.p.y] == Player) return true;
                }
            }
            return false;
        }*/
    };

    public class Monster
	{
		public Circle cir;
        public GamePos p;
		public Monster(Circle _p){
            cir = _p;
            p = new GamePos(_p.c);
        }
        /*
        public void move(Point3D _p)
        {
            cir.c = cir.c + _p;
            p = new GamePos(cir.c);
        }*/
	};

	public class SuccesiveGameMap : GameMap 
	{
		Rectangle [] Recs;
		Circle [] Beans;
        Monster[] Mons;
        Circle Play;
        private const int CirMaxNum = 100;
        public int CirNum;
        public int RecNum;
        public float xLimit;
        public float yLimit;
        public int BeanNum;
		SuccesiveGameMap(int _n = 0, int _m = 0):base(_n, _m)
        {
            Recs = new Rectangle[_n * _m];
            Beans = new Circle[CirNum];
            xLimit = (float)_n - FloatCmp.EPS;
            yLimit = (float)_m - FloatCmp.EPS;
        }
		
	    private void generateBeans(int bnum)
        {
            Random ran = new Random();
            //BeanNum = bnum;
            for (int i = 0; BeanNum < bnum && i < n * m * 10 ; i++)
            {
                float x = (float)ran.NextDouble() * xLimit;
                float y = (float)ran.NextDouble() * yLimit;
                Circle tmp = new Circle(new Point3D(x, y));
                bool flag = true;
                for (int j = 0; j < RecNum; ++j)
                {
                    if (Circle.RCIntersect(Recs[j], tmp))
                    {
                        flag = false;
                    }
                }
                if (!flag) continue;
                for (int j = 0; j < CirNum; ++j)
                {
                    if (Circle.CircleIntersect(Beans[j], tmp))
                    {
                        flag = false;
                    }
                }
                if (flag)
                {
                    Beans[BeanNum++] = tmp;
                }
            }
        }
        void generateMonsters()
        {
            Random ran = new Random();
            for (int i = 0; i < Monster_Num;)
            {
                float x = (float)ran.NextDouble() * xLimit;
                float y = (float)ran.NextDouble() * yLimit;
                Monster tmp = new Monster(new Circle(new Point3D(x, y)));
                bool flag = true;
                for (int j = 0; j < RecNum; ++j)
                {
                    if (Circle.RCIntersect(Recs[j], tmp.cir))
                    {
                        flag = false;
                    }
                }
                if (!flag) continue;
                if (flag) Mons[i++] = tmp; // !!!!
            }
        }
		
	    public void setPlayer(Point3D p)
        {
            Play = new Circle(p);
        }
        void generateMap()
        {// using base's generate and transform to successive
            generate();
            generateBeans(n * m);
            generateMonsters();
        }
		// need : generate() -> generateBeans() -> generateMonsters() 
		
		//void paintMap(); //generate a graph for small map
       
		/* @myp : fill this ! */
	};
}