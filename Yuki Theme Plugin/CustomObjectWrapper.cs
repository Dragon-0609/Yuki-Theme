using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Yuki_Theme_Plugin
{
	public class CustomObjectWrapper : CustomTypeDescriptor
	{
		public object       WrappedObject       { get; private set; }
		public List<string> BrowsableProperties { get; private set; }
		public CustomObjectWrapper(object o)
			:base(TypeDescriptor.GetProvider(o).GetTypeDescriptor(o))
		{
			WrappedObject = o;
			BrowsableProperties = new List<string>() { "Text", "Controls", "BackColor" };
		}
		public override PropertyDescriptorCollection GetProperties()
		{
			return this.GetProperties(new Attribute[] { });
		}
		public override PropertyDescriptorCollection GetProperties(Attribute[] attributes)
		{
			var properties = base.GetProperties(attributes).Cast<PropertyDescriptor>()
			                     .Where(p=>BrowsableProperties.Contains(p.Name))
			                     .Select(p => TypeDescriptor.CreateProperty(
				                             WrappedObject.GetType(),
				                             p,
				                             p.Attributes.Cast<Attribute>().ToArray()))
			                     .ToArray();
			return new PropertyDescriptorCollection(properties);
		}
	}
}