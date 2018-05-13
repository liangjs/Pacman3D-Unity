using System;

namespace Pacman3D
{
    public class GamePos
    {
        public int x;
        public int y;
        public GamePos(int _x = 0, int _y = 0)
        {
            x = _x;
            y = _y;
        }
        public GamePos(Point3D p)
        {
            x = (int)p.x;
            y = (int)p.y;
        }
    }

    public class transform_coord
    {
        public const float rate = 0.2f;
        public float maxx, maxz, minx, minz;
        public float meany;
        public GamePos tranformxyz(int num, Point3D[] p)
        {
            maxx = Max_x(num, p);
            minx = Min_x(num, p);
            maxz = Max_z(num, p);
            minz = Min_z(num, p);
            meany = Mean(num, p);

            // determine m, n   m:col_num  n:row_num  
            int m = (int)((maxx - minx) / rate + 1);
            int n = (int)((maxz - minz) / rate + 1);

            return new GamePos(m, n);
        }

        public Point3D WorldToGame(Point3D p)
        {
            float x = ((p.x - minx) / rate);
            float y = ((p.z - minz) / rate);
            return new Point3D(x, 0, y);
        }

        public Point3D GameToWolrd(Point3D p)
        {
            float x = p.x * rate + minx;
            float z = p.z * rate + minz;
            return new Point3D(x, meany, z);
        }

        public Point3D GameToWolrd(GamePos p)
        {
            return GameToWolrd(new Point3D(p.x, 0, p.y));
        }

        private float Mean(int num, Point3D[] p) // num is number of input dots(x,y,z)
        {
            float res = 0;
            for (int i = 0; i < num; i++)
            {
                res += p[i].coord[1];
            }
            return res;
        }
        private float Max_x(int num, Point3D[] p) // num is number of input dots(x,y,z)
        {
            float res = -FloatCmp.INF;
            for (int i = 0; i < num; i++)
            {
                res = Math.Max(res, p[i].coord[0]);
            }
            return res;
        }
        private float Max_z(int num, Point3D[] p) // num is number of input dots(x,y,z)
        {
            float res = -FloatCmp.INF;
            for (int i = 0; i < num; i++)
            {
                res = Math.Max(res, p[i].coord[2]);
            }
            return res;
        }
        private float Min_x(int num, Point3D[] p) // num is number of input dots(x,y,z)
        {
            float res = FloatCmp.INF;
            for (int i = 0; i < num; i++)
            {
                res = Math.Min(res, p[i].coord[0]);
            }
            return res;
        }
        private float Min_z(int num, Point3D[] p) // num is number of input dots(x,y,z)
        {
            float res = FloatCmp.INF;
            for (int i = 0; i < num; i++)
            {
                res = Math.Min(res, p[i].coord[2]);
            }
            return res;
        }
    };

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
        public const int Passage_Width = 6;
        public int n, m;
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
                    t[i, j] = Empty;
                }
            }
        }

        private void drawStroke(Line l)
        {
            float x1, x2, y1, y2;
            if (l.st.x < l.ed.x)
            {
                x1 = l.st.x;
                x2 = l.ed.x;
                y1 = l.st.y;
                y2 = l.ed.y;
            }
            else
            {
                x2 = l.st.x;
                x1 = l.ed.x;
                y2 = l.st.y;
                y1 = l.ed.y;
            }
            float dx = x2 - x1;
            float dy = y2 - y1;
            if (dx > FloatCmp.EPS)
                for (int x = (int)x1; x <= (int)x2; ++x)
                {
                    float y = y1 + dy * (x - x1) / dx;
                    setType(new GamePos(x, (int)y), Obstacle);
                }
            else
                for (int y = (int)Math.Min(y1,y2); y <= (int)Math.Max(y1, y2); ++y)
                    setType(new GamePos((int)x1, (int)y), Obstacle);
        }
        
        void bfs(int x, int y)
        {
            if (t[x, y] != Empty)
            {
                return;
            }
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
                }
                else if (p.y - 1 >= 0 && t[p.x, p.y - 1] == Empty)
                {
                    q[++r] = new GamePos(p.x, p.y - 1);
                }
                else if (p.x + 1 < n && t[p.x + 1, p.y] == Empty)
                {
                    q[++r] = new GamePos(p.x + 1, p.y);
                }
                else if (p.x - 1 >= 0 && t[p.x - 1, p.y] == Empty)
                {
                    q[++r] = new GamePos(p.x - 1, p.y);
                }
            }
        }

        public void addBorder(Line[] lines)
        {
            for (int i = 0; i < lines.Length; ++i)
            {
                drawStroke(lines[i]);
            }
            for (int i = 0; i < n; ++i)
            {
                bfs(i, 0);
                bfs(i, m - 1);
            }
            for (int i = 0; i < m; ++i)
            {
                bfs(0, i);
                bfs(n - 1, i);
            }
        }

        /*
        public GamePos worldToGame(Point3D worldPos)
        {
            float x = worldPos.x, y = worldPos.y;
            x = x * rate + x_bias;
            y = y * rate + y_bias;
            return new GamePos((int)x, (int)y);
        }

        public Point3D gameToWorld(GamePos gamePos, float height)
        {
            float x = gamePos.x, y = gamePos.y;
            x = (x - x_bias) / rate;
            y = (y - y_bias) / rate;
            return new Point3D(x, y, height);
        }*/

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
        private Point3D pointShadow(Point3D worldPos)
        {
            float x = worldPos.x, y = worldPos.y;
            x = x * rate + x_bias;
            y = y * rate + y_bias;
            return new Point3D(x, y, 0.0f);
        }
        private float cross(Point3D a, Point3D b)
        {
            return a.x * b.y - a.y * b.x;
        }

        private bool inTriangle(Point3D p, Triangle t)
        {
            Point3D p0 = pointShadow(t.points[0]), p1 = pointShadow(t.points[1]), p2 = pointShadow(t.points[2]);
            float Sabc = System.Math.Abs(cross(p1 - p0, p2 - p0));
            float Spab = System.Math.Abs(cross(p0 - p, p1 - p));
            float Spac = System.Math.Abs(cross(p0 - p, p2 - p));
            float Spbc = System.Math.Abs(cross(p1 - p, p2 - p));
            return System.Math.Abs(Sabc - Spab - Spac - Spbc) < FloatCmp.EPS;
        }

        int Min(int x, int y, int z)
        {
            if (x <= y && x <= z)
            {
                return x;
            }
            else if (y <= x && y <= z)
            {
                return y;
            }
            else return z;
        }
        int Max(int x, int y, int z)
        {
            if (x >= y && x >= z)
            {
                return x;
            }
            else if (y >= x && y >= z)
            {
                return y;
            }
            else return z;
        }
        public void addTriangle(Triangle T)
        {
            int xMin = Min(new GamePos(T.points[0]).x, new GamePos(T.points[1]).x, new GamePos(T.points[2]).x);
            int yMin = Min(new GamePos(T.points[0]).y, new GamePos(T.points[1]).y, new GamePos(T.points[2]).y);
            int xMax = Max(new GamePos(T.points[0]).x, new GamePos(T.points[1]).x, new GamePos(T.points[2]).x);
            int yMax = Max(new GamePos(T.points[0]).y, new GamePos(T.points[1]).y, new GamePos(T.points[2]).y);

            for (int i = Math.Max(0, xMin - 2); i < Math.Min(n - 1, xMax + 2); ++i)
            {
                for (int j = Math.Max(0, yMin - 2); j < Math.Min(m - 1, yMax + 2); ++j)
                {
                    if (t[i, j] == Obstacle) continue;

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
                    if (t[i, j] == Empty) g[i, j] = 0;
                    else g[i, j] = 1; //obstacle
                }
            int cnt = 2, k;
            for (int i = Passage_Width; i < n; i += Passage_Width)
            {
                for (int j = Passage_Width; j < m; j += Passage_Width)
                {
                    if (t[i, j] != Empty) continue;


                    if (g[i - Passage_Width, j] != 0 && g[i, j - Passage_Width] != 0)
                    {
                        if (g[i - Passage_Width, j] == g[i, j - Passage_Width])
                        {
                            k = ran.Next(0, 99);
                            if (k < 60)
                            {
                                k = ran.Next(0, 1);
                                if (k == 0)
                                {
                                    g[i, j] = g[i - Passage_Width, j];
                                    buildWallUp(i, j);
                                }
                                else
                                {
                                    g[i, j] = g[i, j - Passage_Width];
                                    buildWallLeft(i, j);
                                }
                            }
                        }
                        else
                        {
                            k = ran.Next(0, 119);
                            if (k < 30)
                            {
                                g[i, j] = g[i - Passage_Width, j];
                                buildWallUp(i, j);
                            }
                            else if (k < 60)
                            {
                                g[i, j] = g[i, j - Passage_Width];
                                buildWallLeft(i, j);
                            }
                            else if (k < 80)
                            {
                                int clr = g[i - Passage_Width, j];
                                for (int _i = 0; _i < n; ++_i)
                                {
                                    for (int _j = 0; _j < m; ++_j)
                                    {
                                        if (g[_i, _j] == clr)
                                        {
                                            g[_i, _j] = g[i, j - Passage_Width];
                                        }
                                    }
                                }
                                g[i, j] = g[i, j - 3];
                                buildWallUp(i, j);
                                buildWallLeft(i, j);
                            }
                        }
                    }
                    else if (g[i - Passage_Width, j] != 0)
                    { //only up
                        k = ran.Next(0, 89);
                        if (k < 30)
                        {
                            g[i, j] = g[i - Passage_Width, j];
                            buildWallUp(i, j);
                        }
                        else if (k < 60)
                        {
                            g[i, j] = g[i, j - Passage_Width] = cnt++;
                            setType(new GamePos(i, j - Passage_Width), Wall);
                            buildWallLeft(i, j);
                        }
                    }
                    else if (g[i, j - Passage_Width] != 0)
                    { //only left
                        k = ran.Next(0, 89);
                        if (k < 30)
                        {
                            g[i, j] = g[i - Passage_Width, j] = cnt++;
                            setType(new GamePos(i - Passage_Width, j), Wall);
                            buildWallUp(i, j);
                        }
                        else if (k < 60)
                        {
                            g[i, j] = g[i, j - Passage_Width];
                            buildWallLeft(i, j);
                        }
                    }
                    else
                    {
                        k = ran.Next(0, 119);
                        if (k < 30)
                        {
                            g[i, j] = g[i - Passage_Width, j] = cnt++;
                            setType(new GamePos(i - Passage_Width, j), Wall);
                            buildWallUp(i, j);
                        }
                        else if (k < 60)
                        {
                            g[i, j] = g[i, j - Passage_Width] = cnt++;
                            setType(new GamePos(i, j - Passage_Width), Wall);
                            buildWallLeft(i, j);
                        }
                        else if (k < 90)
                        {
                            g[i, j] = g[i - Passage_Width, j] = g[i, j - Passage_Width] = cnt++;
                            setType(new GamePos(i - Passage_Width, j), Wall);
                            setType(new GamePos(i, j - Passage_Width), Wall);
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
        public Monster(Circle _p)
        {
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
        Rectangle[] Recs;
        public Circle[] Beans;
        public Monster[] Mons;
        Circle Play;
        public int RecNum;
        public float xLimit;
        public float yLimit;
        public int BeanNum;

        public const float Bean_radius = 1;
        public const float Monster_radius = 1;
        public const float Player_radius = 0.5f;

        public SuccesiveGameMap(int _n = 0, int _m = 0):base(_n, _m)
        {
            Recs = new Rectangle[_n * _m];
            Beans = new Circle[n * m];
            xLimit = (float)_n - FloatCmp.EPS;
            yLimit = (float)_m - FloatCmp.EPS;
        }

        private void generateBeans(int bnum)
        {
            Random ran = new Random();
            //BeanNum = bnum;
            for (int i = 0; BeanNum < bnum && i < n * m * 10; i++)
            {
                float x = (float)ran.NextDouble() * xLimit;
                float y = (float)ran.NextDouble() * yLimit;
                Circle tmp = new Circle(new Point3D(x, y), Bean_radius);
                bool flag = true;
                for (int j = 0; j < RecNum; ++j)
                {
                    if (Circle.RCIntersect(Recs[j], tmp))
                    {
                        flag = false;
                    }
                }
                if (!flag) continue;
                for (int j = 0; j < BeanNum; ++j)
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
            Mons = new Monster[Monster_Num];
            for (int i = 0; i < Monster_Num;)
            {
                float x = (float)ran.NextDouble() * xLimit;
                float y = (float)ran.NextDouble() * yLimit;
                Monster tmp = new Monster(new Circle(new Point3D(x, y), Monster_radius));
                bool flag = true;
                for (int j = 0; j < RecNum; ++j)
                {
                    if (Circle.RCIntersect(Recs[j], tmp.cir))
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
            Play = new Circle(p, Player_radius);
        }
        public void generateMap()
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
