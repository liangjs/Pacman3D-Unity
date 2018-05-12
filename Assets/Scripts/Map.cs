using System;
namespace Pacman3D
{
    public const int Empty = 0;
    public const int Wall = 1;
    public const int Player = 2;
    public const int Bean = 3;
    public const int Monster = 4;
    public const int Obstacle = 5;
    public const float rate = 1.0; // Game / World
    public const float xBias = 0.0;
    public const float yBias = 0.0;
    public const int Monster_Num = 5;
	
	class GamePos
	{
		public int x;
		public int y;
		public GamePos(int _x = 0, int _y = 0) {
            x = _x;
            y = _y;
        }
        public GamePos(ref Point3D p)
        {
            x = (int)p.x;
            y = (int)p.y;
        }
	}
	

	class GameMap
	{
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
                    t[i][j] = Empty;
                }
            }
        }

        void drawStroke(Line l)
        {
        }

        void bfs(int x, int y)
        {
            GamePos[] q = new GamePos[n * m];
            int l = 0, r = -1;
            q[++r] = new GamePos(x, y);
            while (l <= r)
            {
                GamePos p = q[l++];
                setType(p, Obstacle);
                if (p.y + 1 < m && t[p.x, p.y + 1] == Empty)
                {
                    q[++r] = new GamePos(p.x, p.y + 1);
                } else if (p.y - 1 >= 0 && t[p.x, p.y -1] == Empty)
                {
                    q[++r] = new GamePos(p.x, p.y - 1);
                } else if (p.x + 1 < n && t[p.x + 1, p.y] == Empty)
                {
                    q[++r] = new GamePos(p.x + 1, p.y);
                } else if (p.x - 1 >= 0 && t[p.x - 1, p.y] == Empty)
                {
                    q[++r] = new GamePos(p.x - 1, p.y);
                }
            }
        }

        public addBorder(Line[] lines, int lineNum)
        {
            for (int i = 0; i < lineNum; ++i)
            {
                drawStroke(lines[i]);
            }
            bfs(0, 0);
            bfs(n - 1, 0);
            bfs(0, m - 1);
            bfs(n - 1, m - 1);
        }

		
		public GamePos worldToGame(ref Point3D worldPos)
        {
            float x = worldPos.x, y = worldPos.y;
            x = x * rate + x_bias;
            y = y * rate + y_bias;
            return GamePos((int) x , (int) y);
        }
		
		public Point3D gameToWorld(ref GamePos gamePos, float height)
        {
            float x = worldPos.x, y = worldPos.y;
            x = (x - x_bias) / rate;
            y = (y - y_bias) / rate;
            return Point3D(x, y, height);
        }
		
		private void buildWallUp(int x, int y)
        {
            setType(GamePos(x, y), Wall);
            setType(GamePos(x - 1, y), Wall);
            setType(GamePos(x - 2, y), Wall);

        }
        private void buildWallLeft(int x, int y)
        {
            setType(GamePos(x, y), Wall);
            setType(GamePos(x, y - 1), Wall);
            setType(GamePos(x, y - 2), Wall);
        }
        private Point3D pointShadow(ref Point3D worldPos){
            float x = worldPos.x, y = worldPos.y;
            x = x* rate + x_bias;
		    y = y* rate + y_bias;
		    return Point3D(x, y, 0.0);
        }
        private float cross(ref Point3D a, ref Point3D b){
	    	return a.x* b.y - a.y* b.x;
        }

        private bool inTriangle(ref Point3D p, ref Triangle t){
            Point3D p0 = pointShadow(t.points[0]), p1 = pointShadow(t.points[1]), p2 = pointShadow(t.points[2]);
            float Sabc = fabs(cross(p1 - p0, p2 - p0));
            float Spab = fabs(cross(p0 - p, p1 - p));
            float Spac = fabs(cross(p0 - p, p2 - p));
            float Spbc = fabs(cross(p1 - p, p2 - p));
            return fabs(Sabc - Spab - Spac - Spbc) < EPS;
        }
		
		private void setType(ref GamePos p, int _t){
            t[p.x][p.y] = _t;
	    }
        private void addTriangle(ref Triangle T)
        {
            for (int i = 0; i < n; ++i)
            {
                for (int j = 0; j < m; ++j)
                {
                    if (t[i][j] == Obstacle) continue;

                    float x = (float)i, y = (float)j;
                    Point3D p = new Point3D(x + EPS, y + EPS);
                    if (inTriangle(p, T))
                    {
                        setType(GamePos(i, j), Obstacle);
                        continue;
                    }
                    p = new Point3D(x + 1 - EPS, y + 1 - EPS);
                    if (inTriangle(p, T))
                    {
                        setType(GamePos(i, j), Obstacle);
                        continue;
                    }
                    p = new Point3D(x + 1 - EPS, y + EPS);
                    if (inTriangle(p, T))
                    {
                        setType(GamePos(i, j), Obstacle);
                        continue;
                    }
                    p = new Point3D(x + EPS, y + 1 - EPS);
                    if (inTriangle(p, T))
                    {
                        setType(GamePos(i, j), Obstacle);
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
                    if (t[i][j] == Empty) g[i][j] = 0;
                    else g[i][j] = 1; //obstacle
                }
            int cnt = 2, k;
            for (int i = 3; i < n; i += 3)
            {
                for (int j = 3; j < m; j += 3)
                {
                    if (t[i][j] != Empty) continue;


                    if (g[i - 3][j] != 0 && g[i][j - 3] != 0)
                    {
                        if (g[i - 3][j] == g[i][j - 3])
                        {
                            k = ran.Next(0, 99);
                            if (k < 60)
                            {
                                k = ran.Next(0, 1);
                                if (k == 0)
                                {
                                    g[i][j] = g[i - 3][j];
                                    buildWallUp(i, j);
                                }
                                else
                                {
                                    g[i][j] = g[i][j - 3];
                                    buildWallLeft(i, j);
                                }
                            }
                        }
                        else
                        {
                            k = ran.Next(0, 119);
                            if (k < 30)
                            {
                                g[i][j] = g[i - 3][j];
                                buildWallUp(i, j);
                            }
                            else if (k < 60)
                            {
                                g[i][j] = g[i][j - 3];
                                buildWallLeft(i, j);
                            }
                            else if (k < 80)
                            {
                                int clr = g[i - 3][j];
                                for (int _i = 0; _i < n; ++_i)
                                {
                                    for (int _j = 0; _j < m; ++_j)
                                    {
                                        if (g[_i][_j] == clr)
                                        {
                                            g[_i][_j] = g[i][j - 3];
                                        }
                                    }
                                }
                                g[i][j] = g[i][j - 3];
                                buildWallUp(i, j);
                                buildWallLeft(i, j);
                            }
                        }
                    }
                    else if (g[i - 3][j] != 0)
                    { //only up
                        k = ran.Next(0, 89);
                        if (k < 30)
                        {
                            g[i][j] = g[i - 3][j];
                            buildWallUp(i, j);
                        }
                        else if (k < 60)
                        {
                            g[i][j] = g[i][j - 3] = cnt++;
                            setType(GamePos(i, j - 3), Wall);
                            buildWallLeft(i, j);
                        }
                    }
                    else if (g[i][j - 3] != 0)
                    { //only left
                        k = ran.Next(0, 89);
                        if (k < 30)
                        {
                            g[i][j] = g[i - 3][j] = cnt++;
                            setType(GamePos(i - 3, j), Wall);
                            buildWallUp(i, j);
                        }
                        else if (k < 60)
                        {
                            g[i][j] = g[i][j - 3];
                            buildWallLeft(i, j);
                        }
                    }
                    else
                    {
                        k = ran.Next(0, 119);
                        if (k < 30)
                        {
                            g[i][j] = g[i - 3][j] = cnt++;
                            setType(GamePos(i - 3, j), Wall);
                            buildWallUp(i, j);
                        }
                        else if (k < 60)
                        {
                            g[i][j] = g[i][j - 3] = cnt++;
                            setType(GamePos(i, j - 3), Wall);
                            buildWallLeft(i, j);
                        }
                        else if (k < 90)
                        {
                            g[i][j] = g[i - 3][j] = g[i][j - 3] = cnt++;
                            setType(GamePos(i - 3, j), Wall);
                            setType(GamePos(i, j - 3), Wall);
                            buildWallUp(i, j);
                            buildWallLeft(i, j);
                        }
                    }

                }
            }

        }
        public bool detectPlayer(ref Monster m, int direction)
        {
            if (direction == left)
            {
                for (int i = m.p.y; i >= 0; --i)
                {
                    if (t[m.p.x][i] == Wall) break;
                    else if (t[m.p.x][i] == Player) return true;
                }
            }
            else if (direction == right)
            {
                for (int i = m.p.y; i < m; ++i)
                {
                    if (t[m.p.x][i] == Wall) break;
                    else if (t[m.p.x][i] == Player) return true;
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
        }
    };
	
	class Monster
	{
		public Circle cir;
        public GamePos p;
		Monster(ref Circle _p){
            cir = _p;
            p = new GamePos(_p.c);
        }

        public void move(ref Point3D _p)
        {
            cir.c = cir.c + _p;
            p = GamePos(pos);
        }
	};

	public class SuccesiveGameMap:GameMap 
	{
		Rectangle [] Recs;
		Circle [] Beans;
        Monster[] Mons;
        Circle Play;
        private const int CirMaxNum = 100;
        public int RecNum;
        public float xLimit;
        public float yLimit;
        public int BeanNum;
		SuccesiveGameMap(int _n = 0, int _m = 0):base(_n, _m)
        {
            Recs = new Rectangle[_n * _m];
            Beans = new Circle[CirNum];
            xLimit = (float)_n - EPS;
            yLimit = (float)_m - EPS;
        }
		
	    private void generateBeans(int bnum)
        {
            Random ran = new Random();
            BeanNum = bnum;
            for (int i = 0; i < BeanNum; )
            {
                float x = ran.NextDouble() * xLimit;
                float y = ran.NextDouble() * yLimit;
                Circle tmp = new Circle(Point3D(x, y));
                bool flag = true;
                for (int j = 0; j < RecNum; ++j)
                {
                    if (Recs[j].isCrash(tmp))
                    {
                        flag = false;
                    }
                }
                if (!flag) continue;
                for (int j = 0; j < BeanNum; ++j)
                {
                    if (Beans[j].isCrash(tmp))
                    {
                        flag = false;
                    }
                }
                if (flag) Beans[i++] = tmp;
            }
        }
        void generateMonsters()
        {
            Random ran = new Random();
            for (int i = 0; i < 3;)
            {
                float x = ran.NextDouble() * xLimit;
                float y = ran.NextDouble() * yLimit;
                Monster tmp = new Monster(new Circle(new Point3D(x, y)));
                bool flag = true;
                for (int j = 0; j < RecNum; ++j)
                {
                    if (Recs[j].isCrash(tmp))
                    {
                        flag = false;
                    }
                }
                if (!flag) continue;
                if (flag) Mons[i++] = tmp;
            }
        }
		
	    public void setPlayer(Point3D p)
        {
            Play = new Circle(p);
        }
        void generateMap()
        {// using base's generate and transform to successive
            generate();
            generateBeans();
            generateMonsters();
        }
		// need : generate() -> generateBeans() -> generateMonsters() 
		
		void paintMap(); //generate a graph for small map
		/* @myp : fill this ! */
	};
}