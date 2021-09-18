namespace Q2App
{
    //Feels like a good idea to make this struct immutable
    public readonly struct Vec3
    {
        public double X { get;  }
        public double Y { get; }
        public double Z { get; }
        public Vec3(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }

    public static class Q2
    {
        /**
         * I am assuming that here we mean the closet point on the specific line that is created and is contained within the distance betwee
         * two vectors and not an infinite line that runs through two given vectors. This means that the closest point might be the start/end points.
         * 
         * I have not done this sort of thing for a long while so making my best judgement on how this can be done and maybe making some missleading assumptions. We will see.
         * I know that two points in 3D space just create an edge and that adding a 3rd point means we can create a face. its direction in 3d space will be dependant
         * on the position of all 3 vertices but the resulting plane will essentially be a 2d plane, at least in relation to itself. So I hope I can mostly solve this in the
         * same way you would solve the problem in 2D using x,y only. I am just going to assume this will work with the z direction so long as I do my vert math properly. 
         * 
         * I am using this as my basic understanding of how to do this in 2D space
         * https://diego.assencio.com/?index=ec3d5dfdfc0b6a0d147a656f0af332bd#mjx-eqn-post_ec3d5dfdfc0b6a0d147a656f0af332bd_lambda_closest_point_line_to_point
         * 
         * As I understand it we find the line that intersects the start and end point and then find the point on that line for which the provided point is perpendicular as
         * that spot is the closest spot. Then we have to determine if that point it between start/end or outside. If its between then it is the shortest point. If its outside then
         * its start if its less than start or its end if its greater than end. 
         * 
         * I think there is a way to project 3D points into a 2D plane (probably how they do normal mapping) but I am not going there unless I have to. 
         **/
        public static Vec3 GetNearestPointOnSegment(Vec3 point, Vec3 start, Vec3 end)
        {
            //N is considered to be the point on the line that is perpendicular to point. λN is then the distance from start to N where a value between 0 and 1 means its between 
            //end

            var lambdaN = GetLambdaN(point, start, end);

            if (lambdaN <= 0 )
            {
                return start;
            }
            else if (lambdaN >= 1)
            {
                return end;
            }
            else
            {
                return GetN(start, end, lambdaN);
            }
        }

        private static double GetLambdaN(Vec3 point, Vec3 start, Vec3 end)
        {

        }

        private static Vec3 GetN(Vec3 start, Vec3 end, double lambdaN)
        {

        }

        private static Vec3 VecSubtract(Vec3 first, Vec3 second)
        {
            return new Vec3(first.X - second.X, first.Y - second.Y, first.Z - second.Z);
        }
    }
}
