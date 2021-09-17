using System;

namespace AlsRitter.Utilities {
    /**
     * 处理浮点型比较大小的方法
     */
    internal class DoubleUtil {
        // Fields 浮点型的误差
        private const double DOUBLE_DELTA = 1E-06;

        public static bool AreEqual(double value1, double value2) {
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            return (value1 == value2) // 可能指向同一个对象
                || Math.Abs(value1 - value2) < DOUBLE_DELTA;
        }

        public static bool GreaterThan(double value1, double value2) {
            return ((value1 > value2) && !AreEqual(value1, value2));
        }

        public static bool GreaterThanOrEqual(double value1, double value2) {
            return (value1 > value2) || AreEqual(value1, value2);
        }

        public static bool IsZero(double value) {
            return (Math.Abs(value) < DOUBLE_DELTA);
        }

        public static bool LessThan(double value1, double value2) {
            return ((value1 < value2) && !AreEqual(value1, value2));
        }

        public static bool LessThanOrEqual(double value1, double value2) {
            return (value1 < value2) || AreEqual(value1, value2);
        }
    }
}