﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using UE = UnityEngine;

namespace Unity.API
{
    public static class UnityGdiHelper
    {
        public static Point FromVector2(UE.Vector2 vector)
        {
            return new Point((int)vector.x, (int)vector.y);
        }
        public static Bitmap ToBitmap(this UE.Texture2D texture)
        {
            if (texture == null)
                return null;

            return Bitmap.FromTexture(Graphics.ApiGraphics.CreateTexture(texture));
        }
        public static Color ToColor(this UE.Color color)
        {
            return Color.FromArgb((int)(color.a * 255), (int)(color.r * 255), (int)(color.g * 255), (int)(color.b * 255));
        }
        public static UE.Vector2 ToVector2(this Point point)
        {
            return new UE.Vector2(point.X, point.Y);
        }
        public static UE.Color ToUnityColor(this Color color)
        {
            return new UnityEngine.Color((float)color.R / 255, (float)color.G / 255, (float)color.B / 255, (float)color.A / 255);
        }
    }
}
