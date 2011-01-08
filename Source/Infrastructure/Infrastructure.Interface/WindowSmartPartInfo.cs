using System.Collections.Generic;

namespace ACOT.Infrastructure.Interface
{
    public class WindowSmartPartInfo : Microsoft.Practices.CompositeUI.WinForms.WindowSmartPartInfo
    {
        private Dictionary<string, object> _settings = new Dictionary<string, object>();

        public IDictionary<string, object> Keys
        {
            get { return _settings; }
        }
    }
}
