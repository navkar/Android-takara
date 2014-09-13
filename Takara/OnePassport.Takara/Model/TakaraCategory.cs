using System;
using System.Runtime.Serialization;
using System.ComponentModel;
using System.Collections.ObjectModel;


namespace OnePassport.Takara.Model
{
    [DataContract]
    public class TakaraCategory : ModelBase
    {
        private ObservableCollection<OnePassport.Takara.Model.Takara> takaras;

        [DataMember]
        public ObservableCollection<OnePassport.Takara.Model.Takara> Takaras
        {
            get { return takaras; }
            private set
            {
                if (takaras != value)
                {
                    takaras = value;
                    NotifyPropertyChanged("Takaras");
                }
            }
        }

        public TakaraCategory()
        {
            Takaras = new ObservableCollection<Takara>();
        }
    }
}
