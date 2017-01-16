using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using DocumentsExchange.BusinessLayer.Services.Interfaces;
using DocumentsExchange.DataLayer.Entity;

namespace DocumentsExchange.BusinessLayer.Services.Implementations
{
   public class GetCurrencyCourse: IGetCurrencyCourse
   {
       private readonly decimal _euroCurrency;
       private readonly decimal _usdCurrency;

       public GetCurrencyCourse()
       {
            XmlTextReader reader = new XmlTextReader("http://www.cbr.ru/scripts/XML_daily.asp");
       
            string USDXml = "";
            string EuroXML = "";
 
            while (reader.Read())
            {
                
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element:
                        if (reader.Name == "Valute")
                        {
                            if (reader.HasAttributes)
                            {
                                while (reader.MoveToNextAttribute())
                                {
                                    if (reader.Name == "ID")
                                    {
                                        if (reader.Value == "R01235")
                                        {
                                            reader.MoveToElement();
                                            USDXml = reader.ReadOuterXml();
                                        }
                                    }

                                    if (reader.Name == "ID")
                                    {
                                        if (reader.Value == "R01239")
                                        {
                                            reader.MoveToElement();
                                            EuroXML = reader.ReadOuterXml();
                                        }
                                    }
                                }
                            }
                        }

                        break;
                }
            }


            XmlDocument usdXmlDocument = new XmlDocument();
            usdXmlDocument.LoadXml(USDXml);
            XmlDocument euroXmlDocument = new XmlDocument();
            euroXmlDocument.LoadXml(EuroXML);

            XmlNode xmlNode = usdXmlDocument.SelectSingleNode("Valute/Value");

             _usdCurrency = Convert.ToDecimal(xmlNode.InnerText);
            xmlNode = euroXmlDocument.SelectSingleNode("Valute/Value");
            _euroCurrency = Convert.ToDecimal(xmlNode.InnerText);
        }


       public decimal GetUsdCurrency()
       {
           return _usdCurrency;
       }

       public decimal GetEuroCurrency()
       {
           return _euroCurrency;
       }

       public decimal GetCurrencyByCode(Currency currency)
       {
           switch (currency)
           {
                case Currency.Usd:
                   return _usdCurrency;
                case Currency.Euro:
                   return _euroCurrency;
                default:
                   return 1;
           }
       }

       public decimal[] GetUsdAndEuroCurrency()
       {
           return new decimal[] {_usdCurrency,_euroCurrency};
       }
    }
}
