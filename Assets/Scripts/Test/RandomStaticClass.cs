using UnityEngine;

public static class RandomStaticClass
{
    private static int rndNum = 0;

    static int rndNum2 = 0;
    static void RndNumber()
    {

    }

    static int RndIntNum()
    {
        return rndNum++;
    }
}
