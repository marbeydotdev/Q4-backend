namespace Common;

public static class Utils
{
    public static double GetMedian(double[] arrSource)
    {
        // Check if the array has values        
        if (arrSource.Length == 0)
        {
            return 0;
        }

        // Sort the array
        var arrSorted = (double[])arrSource.Clone();
        Array.Sort(arrSorted);

        // Calculate the median
        var size = arrSorted.Length;
        var mid = size / 2;

        if (size % 2 != 0)
        {
            return arrSorted[mid];
        }

        dynamic value1 = arrSorted[mid];
        dynamic value2 = arrSorted[mid - 1];
        return (value1 + value2) / 2;
    }
}