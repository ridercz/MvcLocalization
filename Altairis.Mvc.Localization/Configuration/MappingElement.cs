using System;
using System.Configuration;

namespace Altairis.Mvc.Localization.Configuration {

    public class MappingElement : ConfigurationElement {

        [ConfigurationProperty("language", IsRequired = true)]
        public string Language {
            get { return (string)this["language"]; }
            set { this["language"] = value; }
        }

        [ConfigurationProperty("locale", IsRequired = true, DefaultValue = "xx")]
        [RegexStringValidator("^[A-Za-z-]{1,}$")]
        public string Locale {
            get { return (string)this["locale"]; }
            set { this["locale"] = value; }
        }
    }

    public class MappingElementCollection : ConfigurationElementCollection {

        protected override ConfigurationElement CreateNewElement() {
            return new MappingElement() as ConfigurationElement;
        }

        protected override object GetElementKey(ConfigurationElement element) {
            if (element == null) throw new ArgumentNullException("element");
            var me = element as MappingElement;
            if (me == null) throw new ArgumentException("Value must be MappingElement.", "element");
            return me.Language;
        }
    }
}