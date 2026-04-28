using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities;

public  class Device
{
    public int DeviceID { get; private set; }
    public int BrandID { get; private set; }
    public int TypeID { get; private set; }
    public int SpecID { get; private set; }
    public string ?  SerialNumber { get; private set; }
    public string? ModelName { get; private set; }

    public Device(string? serialNumber, string? modelName, int brandID, int typeID, int specID)
    {
        Validation(brandID, typeID, specID);

        SerialNumber = serialNumber?.Trim();
        ModelName = modelName?.Trim();
        BrandID = brandID;
        TypeID = typeID;
        SpecID = specID;
    }

    private void Validation( int brandID, int typeID, int specID)
    {
        if (brandID <= 0)
            throw new ArgumentException("Brand غير صالح.");

        if (typeID <= 0)
            throw new ArgumentException("Type غير صالح.");

        if (specID <= 0)
            throw new ArgumentException("Spec غير صالح.");
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(obj , null))
            return  false;

        if (ReferenceEquals(obj , this))
            return  true;

        if (obj is not Device other)
            return false;

        return  this.SerialNumber == other.SerialNumber;

    }

    public override int GetHashCode()
    {

        if (SerialNumber == null)
            return base.GetHashCode();


        return SerialNumber.GetHashCode();
    }

}
