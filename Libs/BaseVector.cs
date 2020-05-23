﻿public abstract class BaseVector
{
    public abstract BaseVector Multiply(float multiplier);

    public static BaseVector operator *(BaseVector v, float multiplier)
    {
        return v.Multiply(multiplier);
    }
}
