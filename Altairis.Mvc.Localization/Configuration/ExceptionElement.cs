using System;
using System.Configuration;

namespace Altairis.Mvc.Localization.Configuration {

    public enum ExceptionMatchType {
        Exact = 1,
        StartsWith = 2,
        EndsWith = 3,
        Regex = 4
    }

    public class ExceptionElement : ConfigurationElement {

        [ConfigurationProperty("path", IsRequired = true, DefaultValue = "")]
        public string Path {
            get { return (string)this["path"]; }
            set { this["path"] = value; }
        }

        [ConfigurationProperty("match", IsRequired = false, DefaultValue = ExceptionMatchType.StartsWith)]
        public ExceptionMatchType Match {
            get { return (ExceptionMatchType)this["match"]; }
            set { this["match"] = value; }
        }

    }

    public class ExceptionElementCollection : ConfigurationElementCollection {

        protected override ConfigurationElement CreateNewElement() {
            return new ExceptionElement() as ConfigurationElement;
        }

        protected override object GetElementKey(ConfigurationElement element) {
            if (element == null) throw new ArgumentNullException("element");
            var e = element as ExceptionElement;
            if (e == null) throw new ArgumentException("Value must be ExceptionElement.", "element");
            return e.Path;
        }
    }
}