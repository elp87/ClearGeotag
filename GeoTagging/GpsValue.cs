using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Imaging;

namespace GeoTagging
{
    public class GpsValue
    {
        private float _degree, _minutes, _seconds;

        public GpsValue(PropertyItem ItemRef, PropertyItem Item)
        {
            this.CalcValue(ItemRef, Item);
        }

        public float Degree { get { return this._degree; } }
        public float Minutes { get { return this._minutes; } }
        public float Seconds { get { return this._seconds; } }

        private void CalcValue(PropertyItem ItemRef, PropertyItem Item)
        {
            UInt32 latitudeDegreeNumerator = BitConverter.ToUInt32(Item.Value, 0);
            UInt32 latitudeDegreeDenominator = BitConverter.ToUInt32(Item.Value, 4);
            this._degree = (float)latitudeDegreeNumerator / (float)latitudeDegreeDenominator;

            UInt32 latitudeMinutesNumerator = BitConverter.ToUInt32(Item.Value, 8);
            UInt32 latitudeMinutesDenominator = BitConverter.ToUInt32(Item.Value, 12);
            this._minutes = (float)latitudeMinutesNumerator / (float)latitudeMinutesDenominator;

            UInt32 latitudeSecondsNumerator = BitConverter.ToUInt32(Item.Value, 16);
            UInt32 latitudeSecondsDenominator = BitConverter.ToUInt32(Item.Value, 20);
            this._seconds = (float)latitudeSecondsNumerator / (float)latitudeSecondsDenominator;
        }

        public override string ToString()
        {
            return this._degree.ToString() + @"° " + this._minutes.ToString() + @"' " + this._seconds.ToString() + "\"";
        }
    }
}
