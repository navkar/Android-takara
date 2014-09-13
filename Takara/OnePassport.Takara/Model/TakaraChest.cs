using System;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;
using System.ComponentModel;


namespace OnePassport.Takara.Model
{
    [DataContract]
    public class TakaraChest : ModelBase
    {
        private int version;
        private ObservableCollection<OnePassport.Takara.Model.TakaraCategory> takaras;

        [DataMember]
        public int Version
        {
            get { return version; }
            set
            {
                if (version != value)
                {
                    version = value;
                    NotifyPropertyChanged("Version");
                }
            }
        }

        //[DataMember]
        //public int HashCode
        //{
        //    get
        //    {
        //        return Takaras.GetHashCode();            
        //    }
        //}

        [DataMember]
        public ObservableCollection<OnePassport.Takara.Model.TakaraCategory> Takaras
        {
            get { return takaras; }
            set
            {
                if (takaras != value)
                {
                    takaras = value;
                    NotifyPropertyChanged("Takaras");
                }
            }
        }

        public TakaraChest()
        {
            Takaras = new ObservableCollection<TakaraCategory>();
        }
    }
}
