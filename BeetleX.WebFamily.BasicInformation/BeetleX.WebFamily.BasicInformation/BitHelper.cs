using System;
using System.Collections.Generic;
using System.Text;

namespace BeetleX.WebFamily.BasicInformation
{
    public class BitHelper
    {

        public static List<bool> ToValues(byte[] data)
        {
            List<bool> result = new List<bool>();
            for (int i = data.Length - 1; i >= 0; i--)
            {
                result.AddRange(ToValues(data[i]));
            }
            return result;
        }
        public static List<bool> ToValues(byte data)
        {
            List<bool> result = new List<bool>();
            for (int i = 7; i >= 0; i--)
            {
                result.Add(((data >> i) & 1u) > 0);
            }
            return result;
        }

        public static List<bool> ToValues(uint data)
        {
            List<bool> result = new List<bool>();
            for (int i = 31; i >= 0; i--)
            {
                result.Add(((data >> i) & 1u) > 0);
            }
            return result;
        }

        public static List<bool> ToValues(ulong data)
        {
            List<bool> result = new List<bool>();
            for (int i = 63; i >= 0; i--)
            {
                result.Add(((data >> i) & 1u) > 0);
            }
            return result;
        }

        public static string ToString(byte[] data)
        {
            string result = "";
            for (int i = data.Length - 1; i >= 0; i--)
            {
                result += ToString(data[i]);
            }
            return result;
        }
        public static string ToString(byte data)
        {
            string result = "";
            for (int i = 7; i >= 0; i--)
            {
                result += (data >> i) & 1u;
            }
            return result;
        }

        public static string ToString(uint data)
        {
            string result = "";
            for (int i = 31; i >= 0; i--)
            {
                result += (data >> i) & 1u;
            }
            return result;
        }

        public static string ToString(ulong data)
        {
            // return Convert.ToString(data, 2);
            string result = "";
            for (int i = 63; i >= 0; i--)
            {
                result += (data >> i) & 1u;
            }
            return result;
        }

        public static bool GetValue(uint data, byte postion)
        {
            if (postion > 31)
                throw new Exception("Offset position overflow");
            return (data & (1u << postion)) > 0;
        }

        public static uint Enabled(uint data, byte postion)
        {
            if (postion > 31)
                throw new Exception("Offset position overflow");
            return (data | (1u << postion));
        }

        public static uint Disable(uint data, byte postion)
        {
            if (postion > 31)
                throw new Exception("Offset position overflow");
            return (data & ~(1u << postion));
        }

        public static bool GetValue(ulong data, byte postion)
        {
            if (postion > 63)
                throw new Exception("Offset position overflow");
            return (data & (1ul << postion)) > 0;
        }

        public static ulong Enabled(ulong data, byte postion)
        {
            if (postion > 63)
                throw new Exception("Offset position overflow");
            return (data | (1ul << postion));
        }

        public static ulong Disable(ulong data, byte postion)
        {
            if (postion > 63)
                throw new Exception("Offset position overflow");
            return (data & ~(1ul << postion));
        }

        public static bool GetValue(byte data, byte postion)
        {
            if (postion > 7)
                throw new Exception("Offset position overflow");
            return (data & (1u << postion)) > 0;
        }

        public static byte Enabled(byte data, byte postion)
        {
            if (postion > 7)
                throw new Exception("Offset position overflow");
            return (byte)(data | ((byte)1 << postion));
        }

        public static byte Disable(byte data, byte postion)
        {
            if (postion > 7)
                throw new Exception("Offset position overflow");
            return (byte)(data & ~((byte)1 << postion));
        }


        public static bool GetValue(byte[] data, uint postion)
        {
            if (postion >= (8 * data.Length - 1))
                throw new Exception("Offset position overflow");
            int index = (int)(postion / 8);
            byte subPos = (byte)(postion % 8);
            return GetValue(data[index], subPos);
        }

        public static void Enabled(byte[] data, uint postion)
        {
            if (postion >= (8 * data.Length - 1))
                throw new Exception("Offset position overflow");
            int index = (int)(postion / 8);
            byte subPos = (byte)(postion % 8);
            data[index] = Enabled(data[index], subPos);
        }

        public static void Disable(byte[] data, uint postion)
        {
            if (postion >= (8 * data.Length - 1))
                throw new Exception("Offset position overflow");
            int index = (int)(postion / 8);
            byte subPos = (byte)(postion % 8);
            data[index] = Disable(data[index], subPos);
        }
    }
}
