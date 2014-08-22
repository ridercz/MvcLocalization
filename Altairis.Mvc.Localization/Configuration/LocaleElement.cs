using System;
using System.Configuration;
using System.Globalization;

namespace Altairis.Mvc.Localization.Configuration {

    public class LocaleElement : ConfigurationElement {

        [ConfigurationProperty("prefix", IsRequired = true, DefaultValue = "xx")]
        [RegexStringValidator("^[A-Za-z-]{1,}$")]
        public string Prefix {
            get { return (string)this["prefix"]; }
            set { this["prefix"] = value; }
        }

        [ConfigurationProperty("culture", IsRequired = true, DefaultValue = "")]
        public CultureInfo Culture {
            get { return (CultureInfo)this["culture"]; }
            set { this["culture"] = value; }
        }

        [ConfigurationProperty("uiCulture", IsRequired = true, DefaultValue = "")]
        public CultureInfo UiCulture {
            get { return (CultureInfo)this["uiCulture"]; }
            set { this["uiCulture"] = value; }
        }

        protected override void PostDeserialize() {
            base.PostDeserialize();
            if (this.Culture.Equals(CultureInfo.InvariantCulture)) throw new ConfigurationErrorsException("Invariant culture cannot be used here.");
            if (this.Culture.IsNeutralCulture) throw new ConfigurationErrorsException(string.Format("Specified culture \"{0}\" is a neutral culture and cannot be used in this context.", this.Culture));
            if (this.UiCulture.Equals(CultureInfo.InvariantCulture)) throw new ConfigurationErrorsException("Invariant culture cannot be used here.");
        }
    }

    public class LocaleElementCollection : ConfigurationElementCollection {

        protected override ConfigurationElement CreateNewElement() {
            return new LocaleElement() as ConfigurationElement;
        }

        protected override object GetElementKey(ConfigurationElement element) {
            if (element == null) throw new ArgumentNullException("element");
            var le = element as LocaleElement;
            if (le == null) throw new ArgumentException("Value must be LocaleElement.", "element");
            return le.Prefix;
        }
    }
}