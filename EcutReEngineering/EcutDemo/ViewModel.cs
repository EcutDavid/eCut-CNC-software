using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using System.Windows.Media;

namespace P1S1
{
    [ValueConversion(typeof(bool), typeof(SolidColorBrush))]
    class LEDConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ((bool)value == false)
            {
                return new SolidColorBrush(Color.FromRgb(0x80, 0x80, 0x80));
            }
            return new SolidColorBrush(Color.FromRgb(0, 255, 0));
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }

    class LEDManager : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool enableFlag;

        public bool EnableFlag
        {
            get { return enableFlag; }
            set
            {
                enableFlag = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("EnableFlag"));
                }
            }
        }
    }

    class GLNumber : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        public GLNumber()
        {
            Number = "40";
        }

        private string number;

        public string Number
        {
            get
            {
                return number;
            }
            set
            {
                this.number = value;
                if (PropertyChanged != null)
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Number"));
            }
        }


    }

    class bindingNumber : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public bindingNumber()
        {
            Value = 0;
        }

        private string percentage;
        public string Percentage
        {
            get
            {
                return percentage;
            }
            set
            {
                this.percentage = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Percentage"));
                }
            }
        }

        private Double value;
        public Double Value
        {
            get
            {
                return this.value;
            }
            set
            {
                this.value = Math.Round(value, 3);
                Percentage = ((int)(this.value * 100 / 10.0)).ToString() + "%";
                DisplayNumber = this.value.ToString();
                if (PropertyChanged != null)
                {
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Value"));
                }
            }
        }

        private string displayNumber;

        public string DisplayNumber
        {
            get
            {
                return displayNumber;
            }
            set
            {
                displayNumber = value;
                if (!displayNumber.Contains('.'))
                {
                    displayNumber += ".000";
                }
                displayNumber.Split('.');
                displayNumber += new String('0', 3 - displayNumber.Split('.')[1].ToCharArray().Length);
                if (PropertyChanged != null)
                {
                    PropertyChanged.Invoke(this, new PropertyChangedEventArgs("DisplayNumber"));
                }
            }
        }

    }

}
