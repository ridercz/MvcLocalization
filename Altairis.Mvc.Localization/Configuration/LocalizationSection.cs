using System;
using System.Configuration;
using System.Linq;

namespace Altairis.Mvc.Localization.Configuration {

    public class LocalizationSection : ConfigurationSection {

        [ConfigurationProperty("fallbackLocale", IsRequired = true, DefaultValue = "xx")]
        [RegexStringValidator("^[A-Za-z-]{1,}$")]
        public string FallbackLocale {
            get { return (string)this["fallbackLocale"]; }
            set { this["fallbackLocale"] = value; }
        }

        [ConfigurationProperty("locales", IsRequired = true)]
        public LocaleElementCollection Locales {
            get { return (LocaleElementCollection)this["locales"]; }
            set { this["locales"] = value; }
        }

        [ConfigurationProperty("mappings", IsRequired = true)]
        public MappingElementCollection Mappings {
            get { return (MappingElementCollection)this["mappings"]; }
            set { this["mappings"] = value; }
        }

        [ConfigurationProperty("exceptions", IsRequired = false)]
        public ExceptionElementCollection Exceptions {
            get { return (ExceptionElementCollection)this["exceptions"]; }
            set { this["exceptions"] = value; }
        }

        protected override void PostDeserialize() {
            base.PostDeserialize();

            if (this.Locales == null || this.Locales.Count == 0) throw new ConfigurationErrorsException("Locales collection cannot be missing or empty.");
            if (this.Mappings == null || this.Mappings.Count == 0) throw new ConfigurationErrorsException("Mappings collection cannot be missing or empty.");
            if (!this.Locales.Cast<LocaleElement>().Any(x => x.Prefix.Equals(this.FallbackLocale, StringComparison.OrdinalIgnoreCase))) throw new ConfigurationErrorsException("Fallback locale does not match any specified locale.");
        }
    }
}