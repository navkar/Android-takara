using System;
using System.Runtime.Serialization;

namespace OnePassport.Takara.Model
{
    [DataContract]
    public class Takara : ModelBase
    {
        private string storedValue;

        [DataMember]
        public string Value 
        {
            get { return storedValue; }
            set
            {
                if (storedValue != value)
                {
                    storedValue = value;
                    NotifyPropertyChanged("Value");
                }
            }

        }
    }
}
