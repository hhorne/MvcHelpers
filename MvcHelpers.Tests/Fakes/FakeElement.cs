using System;
using System.Configuration;

namespace MvcHelpers.Tests.Fakes
{
	public class FakeConfiguration : ConfigurationSection
	{
		[ConfigurationProperty("FakeElementCollection")]
		[ConfigurationCollection(typeof(FakeElementCollection), AddItemName = "FakeElement")]
		public FakeElementCollection FakeElementCollection
		{
			get
			{
				object o = this["FakeElementCollection"];
				return o as FakeElementCollection;
			}
		}
	}

	public class FakeElementCollection : ConfigurationElementCollection
	{
		public FakeElement this[int index]
		{
			get
			{
				return base.BaseGet(index) as FakeElement;
			}
			set
			{
				if (base.BaseGet(index) != null)
				{
					base.BaseRemoveAt(index);
				}
				this.BaseAdd(index, value);
			}
		}

		protected override ConfigurationElement CreateNewElement()
		{
			return new FakeElement();
		}

		protected override object GetElementKey(ConfigurationElement element)
		{
			return ((FakeElement) element).Foo;
		}
	}

	public class FakeElement : ConfigurationElement
	{
		[ConfigurationProperty("Foo", IsRequired = true)]
		public int Foo 
		{
			get
			{
				return Convert.ToInt32(this["Foo"]);
			}
		}
		
		[ConfigurationProperty("Bar", IsRequired = true)]
		public string Bar
		{
			get
			{
				return this["Bar"] as string;
			}
		}
		
		[ConfigurationProperty("Baz", IsRequired = false)]
		public bool Baz
		{
			get
			{
				return Convert.ToBoolean(this["Baz"]);
			}
		}
	}
}
