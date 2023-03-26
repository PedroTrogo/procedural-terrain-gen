using UnityEngine;

public class MathP : MonoBehaviour {


    /// <summary>
    /// Get f and return f positive
    /// </summary>
    /// <param name="f"></param>
    /// <returns></returns>
	public static float Positive(float f)
    {
        if (f < 0)
            f *= -1;

        return f;
    }


    /// <summary>
    /// Get referential value, min of value, max of value, min of f, max of f and return f inversely proportional of value.
    /// </summary>
    /// <param name="refValue"></param>
    /// <param name="refMin"></param>
    /// <param name="refMax"></param>
    /// <param name="fMin"></param>
    /// <param name="fMax"></param>
    /// <returns></returns>
    public static float InverseTowardsProportional (float refValue, float minRef, float maxRef, float minf, float maxf)
    {
        float fAbsValue = maxf - minf;
        float refAbsValue = maxRef - minRef;

        float percentOfRef = (refValue - minRef) / refAbsValue;
        float f = fAbsValue - (fAbsValue * percentOfRef);

        return f;
    }


    /// <summary>
    /// Get referential value, min of value, max of value, min of f, max of f and return f proportional of value.
    /// </summary>
    /// <param name="refValue"></param>
    /// <param name="refMin"></param>
    /// <param name="refMax"></param>
    /// <param name="fMin"></param>
    /// <param name="fMax"></param>
    /// <returns></returns>
    public static float TowardsProportional (float refValue, float minRef, float maxRef, float minf, float maxf)
    {
        float fAbsValue = maxf - minf;
        float refAbsValue = maxRef - minRef;

        refValue = Mathf.Clamp(refValue, minRef, maxRef);

        float percentOfRef = (refValue - minRef) / refAbsValue;
        float f = fAbsValue * percentOfRef + minf;

        return f;
    }



    public static float ReturnPositiveOrZero (float f)
    {
        if (f > 0)
            return f;
        else
            return 0;
    }
}
