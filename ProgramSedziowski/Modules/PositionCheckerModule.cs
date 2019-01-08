using ProgramSedziowski.Exceptions;
using ProgramSedziowski.Model;

namespace ProgramSedziowski.Modules
{
    public static class PositionCheckerModule
    {
        public static void CheckPosition(Point[] points, int size, int[,] board, int numCurrentGamer, Game currentGame)
        {
            bool isOk = true;
            foreach (var point in points)
            {
                isOk &= (point.x >= 0 && point.x < size && point.y >= 0 && point.y < size);
            }

            if (isOk)
            {
                if (points[0].y == points[1].y)
                {
                    isOk &= (points[0].x + 1 == points[1].x || points[0].x - 1 == points[1].x || (points[0].x == 0 && points[1].x == size - 1) || (points[1].x == 0 && points[0].x == size - 1));
                }

                if (points[0].x == points[1].x)
                {
                    isOk &= (points[0].y + 1 == points[1].y || points[0].y - 1 == points[1].y || (points[0].y == 0 && points[1].y == size - 1) || (points[1].y == 0 && points[0].y == size - 1));
                }

                if (isOk)
                {
                    if (board[points[0].x, points[0].y] == 0 && board[points[1].x, points[1].y] == 0)
                    {
                        board[points[0].x, points[0].y] = numCurrentGamer + 1;
                        board[points[1].x, points[1].y] = numCurrentGamer + 1;
                        currentGame.moves.Add(new RegisteredMove(points[0], numCurrentGamer + 1));
                        currentGame.moves.Add(new RegisteredMove(points[1], numCurrentGamer + 1));
                    }
                    else
                    {
                        throw new InvalidMoveException($"Wskazane punkty({{{points[0].x};{points[0].y}}},{{{points[1].x};{points[1].y}}}) są już zajęte");
                    }
                }
                else
                {
                    throw new InvalidMoveException(points);
                }
            }
            else
            {
                throw new PointOutBoardException(points, size);
            }
        }

        public static bool IsAllBoardFull(int[,] board, int size)
        {
            for(int x = 0; x < size; x++)
            {
                for(int y = 0; y < size; y++)
                {
                    if(board[x,y] == 0)
                    {
                        if(x==0 && board[size - 1, y] == 0)
                        {
                            return false;
                        }else if (y == 0 && board[x, size - 1] == 0)
                        {
                            return false;
                        }
                        else if (x+1<size && board[x+1, y] == 0)
                        {
                            return false;
                        }
                        else if (x - 1 >=0 && board[x - 1, y] == 0)
                        {
                            return false;
                        }
                        else if (y + 1 < size && board[x, y+1] == 0)
                        {
                            return false;
                        }
                        else if (y - 1 >= 0 && board[x, y - 1] == 0)
                        {
                            return false;
                        }else if (x == size-1 && board[0, y] == 0)
                        {
                            return false;
                        }
                        else if (y == size-1 && board[x, 0] == 0)
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }
    }
}
