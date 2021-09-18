namespace Q1App
{
    public static class Q1
    {
        //I assume that arr must be a sorted list of integers (ascending) or this would likely do weird things. Given we are making assumptions based on the
        //size of arr[middle] in comparison to our target and moving up or down. We effectively cut out half the range in either direction based on
        //the middle index check (binary search) so if they were not sorted you would not find the right answer all the time.
        //
        //I renamed variables to make it easier for me to read.
        //
        //It also looks to me like its a bianry search index of function that only works when you pass it a ascending ordered list of integers.
        //Might be able to save some time if we compared the largest/smallest numbers to the target given if they are out of the 'bounds' of the list 
        //they cant exist.
        //
        //Useful anytime you need a quick index lookup for an array of sorted integers or the existance of a number in a list/set/etc
        public static int Func(int[] arr, int target, int start, int end)
        {
            int middle = (start + end) / 2;
            
            //If our start index is greater than the end index we have searched the range and found nothing.
            //So return -1 to indicate there is no index with an equivalent target
            if (start > end)
            {
                return -1;
            }
            //If the value at the index is less than target we move forward the startindex and try again with the larger set of numbers
            if (arr[middle] < target)
            {
                return Func(arr, target, middle + 1, end);
            }
            //If the value at the index is less greater target we move back the end index and try again with the larger set of numbers
            if (arr[middle] > target)
            {
                return Func(arr, target, start, middle - 1);
            }
            //We found an exact match so return its index
            return middle;
        }
    }
}
