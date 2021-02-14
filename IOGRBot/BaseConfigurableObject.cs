using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IOGRBot
{
    public abstract class BaseConfigurableObject
    {
        public BaseConfigurableObject(object config)
        {
            EnsureConfigurationExists(config);
        }

        private void EnsureConfigurationExists(object config)
        {
            if (config == null)
            {
                throw new ArgumentException($"Missing object configuration for type {GetType()}. Cannot start application.");
            }
            var props = config.GetType().GetProperties();

            foreach (PropertyInfo pi in props)
            {
                string value = (string)pi.GetValue(config);
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException($"Missing configuration value: {pi.Name}. Cannot start application.");
                }
            }
        }
    }
}
