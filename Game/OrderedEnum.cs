using System;

/// <summary>
/// Wrapper class for enums that are represented as ints, start from zero and all values are consecutive.
/// </summary>
public class OrderedEnum<TEnum> where TEnum : struct, System.Enum {
    private TEnum _e;

    /// <summary>Constructor to wrap an enum from its string representation.</summary>
    /// <param name=n>Human readable name for an enum.</param>
    /// <returns>Wrapper that represents the enum indicated by the name received.</returns>
    /// <exception cref="ArgumentException">The name provided does not represent a valid enum.</exception>
    public OrderedEnum(string name) {
        if (Enum.TryParse(name, out TEnum e)) {
            this.SetEnum(e);
        } else {
            throw new ArgumentException();
        }
    }

    /// <summary>Constructor to wrap an enum from its integer representation.</summary>
    /// <param name=n>Number representing a valid underlying value for TEnum enum.</param>
    /// <returns>Wrapper that represents the enum indicated by the value received.</returns>
    /// <exception cref="ArgumentException">The number provided is negative or greater than the amount of values.</exception>
    public OrderedEnum(int i) {
        if (i < 0 || i >= OrderedEnum<TEnum>.GetNumEnums()) {
            throw new ArgumentException();
        }

        this.SetEnum(OrderedEnum<TEnum>.FromInt(i));
    }

    private TEnum GetEnum() {
        return _e;
    }

    private void SetEnum(TEnum e) {
        _e = e;
    }

    /// <returns>The number of values that the wrapped enum defines.</returns>
    public static int GetNumEnums() {
        return Enum.GetNames(typeof(TEnum)).Length;
    }

    /// <returns>The string representation the wrapped enum.</returns>
    public string GetName() {
        return this.GetEnum().ToString();
    }

    /// <returns>A positive number if this is greater than the argument, zero i they are equal and a negative number if this is smaller.</returns>
    public int CompareTo(OrderedEnum<TEnum> e) {
        int i1 = this.ToInt();
        int i2 = e.ToInt();

        return i1.CompareTo(i2);
    }

    /// <summary>Method used to change this enum into the next value. If this is the greatest value then it stays the same.</summary>
    public void NextEnum() {
        int thisEnum = this.ToInt();

        if (thisEnum >= OrderedEnum<TEnum>.GetNumEnums()) {
            return;
        }

        this.SetEnum(OrderedEnum<TEnum>.FromInt(thisEnum + 1));
    }

    /// <summary>Method used to change this enum into the next value. If this is the greatest value then it becomes the smallest one.</summary>
    public void NextEnumWrap() {
        int thisEnum = this.ToInt();
        int nextEnum = (thisEnum + 1) % OrderedEnum<TEnum>.GetNumEnums();

        this.SetEnum(OrderedEnum<TEnum>.FromInt(nextEnum));
    }

    /// <param name=e>True.</param>
    /// <returns>True if e is the next value after this, false otherwise.</returns>
    public bool IsNextEnum(OrderedEnum<TEnum> e) {
        int thisEnum = this.ToInt();
        int thatEnum = e.ToInt();

        return (thisEnum + 1)  == thatEnum;
    }

    /// <param name=e>True.</param>
    /// <returns>True if e is the next value after this or if this is the last element and e is the first. Otherwise returns false.</returns>
    public bool IsNextEnumWrap(OrderedEnum<TEnum> e) {
        int thisEnum = this.ToInt();
        int thatEnum = e.ToInt();
        int numEnums = OrderedEnum<TEnum>.GetNumEnums();

        return ((thisEnum + 1) % numEnums) == thatEnum;
    }

    private bool IsNthEnum(int n) {
        return this.ToInt() == n;
    }

    /// <returns>True if this is the first enum in the ordering defined, false otherwise.</returns>
    public bool IsFirstEnum() {
        return this.IsNthEnum(0);
    }

    /// <returns>True if this is the last enum in the ordering defined, false otherwise.</returns>
    public bool IsLastEnum() {
        return this.IsNthEnum(OrderedEnum<TEnum>.GetNumEnums() - 1);
    }

    private int ToInt() {
        return Convert.ToInt32(this.GetEnum());
    }

    private static TEnum FromInt(Int32 i) {
        return (TEnum)Enum.Parse(typeof(TEnum), i.ToString(), true);
    }

    /// <returns>Produes the first enum in the sequence.</returns>
    public static OrderedEnum<TEnum> FirstEnum() {
        return new OrderedEnum<TEnum>(0);
    }

    /// <returns>Produes the last enum in the sequence.</returns>
    public static OrderedEnum<TEnum> LastEnum() {
        int last = GetNumEnums() - 1;

        return new OrderedEnum<TEnum>(last);
    }
}
