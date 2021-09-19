using System;

namespace Q2App
{
    //Feels like a good idea to make this struct immutable
    public readonly struct Vec3 : IEquatable<Vec3>
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

        public override bool Equals(object obj) => obj is Vec3 other && this.Equals(other);

        public bool Equals(Vec3 other) => X == other.X && Y == other.Y && Z == other.Z;

        public override int GetHashCode() => (X, Y, Z).GetHashCode();
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
         * same way you would solve the problem in 2D using x,y only.
         * 
         * I am using this as my basic understanding of how to do this in 2D space
         * https://diego.assencio.com/?index=ec3d5dfdfc0b6a0d147a656f0af332bd#mjx-eqn-post_ec3d5dfdfc0b6a0d147a656f0af332bd_lambda_closest_point_line_to_point
         * 
         * As I understand it we find the line that intersects the start and end point and then find the point on that line for which the provided point is perpendicular as
         * that spot is the closest spot. Then we have to determine if that point it between start/end or outside. If its between then it is the shortest point. If its outside then
         * its start if its less than start or its end if its greater than end. 
         * 
         * I think there is a way to project 3D points into a 2D plane (probably how they do normal mapping) solve the problem and then return back to the 3D space but I am not 
         * 100% sure how to do this. 
         * 
         * I added implementation of various vector operations as a primer for figuring this all out. Implemeted things such as add/subtract, dot product,
         * scalar multiplication, magnitude, etc.
         * 
         * I then found a few different implementations of this problem and implemented and tested them (as best I could using simple planes with one dimention as 0 to simplify the calculations).
         * I used these different implementations to wrap my head around the problem. Some worked and some didnt but I left them all in as examples of my work. I feel fairly comfident
         * I implemented them as described but it is possible there is small issues... it is currently 2am...
         **/


        // Implementation of a solution found here
        // https://forum.unity.com/threads/how-do-i-find-the-closest-point-on-a-line.340058/
        // This one is giving me wildly different answers in many cases so I abandoned it without putting too much more time into it.
        public static Vec3 GetNearestPointOnSegment(Vec3 point, Vec3 start, Vec3 end)
        {
            var segmentVector = Subtract(end, start);
            var segmentMagnitude = Magnitude(segmentVector);
            var normalizedSegmentVector = Normalize(segmentVector);

            var pointVector = Subtract(point, start);
            var pointAndSegmentDotProduct = Dot(pointVector, normalizedSegmentVector);
            var clampedDotProduct = Clamp(pointAndSegmentDotProduct, 0, segmentMagnitude);

            return ScalarMultiplication(Add(start, normalizedSegmentVector), clampedDotProduct);
        }

        //Implementation of a solution found here
        //https://zalo.github.io/blog/closest-point-between-segments/
        //My testing implies this one works well. It is essentially the same as the 4th implementation
        //it just solves the t < 0 and t > 1 solution using clamp instead and does a Lerp to find the final point
        public static Vec3 GetNearestPointOnSegment2(Vec3 point, Vec3 start, Vec3 end)
        {
            var segmentVector = Subtract(end, start);
            var t = Dot(Subtract(point, start), segmentVector) / Dot(segmentVector, segmentVector);
            var clampedT = Clamp(t, 0, 1);

            return Lerp(start, end, clampedT);
        }

        //Implementation of a solution found here
        //https://math.stackexchange.com/questions/1521128/given-a-line-and-a-point-in-3d-how-to-find-the-closest-point-on-the-line
        //My testing implies it failes for anything out of bounds. So if the goal was to find the closest point on the infinit line that crosses
        //between start/end then this would work. I think it could be updated to work however by clamping or checking t. By leaving in unbounded it
        //finds a different answer when the closest is outside the segment.
        //
        //It also does its math a bit differently by defining the segment vector as end - start instead and then using subtraction to determine the final
        //point instead of addition. Minor changes but threw me off at first.
        public static Vec3 GetNearestPointOnSegment3(Vec3 point, Vec3 start, Vec3 end)
        {
            var t = Dot(Subtract(end, start), Subtract(start, point)) / Dot(Subtract(end, start), Subtract(end, start));
            var closestPoint = Subtract(start, ScalarMultiplication(Subtract(end, start), t));
            return closestPoint;
        }

        //Implementatin of a solution found here
        //https://diego.assencio.com/?index=ec3d5dfdfc0b6a0d147a656f0af332bd#mjx-eqn-post_ec3d5dfdfc0b6a0d147a656f0af332bd_lambda_closest_point_line_to_point
        //This was an example for vec2 that I just adapted to work with vec3 by just implementing the vec3 versions of the operations. Appears to work well
        //but the giant if statements look less nice than implementation 2.
        public static Vec3 GetNearestPointOnSegment4(Vec3 point, Vec3 start, Vec3 end)
        {
            var t = Dot(Subtract(point, start), Subtract(end, start)) / Dot(Subtract(end, start), Subtract(end, start));
            
            if(t < 0)
            {
                return start;
            }
            else if(t > 1)
            {
                return end;
            }
            else
            {
                return Add(start, ScalarMultiplication(Subtract(end, start), t));
            }
        }

        public static Vec3 Add(Vec3 first, Vec3 second)
        {
            return new Vec3(first.X + second.X, first.Y + second.Y, first.Z + second.Z);
        }

        public static Vec3 Subtract(Vec3 first, Vec3 second)
        {
            return new Vec3(first.X - second.X, first.Y - second.Y, first.Z - second.Z);
        }

        public static Vec3 ScalarMultiplication(Vec3 input, double scalar)
        {
            return new Vec3(input.X * scalar, input.Y * scalar, input.Z * scalar);
        }

        public static double Dot(Vec3 first, Vec3 second)
        {
            return (first.X * second.X) + (first.Y * second.Y) + (first.Z * second.Z);
        }

        public static Vec3 Normalize(Vec3 input)
        {
            var magnitude = Magnitude(input);
            return new Vec3(input.X / magnitude, input.Y / magnitude, input.Z / magnitude);
        }

        public static double Magnitude(Vec3 input)
        {
            return Math.Sqrt(Math.Pow(input.X, 2) + Math.Pow(input.Y, 2) + Math.Pow(input.Z, 2));
        }

        public static double Clamp(double input, double low, double high)
        {
            if (input < low)
            {
                return low;
            }
            else if( input > high)
            {
                return high;
            }
            else
            {
                return input;
            }
        }

        //T must be between 0 - 1 inclusive
        public static Vec3 Lerp(Vec3 start, Vec3 end, double t)
        {
            if (t < 0 || t > 1)
            {
                throw new ArgumentException("t must be between 0 and 1 inclusive");
            }

            return Add(start, ScalarMultiplication(Subtract(end, start), t));
        }
    }
}
