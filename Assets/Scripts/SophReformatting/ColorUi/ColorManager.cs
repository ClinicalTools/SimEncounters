﻿using UnityEngine;

namespace ClinicalTools.SimEncounters.ColorUi
{
    // while assigning each value of an enum to a number is usually frowned upon,
    // this allows new values to be added anywhere in the enum wihtout messing up serialized uses
    public enum ColorType
    {
        PrimaryColor = 0,
        DarkColor = 1,
        LightColor = 2
    }
    public class ColorManager : MonoBehaviour
    {
        private static readonly Color primaryColor = new Color(0.2784314f, 0.4666667f, 0.6980392f);
        private static readonly Color darkColor = new Color(0.1960784f, 0.3529412f, 0.5529412f);
        private static readonly Color lightColor = new Color(0.4150943f, 0.601305f, 0.8301887f);

        public static Color GetColor(ColorType colorType)
        {
            switch (colorType) {
                case ColorType.PrimaryColor:
                    return primaryColor;
                case ColorType.DarkColor:
                    return darkColor;
                case ColorType.LightColor:
                    return lightColor;
                default:
                    return primaryColor;
            }
        }
    }
}