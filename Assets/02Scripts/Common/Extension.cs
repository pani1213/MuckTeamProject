using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extension 
{
    public static (int A,int B) GetHalf(this int _num)
    {
        float a = 0;
        float b = 0;

        a = _num / 2; 
        b = _num / 2;

        if (_num % 2 == 1)
            a += 1;
        return ((int)a, (int)b);
    }
    public static bool Between(this float _float, float _min, float _max)
    {
        if(_float >= _min && _float <= _max)
            return true;
        return false;
    }
}