using System.Xml.Serialization;

namespace TNZAPI.NET.Api.Addressbook.Contact.Dto
{
	[XmlType(TypeName = "ContactID")]
	public record ContactID
	{
		[XmlText]
		public string Value { get; set; }

		public ContactID(string value)
		{
			Value = value;
		}

		public ContactID(object obj)
		{
			if(obj is string)
			{
				Value = (string)obj;
				return;
			}
			if(obj is ContactID)
			{
				Value = (ContactID)obj;
				return;
			}

			throw new Exception($"Unsupported type - {obj}");
		}

		// Required for XmlSerializer
		public ContactID()
		{
		}

		public static implicit operator string(ContactID contactID) => contactID.Value;

		public override string ToString()
		{
			if (Value is null)
			{
				return null;
			}

			return $"{Value}";
		}
	}
}
