using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class ColorHelper
{
    public static Color Saturation(Color color, float saturate)
    {
        Vector3 hlsColor = RGBToHSL(new Vector3(color.r,color.g,color.b));
        Vector3 rgbColor = HSLToRGB(new Vector3(hlsColor.x, hlsColor.y*saturate, hlsColor.z));
        return new Color(rgbColor.x,rgbColor.y,rgbColor.z);
    }
    
    public static Color ContrastSaturationBrightness(Color color, float brt, float sat, float con)
    {
        // Increase or decrease theese values to adjust r, g and b color channels seperately
        const float avgLumR = 0.5f;
        const float avgLumG = 0.5f;
        const float avgLumB = 0.5f;

        Vector3 LumCoeff = new Vector3(0.2125f, 0.7154f, 0.0721f);

        Vector3 AvgLumin = new Vector3(avgLumR, avgLumG, avgLumB);
        Vector3 brtColor = new Vector3(color.r, color.g, color.b)*brt;
        float intensityf = Vector3.Dot(brtColor, LumCoeff);
        Vector3 intensity = new Vector3(intensityf, intensityf, intensityf);
        Vector3 satColor = Vector3.Lerp(intensity, brtColor, sat);
        Vector3 conColor = Vector3.Lerp(AvgLumin, satColor, con);
        return new Color(conColor.x, conColor.y, conColor.z);
    }

    public static Vector3 RGBToHSL(Vector3 color)
    {
        Vector3 hsl = Vector3.zero; // init to 0 to avoid warnings ? (and reverse if + remove first part)

        float fmin = Mathf.Min(Mathf.Min(color.x, color.y), color.z);    //Min. value of RGB
        float fmax = Mathf.Max(Mathf.Max(color.x, color.y), color.z);    //Max. value of RGB
        float delta = fmax - fmin;             //Delta RGB value

        hsl.z = (fmax + fmin) / 2.0f; // Luminance
        
        if (delta == 0.0f)		//This is a gray, no chroma...
        {
            hsl.x = 0.0f;	// Hue
            hsl.y = 0.0f;	// Saturation
        }
        else   //Chromatic data...
        {
            if (hsl.z < 0.5f)
                hsl.y = delta / (fmax + fmin); // Saturation
            else
                hsl.y = delta / (2.0f - fmax - fmin); // Saturation

            float deltaR = (((fmax - color.x) / 6.0f) + (delta / 2.0f)) / delta;
            float deltaG = (((fmax - color.y) / 6.0f) + (delta / 2.0f)) / delta;
            float deltaB = (((fmax - color.z) / 6.0f) + (delta / 2.0f)) / delta;

            if (color.x == fmax)
                hsl.x = deltaB - deltaG; // Hue
            else if (color.y == fmax)
                hsl.x = (1.0f / 3.0f) + deltaR - deltaB; // Hue
            else if (color.z == fmax)
                hsl.x = (2.0f / 3.0f) + deltaG - deltaR; // Hue

            if (hsl.x < 0.0f)
                hsl.x += 1.0f; // Hue
            else if (hsl.x > 1.0f)
                hsl.x -= 1.0f; // Hue
        }

        return hsl;
    }

    public static Vector3 HSLToRGB(Vector3 hsl)
    {
        Vector3 rgb;

        if (hsl.y == 0.0)
            rgb = new Vector3(hsl.z, hsl.z, hsl.z); // Luminance
        else
        {
            float f2;

            if (hsl.z < 0.5f)
                f2 = hsl.z * (1.0f + hsl.y);
            else
                f2 = (hsl.z + hsl.y) - (hsl.y * hsl.z);

            float f1 = 2.0f * hsl.z - f2;

            rgb.x = HueToRGB(f1, f2, hsl.x + (1.0f / 3.0f));
            rgb.y = HueToRGB(f1, f2, hsl.x);
            rgb.z = HueToRGB(f1, f2, hsl.x - (1.0f / 3.0f));
        }

        return rgb;
    }

    public static float HueToRGB(float f1, float f2, float hue)
    {
        if (hue < 0.0f)
            hue += 1.0f;
        else if (hue > 1.0f)
            hue -= 1.0f;
        float res;
        if ((6.0 * hue) < 1.0f)
            res = f1 + (f2 - f1) * 6.0f * hue;
        else if ((2.0f * hue) < 1.0f)
            res = f2;
        else if ((3.0f * hue) < 2.0f)
            res = f1 + (f2 - f1) * ((2.0f / 3.0f) - hue) * 6.0f;
        else
            res = f1;
        return res;
    }
}

