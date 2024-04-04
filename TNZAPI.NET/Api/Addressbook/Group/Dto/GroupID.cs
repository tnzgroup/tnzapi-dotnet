using System.Xml.Serialization;

namespace TNZAPI.NET.Api.Addressbook.Contact.Dto
{
	[XmlType(TypeName = "GroupID")]
	public record GroupID
	{
		[XmlText]
		public string Value { get; set; }

		public GroupID(string value)
		{
			Value = value;
		}

		public GroupID(object obj)
		{
			if (obj is string)
			{
				Value = (string)obj;
				return;
			}
			if (obj is GroupID)
			{
				Value = (GroupID)obj;
				return;
			}

			throw new Exception($"Unsupported type - {obj}");
		}

		// Required for XmlSerializer
		public GroupID()
		{
		}

		public static implicit operator string(GroupID groupID) => groupID.Value;

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
