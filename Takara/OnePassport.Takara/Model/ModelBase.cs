using System;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace OnePassport.Takara.Model
{
    [DataContract]
    public class ModelBase
    {
        private Guid id;
        private string name;
        private string desc;
        private int position;

        [DataMember]
        public Guid Id
        {
            get { return id; }
            set
            {
                if (id != value)
                {
                    id = value;
                    NotifyPropertyChanged("Id");
                }
            }
        }

        [DataMember]
        public string Name
        {
            get { return name; }
            set
            {
                if (name != value)
                {
                    name = value;
                    NotifyPropertyChanged("Name");
                }
            }
        }

        [DataMember]
        public string Desc
        {
            get 
            { 
                return desc; 
            }

            set
            {
                if (desc != value)
                {
                    desc = value;
                    NotifyPropertyChanged("Desc");
                }
            }
        }

        /// <summary>
        /// UI placement position of the entity
        /// </summary>
        [DataMember]
        public int Order
        {
            get { return position; }
            set
            {
                if (position != value)
                {
                    position = value;
                    NotifyPropertyChanged("Order");
                }
            }
        }

        #region INotifyPropertyChanged Event

        public event PropertyChangedEventHandler PropertyChanged;


        protected void NotifyPropertyChanged(String propertyName)
        {
#if ClientUse
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
#endif
        }
        #endregion
    }
}
