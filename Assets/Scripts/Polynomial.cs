using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Polynomial
{
    public List<double> Coefficients;
    public Polynomial(List<double> coefficients)
    {
        if (coefficients.Count == 0)
            throw new ArgumentException();
        if (coefficients.Count(z => z == 0) == coefficients.Count)
            Coefficients = new List<double>() {0};
        else
        {
            var index = 0;
            while (coefficients[index] == 0)
                index++;
            Coefficients = coefficients.Skip(index).ToList();
        }
    }

    public Polynomial(params double[] coefficients)
    {
        if (coefficients.Length == 0)
            throw new ArgumentException();
        if (coefficients.Count(z => z == 0) == coefficients.Length)
            Coefficients = new List<double>() {0};
        else
        {
            var index = 0;
            while (coefficients[index] == 0)
                index++;
            Coefficients = coefficients.Skip(index).ToList();
        }
    }

    public override string ToString()
    {
        if (IsZero())
            return "0";
        var n = Degree()+1;
        var list = new List<string>();
        for (var i = 0; i < n; i++)
        {
            var power = n - 1 - i;
            var coefficient = Coefficients[i];
            if (coefficient == 0)
                continue;
            var monomyal = power == 0
                ? coefficient.ToString()
                : $"({coefficient})*x^{power}";
            list.Add(monomyal);
        }

        return string.Join(" + ", list);
    }

    public bool IsZero() => Coefficients.Count == 1 && Coefficients[0] == 0;
    public int Degree() => IsZero()? -1 : Coefficients.Count - 1;

    public double Evaluate(double x)
    {
        var d = Degree();
        if (d < 0)
            return 0;
        var res = 0.0;
        for (var i = 0; i < d + 1; i++)
            res += Coefficients[i] * Math.Pow(x, d - i);
        return res;
    }

    public static Polynomial operator +(Polynomial self, Polynomial other)
    {
        var m = self.Degree()+1;
        var n = other.Degree()+1;
        
        var max = Math.Max(m, n);

        var mc = self.Coefficients.ToArray().ToList();
        var nc = other.Coefficients.ToArray().ToList();
        for(var i = 0; i<max-m; i++)
            mc.Insert(0, 0);
        for(var i = 0; i<max-n; i++)
            nc.Insert(0, 0);

        return new Polynomial(Enumerable.Range(0, max).Select(i => mc[i] + nc[i]).ToList());
    }
    public static Polynomial operator -(Polynomial self) => new Polynomial(self.Coefficients.Select(z=>-z).ToList());
    public static Polynomial operator -(Polynomial self, Polynomial other) => self + -other;

    public static Polynomial operator *(Polynomial self, double other) =>
        new Polynomial(self.Coefficients.Select(z => z * other).ToList());

    public static Polynomial operator *(Polynomial self, Polynomial other)
    {
        var d1 = self.Degree();
        var d2 = other.Degree();
        if (d1 < 0 || d2 < 0)
            return new Polynomial(0);
        var multCoeffs = Enumerable.Range(0, d1 + d2 + 1).Select(z => 0.0).ToList();
        for(var i = 0; i<d1+1; i++)
        for (var j = 0; j < d2 + 1; j++)
        {
            var power = d1 + d2 - i - j;
            var coefficient = self.Coefficients[i] * other.Coefficients[j];
            multCoeffs[d1 + d2 - power] += coefficient;
        }

        return new Polynomial(multCoeffs);
    }


    public static Polynomial LagrangeInterpolation(List<double> xs, List<double> ys)
    {
        var res = new Polynomial(0);
        var count = xs.Count;
        for (var i = 0; i < count; i++)
        {
            var xsc = Enumerable.Range(0, count)
                .Where(q => q != i)
                .Select(q => new Polynomial(1, -xs[i]));
            var uni = new Polynomial(1.0);
            foreach (var poly in xsc)
                uni *= poly;
            var y = uni.Evaluate(xs[i]);
            var coeff = ys[i] / y;
            res += uni * coeff;
        }

        return res;
    }
}