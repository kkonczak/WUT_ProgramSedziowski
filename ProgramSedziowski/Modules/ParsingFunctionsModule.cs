using ProgramSedziowski.Exceptions;
using ProgramSedziowski.Model;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ProgramSedziowski.Modules
{
    public static class ParsingFunctionsModule
    {
        public static string PointsToString(IEnumerable<Model.Point> points)
        {
            return string.Join(",", points.Select(p => $"{{{p.x};{p.y}}}"));
        }

        public static string PointToString(Model.Point point)
        {
            return $"{{{point.x};{point.y}}}";
        }

        public static Point[] AnswerStringToPoint(string ans)
        {
            if (ans == null || ans == "")
            {
                throw new EmptyAnswerException();
            }

            var res = Regex.Match(ans, @"^(\{([0-9]+)\;([0-9]+)\},\{([0-9]+)\;([0-9]+)\})$", RegexOptions.None);
            if (res.Success)
            {
                return new Model.Point[]
                {
                    new Model.Point(int.Parse(res.Groups[2].Value),int.Parse(res.Groups[3].Value)),
                    new Model.Point(int.Parse(res.Groups[4].Value),int.Parse(res.Groups[5].Value))
                };
            }
            else
            {
                throw new InvalidAnswerException(ans, "{x1;y1},{x2;y2}");
            }
        }
    }
}
