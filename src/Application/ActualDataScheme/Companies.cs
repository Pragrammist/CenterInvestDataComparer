

using System.Xml.Serialization;


namespace Application;

public class TextData { 

	[XmlAttribute(AttributeName="lang")] 
	public string Lang { get; set; }  = null!;

	[XmlText] 
	public string Text { get; set; }  = null!;
}




[XmlRoot(ElementName="coordinates")]
public class Coordinates { 

	[XmlElement(ElementName="lat")] 
	public double Lat { get; set; } 

	[XmlElement(ElementName="lon")] 
	public double Lon { get; set; } 
}

[XmlRoot(ElementName="phone")]
public class Phone { 

	[XmlElement(ElementName="type")] 
	public string Type { get; set; } = null!;

	[XmlElement(ElementName="number")] 
	public string Number { get; set; } = null!;

	[XmlElement(ElementName="info")] 
	public string Info { get; set; } = null!;
}

[XmlRoot(ElementName="company")]
public class Company { 

	[XmlElement(ElementName="name")] 
	public TextData Name { get; set; } = null!;

	[XmlElement(ElementName="name-other")] 
	public TextData Nameother { get; set; } = null!;

	[XmlElement(ElementName="shortname")] 
	public TextData Shortname { get; set; } = null!;

	[XmlElement(ElementName="address")] 
	public TextData Address { get; set; } = null!;

	[XmlElement(ElementName="country")] 
	public TextData Country { get; set; } = null!;

	[XmlElement(ElementName="working-time")] 
	public TextData Workingtime { get; set; } = null!;

	[XmlElement(ElementName="coordinates")] 
	public Coordinates Coordinates { get; set; } = null!;

	[XmlElement(ElementName="phone")] 
	public List<Phone> PhoneList { get; set; } = null!;

	[XmlElement(ElementName="address-add")] 
	public TextData Addressadd { get; set; } = null!;
}


[XmlRoot(ElementName="companies")]
public class Companies { 

	[XmlElement(ElementName="company")] 
	public Company[] CompanyList { get; set; } = null!;
}

